using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ButtplugManaged
{
    /// <summary>
    /// Represents a connected device with control methods for vibration, rotation, linear movement, and sensor reading.
    /// </summary>
    public class ButtplugClientDevice
    {
        /// <summary>
        /// The device index, which uniquely identifies the device on the server.
        /// </summary>
        /// <remarks>
        /// If a device is removed, this may be the only populated field. If the same device
        /// reconnects, the index should be reused.
        /// </remarks>
        public uint Index { get; }

        /// <summary>
        /// The device name, which usually contains the device brand and model.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The Buttplug Protocol messages supported by this device, with additional attributes.
        /// </summary>
        public Dictionary<string, DeviceMessagesDetails> AllowedMessages { get; }

        private readonly ButtplugMessageManager _manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtplugClientDevice"/> class, using
        /// discrete parameters.
        /// </summary>
        /// <param name="manager">The message manager for communication.</param>
        /// <param name="aIndex">The device index.</param>
        /// <param name="aName">The device name.</param>
        /// <param name="aAllowedMessages">The device allowed message list, with corresponding attributes.</param>
        internal ButtplugClientDevice(ButtplugMessageManager manager,
            uint aIndex,
            string aName,
            Dictionary<string, DeviceMessagesDetails> aAllowedMessages)
        {
            _manager = manager;
            Index = aIndex;
            Name = aName;
            AllowedMessages = aAllowedMessages;
        }

        /// <summary>
        /// Determines whether this device is equal to another device based on index.
        /// </summary>
        /// <param name="aDevice">The device to compare with.</param>
        /// <returns>True if the devices have the same index; otherwise, false.</returns>
        public bool Equals(ButtplugClientDevice aDevice)
        {
            return Index == aDevice.Index;
        }

        /// <summary>
        /// Sends a vibration command to all motors at the same speed.
        /// </summary>
        /// <param name="aSpeed">Vibration speed (0.0 to 1.0).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SendVibrateCmd(double aSpeed)
        {
            // If the message is missing from our dict, we should still send anyways just to let the rust library throw.
            var count = 1u;
            if (AllowedMessages.ContainsKey(DeviceMessages.VibrateCmd))
            {
                count = AllowedMessages[DeviceMessages.VibrateCmd].FeatureCount;
            }

            // There is probably a cleaner, LINQyer way to do this but ugh don't care.
            var commandDict = new Dictionary<uint, double>();
            for (var i = 0u; i < count; ++i)
            {
                commandDict.Add(i, aSpeed);
            }

            return SendVibrateCmd(commandDict);
        }

        /// <summary>
        /// Sends a vibration command to all motors with specified speeds.
        /// </summary>
        /// <param name="aCmds">An enumerable collection of speeds for each motor.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SendVibrateCmd(IEnumerable<double> aCmds)
        {
            return SendVibrateCmd(aCmds.Select((cmd, index) => (cmd, index)).ToDictionary(x => (uint)x.index, x => x.cmd));
        }

        /// <summary>
        /// Sends a vibration command to specified motors with individual speeds.
        /// </summary>
        /// <param name="aCmds">A dictionary with motor index as the key and speed as the value.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SendVibrateCmd(Dictionary<uint, double> aCmds)
        {
            VibrateCmd vibrateMessage = new VibrateCmd
            {
                DeviceIndex = Index,
                Speeds = []
            };
            foreach (var command in aCmds)
            {
                vibrateMessage.Speeds.Add(new VibrateSpeed() { Index = command.Key, Speed = command.Value });
            }
            return _manager.SendClientMessage(vibrateMessage);
        }

        /// <summary>
        /// Sends a rotation command to all rotation motors with the same speed and direction.
        /// </summary>
        /// <param name="aSpeed">The speed of rotation.</param>
        /// <param name="aClockwise">True if rotating clockwise, false otherwise.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SendRotateCmd(double aSpeed, bool aClockwise)
        {
            // If the message is missing from our dict, we should still send anyways just to let the rust library throw.
            var count = 1u;
            if (AllowedMessages.ContainsKey(DeviceMessages.RotateCmd))
            {
                count = AllowedMessages[DeviceMessages.RotateCmd].FeatureCount;
            }

            // There is probably a cleaner, LINQyer way to do this but ugh don't care.
            var commandDict = new Dictionary<uint, (double, bool)>();
            for (var i = 0u; i < count; ++i)
            {
                commandDict.Add(i, (aSpeed, aClockwise));
            }

            return SendRotateCmd(commandDict);
        }

        /// <summary>
        /// Sends a rotation command to all rotation motors with specified speeds and directions.
        /// </summary>
        /// <param name="aCmds">An enumerable collection of tuples with speed and direction for each motor.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SendRotateCmd(IEnumerable<(double, bool)> aCmds)
        {
            return SendRotateCmd(aCmds.Select((cmd, index) => (cmd, index)).ToDictionary(x => (uint)x.index, x => x.cmd));
        }

        /// <summary>
        /// Sends a rotation command to specified motors with individual speeds and directions.
        /// </summary>
        /// <param name="aCmds">A dictionary with motor index as the key and a tuple of speed and direction as the value.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SendRotateCmd(Dictionary<uint, (double, bool)> aCmds)
        {
            RotateCmd rotateMessage = new RotateCmd
            {
                DeviceIndex = Index,
                Rotations = []
            };
            foreach (var command in aCmds)
            {
                rotateMessage.Rotations.Add(new Rotations() { Index = command.Key, Speed = command.Value.Item1, Clockwise = command.Value.Item2 });
            }
            return _manager.SendClientMessage(rotateMessage);
        }

        /// <summary>
        /// Sends a linear command to all linear motors with the same duration and position.
        /// </summary>
        /// <param name="aDuration">The duration of the movement.</param>
        /// <param name="aPosition">The target position.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SendLinearCmd(uint aDuration, double aPosition)
        {
            // If the message is missing from our dict, we should still send anyways just to let the rust library throw.
            var count = 1u;
            if (AllowedMessages.ContainsKey(DeviceMessages.LinearCmd))
            {
                count = AllowedMessages[DeviceMessages.LinearCmd].FeatureCount;
            }

            // There is probably a cleaner, LINQyer way to do this but ugh don't care.
            var commandDict = new Dictionary<uint, (uint, double)>();
            for (var i = 0u; i < count; ++i)
            {
                commandDict.Add(i, (aDuration, aPosition));
            }

            return SendLinearCmd(commandDict);
        }

        /// <summary>
        /// Sends a linear command to all linear motors with specified durations and positions.
        /// </summary>
        /// <param name="aCmds">An enumerable collection of tuples with duration and position for each motor.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SendLinearCmd(IEnumerable<(uint, double)> aCmds)
        {
            return SendLinearCmd(aCmds.Select((cmd, index) => (cmd, index)).ToDictionary(x => (uint)x.index, x => x.cmd));
        }

        /// <summary>
        /// Sends a linear command to specified motors with individual durations and positions.
        /// </summary>
        /// <param name="aCmds">A dictionary with motor index as the key and a tuple of duration and position as the value.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SendLinearCmd(Dictionary<uint, (uint, double)> aCmds)
        {
            LinearCmd linearMessage = new LinearCmd
            {
                DeviceIndex = Index,
                Vectors = []
            };
            foreach (var command in aCmds)
            {
                linearMessage.Vectors.Add(new LinearVector() { Index = command.Key, Duration = command.Value.Item1, Position = command.Value.Item2 });
            }
            return _manager.SendClientMessage(linearMessage);
        }

        /// <summary>
        /// Requests the battery level of the device.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with the battery level as the result.</returns>
        public async Task<double> SendBatteryLevelCmd()
        {
            var result = await _manager.SendClientMessage(new BatteryLevelCmd() { DeviceIndex = Index });
            if (result is BatteryLevelReading reading)
                return reading.BatteryLevel;

            throw new ButtplugDeviceException($"Expected message type of BatteryLevelReading not received, got {result} instead.");
        }

        /// <summary>
        /// Requests the RSSI-based battery level of the device.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with the RSSI-based battery level as the result.</returns>
        public async Task<int> SendRSSIBatteryLevelCmd()
        {
            var result = await _manager.SendClientMessage(new RSSILevelCmd() { DeviceIndex = Index });
            if (result is RSSILevelReading reading)
                return reading.RSSILevel;

            throw new ButtplugDeviceException($"Expected message type of RssiLevelReading not received, got {result} instead.");
        }

        /// <summary>
        /// Sends a raw read command to the device.
        /// </summary>
        /// <param name="aEndpoint">The endpoint to read from.</param>
        /// <param name="aExpectedLength">The expected length of the data.</param>
        /// <param name="aTimeout">The timeout for the read operation.</param>
        /// <returns>A task representing the asynchronous operation, with the read data as the result.</returns>
        public async Task<byte[]> SendRawReadCmd(string aEndpoint, uint aExpectedLength, uint aTimeout)
        {
            var task = _manager.SendClientMessage(new RawReadCmd() { DeviceIndex = Index, Endpoint = aEndpoint, ExpectedLength = aExpectedLength });
            if (await Task.WhenAny(task, Task.Delay((int)aTimeout)) == task)
            {
                var result = await task;

                if (result is RawReading reading)
                {
                    return reading.Data.Select(x => (byte)x).ToArray();
                }

                throw new ButtplugDeviceException($"Expected message type of RawReading not received, got {result} instead.");
            }
            else
            {
                throw new ButtplugDeviceException($"No Message returned");
            }

        }

        /// <summary>
        /// Sends a raw write command to the device.
        /// </summary>
        /// <param name="aEndpoint">The endpoint to write to.</param>
        /// <param name="aData">The data to write.</param>
        /// <param name="aWriteWithResponse">True to require a response from the device, false otherwise.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SendRawWriteCmd(string aEndpoint, byte[] aData, bool aWriteWithResponse)
        {
            return _manager.SendClientMessage(new RawWriteCmd() { DeviceIndex = Index, Endpoint = aEndpoint, Data = aData.Select(x => (int)x).ToList(), WriteWithResponse = aWriteWithResponse });
        }

        /// <summary>
        /// Subscribes to notifications from a raw device endpoint.
        /// </summary>
        /// <param name="aEndpoint">The endpoint to subscribe to.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SendRawSubscribeCmd(string aEndpoint)
        {
            return _manager.SendClientMessage(new RawSubscribeCmd() { DeviceIndex = Index, Endpoint = aEndpoint });
        }

        /// <summary>
        /// Unsubscribes from notifications from a raw device endpoint.
        /// </summary>
        /// <param name="aEndpoint">The endpoint to unsubscribe from.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SendRawUnsubscribeCmd(string aEndpoint)
        {
            return _manager.SendClientMessage(new RawUnsubscribeCmd() { DeviceIndex = Index, Endpoint = aEndpoint });
        }

        /// <summary>
        /// Stops all device operations immediately.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SendStopDeviceCmd()
        {
            return _manager.SendClientMessage(new StopDeviceCmd() { DeviceIndex = Index });
        }
    }
}
