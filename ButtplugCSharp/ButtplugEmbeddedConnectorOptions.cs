namespace ButtplugManaged
{
    /// <summary>
    /// Configuration options for embedded Buttplug server (not currently implemented).
    /// </summary>
    public class ButtplugEmbeddedConnectorOptions
    {
        /// <summary>
        /// Gets or sets the server name.
        /// </summary>
        public string ServerName { get; set; } = "Buttplug C# Embedded Server";

        /// <summary>
        /// Gets or sets the device configuration JSON. Empty string means ignore.
        /// </summary>
        public string DeviceConfigJSON { get; set; } = "";

        /// <summary>
        /// Gets or sets the user device configuration JSON. Empty string means ignore.
        /// </summary>
        public string UserDeviceConfigJSON { get; set; } = "";

        /// <summary>
        /// Gets or sets the device communication manager types. 0 means all devices.
        /// </summary>
        public ushort DeviceCommunicationManagerTypes { get; set; } = 0;

        /// <summary>
        /// Gets or sets whether to allow raw device messages. Always opt-in on raw messages.
        /// </summary>
        public bool AllowRawMessages { get; set; } = false;

        /// <summary>
        /// Gets or sets the maximum ping timeout in milliseconds. 0 means no ping timeout.
        /// </summary>
        public uint MaxPingTime { get; set; } = 0;
    }
}
