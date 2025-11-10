using System;

namespace ButtplugManaged
{
    /// <summary>
    /// Configuration options for WebSocket connection to Buttplug server.
    /// </summary>
    public class ButtplugWebsocketConnectorOptions
    {
        /// <summary>
        /// Gets or sets the WebSocket server address.
        /// </summary>
        public Uri NetworkAddress { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtplugWebsocketConnectorOptions"/> class.
        /// </summary>
        /// <param name="aAddress">The WebSocket server URI.</param>
        public ButtplugWebsocketConnectorOptions(Uri aAddress)
        {
            NetworkAddress = aAddress;
        }
    }
}
