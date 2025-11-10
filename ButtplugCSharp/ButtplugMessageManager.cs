// <copyright file="ButtplugConnectorMessageSorter.cs" company="Nonpolynomial Labs LLC">
// Buttplug C# Source Code File - Visit https://buttplug.io for more info about the project.
// Copyright (c) Nonpolynomial Labs LLC. All rights reserved.
// Licensed under the BSD 3-Clause license. See LICENSE file in the project root for full license information.
// </copyright>

using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ButtplugManaged
{
    /// <summary>
    /// Manages WebSocket connection and message routing between client and server.
    /// </summary>
    public class ButtplugMessageManager
    {
        /// <summary>
        /// Stores messages waiting for reply from the server.
        /// </summary>
        private readonly ConcurrentDictionary<uint, TaskCompletionSource<MessageBase>> _waitingMsgs = new();

        /// <summary>
        /// Holds count for message IDs, if needed.
        /// </summary>
        private int _counter;
        private readonly ClientWebSocket _webSocket;
        private uint _pingTimeout;
        private readonly Uri _serverUri;
        private CancellationTokenSource _receiveCts;
        private Task _receiveTask;
        private CancellationTokenSource _pingCts;
        private Task _pingTask;

        /// <summary>
        /// Gets the next available message ID. In most cases, setting the message ID is done automatically.
        /// </summary>
        public uint NextMsgId => Convert.ToUInt32(Interlocked.Increment(ref _counter));

        private readonly ButtplugClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtplugMessageManager"/> class.
        /// </summary>
        /// <param name="connectorOptions">WebSocket connection options.</param>
        /// <param name="client">The client instance.</param>
        public ButtplugMessageManager(ButtplugWebsocketConnectorOptions connectorOptions, ButtplugClient client)
        {
            _webSocket = new ClientWebSocket();
            _serverUri = connectorOptions.NetworkAddress;
            _client = client;
        }

        /// <summary>
        /// Connects to the Buttplug server and performs handshake.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Connect()
        {
            _receiveCts = new CancellationTokenSource();
            _pingCts = new CancellationTokenSource();

            await _webSocket.ConnectAsync(_serverUri, CancellationToken.None);

            // Start receiving messages
            _receiveTask = Task.Run(() => ReceiveLoop(_receiveCts.Token));

            var result = await SendClientMessage(new RequestServerInfo() { ClientName = _client.Name, MessageVersion = 2 });

            if (result is ServerInfo serverInfo)
            {
                Console.WriteLine(serverInfo.MaxPingTime);
                _pingTimeout = serverInfo.MaxPingTime;
                _pingTask = Task.Run(() => PingLoop(_pingCts.Token));

                var result2 = await SendClientMessage(new RequestDeviceList() { });

                if (result2 is DeviceList list)
                {
                    foreach (var device in list.Devices)
                    {
                        AddDevice(device);
                    }
                }
            }
        }

        private async Task ReceiveLoop(CancellationToken cancellationToken)
        {
            var buffer = new byte[8192];
            var messageBuilder = new StringBuilder();

            try
            {
                while (_webSocket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
                {
                    var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                        _client.OnServerDisconnect(this, EventArgs.Empty);
                        break;
                    }

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));

                        if (result.EndOfMessage)
                        {
                            var messageText = messageBuilder.ToString();
                            messageBuilder.Clear();
                            ProcessMessage(messageText);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Normal cancellation
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebSocket receive error: {ex.Message}");
                _client.OnServerDisconnect(this, EventArgs.Empty);
            }
        }

        private void ProcessMessage(string messageText)
        {
            List<Message> messages = JsonConvert.DeserializeObject<List<Message>>(messageText);
            foreach (var message in messages)
            {
                foreach (var item in typeof(Message).GetProperties())
                {
                    if (item.GetValue(message) is MessageBase messageBase)
                        CheckMessage(messageBase);
                }
            }
        }

        private async Task PingLoop(CancellationToken cancellationToken)
        {
            bool wspings = _pingTimeout == 0;
            if (wspings) _pingTimeout = 1000 * 10 * 2;

            try
            {
                while (_webSocket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
                {
                    if (!wspings)
                    {
                        await SendClientMessage(new Ping());
                    }
                    await Task.Delay((int)_pingTimeout / 2, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                // Normal cancellation
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ping error: {ex.Message}");
            }
        }

        /// <summary>
        /// Disconnects from the Buttplug server.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Disconnect()
        {
            _pingCts?.Cancel();
            _receiveCts?.Cancel();

            if (_webSocket.State == WebSocketState.Open)
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client disconnecting", CancellationToken.None);
            }

            _webSocket.Dispose();
        }

        /// <summary>
        /// Shuts down the message manager and cancels all pending operations.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Shutdown()
        {
            // If we've somehow destructed while holding tasks, throw exceptions at all of them.
            foreach (var task in _waitingMsgs.Values)
            {
                task.TrySetException(new ButtplugConnectorException("Sorter has been destroyed with live tasks still in queue, most likely due to a disconnection."));
            }

            await Disconnect();
        }

        /// <summary>
        /// Sends a message to the server and waits for a response.
        /// </summary>
        /// <param name="aMsg">The message to send.</param>
        /// <returns>The server's response message.</returns>
        public async Task<MessageBase> SendClientMessage(MessageBase aMsg)
        {
            var id = NextMsgId;
            // The client always increments the IDs on outgoing messages
            aMsg.Id = id;

            var promise = new TaskCompletionSource<MessageBase>();
            _waitingMsgs.TryAdd(id, promise);
            
            string jsonString = JsonConvert.SerializeObject(new List<Message>() { Message.From(aMsg) }, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var bytes = Encoding.UTF8.GetBytes(jsonString);
            await _webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);

            return await promise.Task;
        }

        /// <summary>
        /// Processes an incoming message from the server.
        /// </summary>
        /// <param name="aMsg">The message to process.</param>
        public void CheckMessage(MessageBase aMsg)
        {
            // We'll never match a system message, those are server -> client only.
            if (aMsg is Error error)
            {
                if (error.ErrorCode == Error.ErrorCodeEnum.ERROR_PING)
                {
                    _client.OnPingTimeout(this, null);
                }
                _client.OnErrorReceived(this, new ButtplugExceptionEventArgs(ButtplugException.FromError(error)));
            }

            if (aMsg.Id == 0)
            {
                if (aMsg is DeviceAdded deviceAdded)
                {
                    AddDevice(deviceAdded);

                }
                if (aMsg is DeviceRemoved deviceRemoved)
                {
                    var device = _client.Devices.SingleOrDefault(x => x.Index == deviceRemoved.DeviceIndex);
                    if (device != null)
                        _client.OnDeviceRemoved(_client, new DeviceRemovedEventArgs(device));
                    else
                        _client.OnErrorReceived(this, new ButtplugExceptionEventArgs(new ButtplugDeviceException($"Cannot remove device index {deviceRemoved.DeviceIndex}, device not found.")));
                }

                if (aMsg is ScanningFinished)
                    _client.OnScanningFinished(_client, new EventArgs());

                return;
            }

            // If we haven't gotten a system message and we're not currently waiting for the message
            // id, throw.
            if (!_waitingMsgs.TryRemove(aMsg.Id, out var queued))
            {
                throw new ButtplugMessageException("Message with non-matching ID received.");
            }

            if (aMsg is Error error2)
            {
                queued.SetException(ButtplugException.FromError(error2));
            }
            else
            {
                queued.SetResult(aMsg);
            }
        }

        private void AddDevice(DeviceAdded deviceAdded)
        {
            if (_client.Devices.Any(x => x.Index == deviceAdded.DeviceIndex))
                _client.OnErrorReceived(this, new ButtplugExceptionEventArgs(new ButtplugDeviceException("A duplicate device index was received. This is most likely a bug, please file at https://github.com/buttplugio/buttplug-rs-ffi")));
            else
                _client.OnDeviceAdded(_client, new DeviceAddedEventArgs(new ButtplugClientDevice(this, deviceAdded.DeviceIndex, deviceAdded.DeviceName, deviceAdded.DeviceMessages)));
        }
    }
}
