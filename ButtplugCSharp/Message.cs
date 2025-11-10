using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ButtplugManaged
{

    /// <summary>
    /// Wrapper class for Buttplug protocol messages.
    /// </summary>
    public class Message
    {

        /// <summary>
        /// Creates a Message wrapper from a MessageBase instance.
        /// </summary>
        /// <param name="messageBase">The message to wrap.</param>
        /// <returns>A Message instance containing the specified message.</returns>
        public static Message From(MessageBase messageBase)
        {
            // Only handle Client messages
            Message message = new();

            switch (messageBase)
            {
                case Ping ping:
                    message.Ping = ping;
                    break;
                case RequestServerInfo requestServerInfo:
                    message.RequestServerInfo = requestServerInfo;
                    break;
                case StartScanning startScanning:
                    message.StartScanning = startScanning;
                    break;
                case StopScanning stopScanning:
                    message.StopScanning = stopScanning;
                    break;
                case RequestDeviceList requestDeviceList:
                    message.RequestDeviceList = requestDeviceList;
                    break;
                case RawWriteCmd rawWriteCmd:
                    message.RawWriteCmd = rawWriteCmd;
                    break;
                case RawReadCmd rawReadCmd:
                    message.RawReadCmd = rawReadCmd;
                    break;
                case RawSubscribeCmd rawSubscribeCmd:
                    message.RawSubscribeCmd = rawSubscribeCmd;
                    break;
                case RawUnsubscribeCmd rawUnsubscribeCmd:
                    message.RawUnsubscribeCmd = rawUnsubscribeCmd;
                    break;
                case StopDeviceCmd stopDeviceCmd:
                    message.StopDeviceCmd = stopDeviceCmd;
                    break;
                case StopAllDevices stopAllDevices:
                    message.StopAllDevices = stopAllDevices;
                    break;
                case VibrateCmd vibrateCmd:
                    message.VibrateCmd = vibrateCmd;
                    break;
                case LinearCmd linearCmd:
                    message.LinearCmd = linearCmd;
                    break;
                case RotateCmd rotateCmd:
                    message.RotateCmd = rotateCmd;
                    break;
                case BatteryLevelCmd batteryLevelCmd:
                    message.BatteryLevelCmd = batteryLevelCmd;
                    break;
                case RSSILevelCmd rssiLevelCmd:
                    message.RSSILevelCmd = rssiLevelCmd;
                    break;
            }

            return message;
        }

        /// <summary>
        /// Gets or sets the Ok status message.
        /// </summary>
        public Ok Ok { get; set; }

        /// <summary>
        /// Gets or sets the Error message.
        /// </summary>
        public Error Error { get; set; }

        /// <summary>
        /// Gets or sets the Ping message.
        /// </summary>
        public Ping Ping { get; set; }

        /// <summary>
        /// Gets or sets the RequestServerInfo handshake message.
        /// </summary>
        public RequestServerInfo RequestServerInfo { get; set; }

        /// <summary>
        /// Gets or sets the ServerInfo handshake response.
        /// </summary>
        public ServerInfo ServerInfo { get; set; }

        /// <summary>
        /// Gets or sets the StartScanning enumeration message.
        /// </summary>
        public StartScanning StartScanning { get; set; }

        /// <summary>
        /// Gets or sets the StopScanning enumeration message.
        /// </summary>
        public StopScanning StopScanning { get; set; }

        /// <summary>
        /// Gets or sets the ScanningFinished notification message.
        /// </summary>
        public ScanningFinished ScanningFinished { get; set; }

        /// <summary>
        /// Gets or sets the RequestDeviceList enumeration message.
        /// </summary>
        public RequestDeviceList RequestDeviceList { get; set; }

        /// <summary>
        /// Gets or sets the DeviceList response message.
        /// </summary>
        public DeviceList DeviceList { get; set; }

        /// <summary>
        /// Gets or sets the DeviceAdded notification message.
        /// </summary>
        public DeviceAdded DeviceAdded { get; set; }

        /// <summary>
        /// Gets or sets the DeviceRemoved notification message.
        /// </summary>
        public DeviceRemoved DeviceRemoved { get; set; }

        /// <summary>
        /// Gets or sets the RawWriteCmd message for raw device communication.
        /// </summary>
        public RawWriteCmd RawWriteCmd { get; set; }

        /// <summary>
        /// Gets or sets the RawReadCmd message for raw device communication.
        /// </summary>
        public RawReadCmd RawReadCmd { get; set; }

        /// <summary>
        /// Gets or sets the RawReading response from raw device communication.
        /// </summary>
        public RawReading RawReading { get; set; }

        /// <summary>
        /// Gets or sets the RawSubscribeCmd message for subscribing to raw device data.
        /// </summary>
        public RawSubscribeCmd RawSubscribeCmd { get; set; }

        /// <summary>
        /// Gets or sets the RawUnsubscribeCmd message for unsubscribing from raw device data.
        /// </summary>
        public RawUnsubscribeCmd RawUnsubscribeCmd { get; set; }

        /// <summary>
        /// Gets or sets the StopDeviceCmd message to stop a specific device.
        /// </summary>
        public StopDeviceCmd StopDeviceCmd { get; set; }

        /// <summary>
        /// Gets or sets the StopAllDevices message to stop all connected devices.
        /// </summary>
        public StopAllDevices StopAllDevices { get; set; }

        /// <summary>
        /// Gets or sets the VibrateCmd message for vibration control.
        /// </summary>
        public VibrateCmd VibrateCmd { get; set; }

        /// <summary>
        /// Gets or sets the LinearCmd message for linear actuator control.
        /// </summary>
        public LinearCmd LinearCmd { get; set; }

        /// <summary>
        /// Gets or sets the RotateCmd message for rotation control.
        /// </summary>
        public RotateCmd RotateCmd { get; set; }

        /// <summary>
        /// Gets or sets the BatteryLevelCmd message to request battery level.
        /// </summary>
        public BatteryLevelCmd BatteryLevelCmd { get; set; }

        /// <summary>
        /// Gets or sets the BatteryLevelReading response with battery level information.
        /// </summary>
        public BatteryLevelReading BatteryLevelReading { get; set; }

        /// <summary>
        /// Gets or sets the RSSILevelCmd message to request signal strength.
        /// </summary>
        public RSSILevelCmd RSSILevelCmd { get; set; }

        /// <summary>
        /// Gets or sets the RSSILevelReading response with signal strength information.
        /// </summary>
        public RSSILevelReading RSSILevelReading { get; set; }

    }

    /// <summary>
    /// Base class for all Buttplug protocol messages.
    /// </summary>
    public class MessageBase
    {
        /// <summary>
        /// Gets or sets the message ID for request/response correlation.
        /// </summary>
        public uint Id { get; set; }
    }

    #region Status Messages

    /// <summary>
    /// Represents a successful operation response.
    /// </summary>
    public class Ok : MessageBase
    {
    }

    /// <summary>
    /// Represents an error response from the server.
    /// </summary>
    public class Error : MessageBase
    {
        /// <summary>
        /// Gets or sets the human-readable error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the error code category.
        /// </summary>
        public ErrorCodeEnum ErrorCode { get; set; }

        /// <summary>
        /// Enumeration of error code categories.
        /// </summary>
        public enum ErrorCodeEnum
        {
            /// <summary>Unknown error type.</summary>
            ERROR_UNKNOWN,
            /// <summary>Error during initialization.</summary>
            ERROR_INIT,
            /// <summary>Error with ping/pong keep-alive.</summary>
            ERROR_PING,
            /// <summary>Error with message format or parsing.</summary>
            ERROR_MSG,
            /// <summary>Error with device communication.</summary>
            ERROR_DEVICE
        }
    }

    /// <summary>
    /// Keep-alive ping message to maintain connection.
    /// </summary>
    public class Ping : MessageBase
    {
    }

    #endregion

    #region Handshake Messages

    /// <summary>
    /// Server information response during handshake.
    /// </summary>
    public class ServerInfo : MessageBase
    {
        /// <summary>
        /// Gets or sets the server name.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Gets or sets the protocol version supported by the server.
        /// </summary>
        public uint MessageVersion { get; set; }

        /// <summary>
        /// Gets or sets the maximum ping timeout in milliseconds.
        /// </summary>
        public uint MaxPingTime { get; set; }
    }

    /// <summary>
    /// Request server information during handshake.
    /// </summary>
    public class RequestServerInfo : MessageBase
    {
        /// <summary>
        /// Gets or sets the client name.
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets the protocol version requested by the client.
        /// </summary>
        public uint MessageVersion { get; set; }
    }
    #endregion

    #region Enumeration Messages

    /// <summary>
    /// Request to start scanning for devices.
    /// </summary>
    public class StartScanning : MessageBase
    {
    }

    /// <summary>
    /// Request to stop scanning for devices.
    /// </summary>
    public class StopScanning : MessageBase
    {
    }

    /// <summary>
    /// Notification that device scanning has finished.
    /// </summary>
    public class ScanningFinished : MessageBase
    {
    }

    /// <summary>
    /// Request the list of currently connected devices.
    /// </summary>
    public class RequestDeviceList : MessageBase
    {
    }

    /// <summary>
    /// Response containing the list of connected devices.
    /// </summary>
    public class DeviceList : MessageBase
    {
        /// <summary>
        /// Gets or sets the list of connected devices.
        /// </summary>
        public List<DeviceAdded> Devices { get; set; }
    }

    /// <summary>
    /// Notification that a device has been connected.
    /// </summary>
    public class DeviceAdded : MessageBase
    {
        /// <summary>
        /// Gets or sets the unique device index.
        /// </summary>
        public uint DeviceIndex { get; set; }

        /// <summary>
        /// Gets or sets the device name.
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of supported messages and their attributes.
        /// </summary>
        public Dictionary<string, DeviceMessagesDetails> DeviceMessages { get; set; }
    }

    /// <summary>
    /// Constants for device message type names.
    /// </summary>
    public class DeviceMessages
    {
        /// <summary>Vibration command message type.</summary>
        public const string VibrateCmd = "VibrateCmd";

        /// <summary>Battery level command message type.</summary>
        public const string BatteryLevelCmd = "BatteryLevelCmd";

        /// <summary>Stop device command message type.</summary>
        public const string StopDeviceCmd = "StopDeviceCmd";

        /// <summary>Linear command message type.</summary>
        public const string LinearCmd = "LinearCmd";

        /// <summary>Rotate command message type.</summary>
        public const string RotateCmd = "RotateCmd";
    }

    /// <summary>
    /// Details about device message capabilities.
    /// </summary>
    public class DeviceMessagesDetails
    {
        /// <summary>
        /// Gets or sets the number of features (e.g., number of motors).
        /// </summary>
        public uint FeatureCount { get; set; }

        /// <summary>
        /// Gets or sets the list of step counts for each feature.
        /// </summary>
        public List<uint> StepCount { get; set; }

        /// <summary>
        /// Gets or sets the list of available endpoints.
        /// </summary>
        public List<string> Endpoints { get; set; }

        /// <summary>
        /// Gets or sets the maximum duration values for each feature.
        /// </summary>
        public List<uint> MaxDuration { get; set; }
    }

    /// <summary>
    /// Notification that a device has been disconnected.
    /// </summary>
    public class DeviceRemoved : MessageBase
    {
        /// <summary>
        /// Gets or sets the index of the removed device.
        /// </summary>
        public uint DeviceIndex { get; set; }
    }

    #endregion

    #region Raw Messages

    /// <summary>
    /// Command to write raw data to a device endpoint.
    /// </summary>
    public class RawWriteCmd : MessageBase
    {
        /// <summary>
        /// Gets or sets the target device index.
        /// </summary>
        public uint DeviceIndex { get; set; }

        /// <summary>
        /// Gets or sets the endpoint name.
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// Gets or sets the data to write.
        /// </summary>
        public List<int> Data { get; set; }

        /// <summary>
        /// Gets or sets whether to wait for a response from the device.
        /// </summary>
        public bool WriteWithResponse { get; set; }
    }

    /// <summary>
    /// Command to read raw data from a device endpoint.
    /// </summary>
    public class RawReadCmd : MessageBase
    {
        /// <summary>
        /// Gets or sets the target device index.
        /// </summary>
        public uint DeviceIndex { get; set; }

        /// <summary>
        /// Gets or sets the endpoint name.
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// Gets or sets the expected number of bytes to read.
        /// </summary>
        public uint ExpectedLength { get; set; }

        /// <summary>
        /// Gets or sets whether to wait for data to be available.
        /// </summary>
        public bool WaitForData { get; set; }
    }

    /// <summary>
    /// Response containing raw data read from a device.
    /// </summary>
    public class RawReading : MessageBase
    {
        /// <summary>
        /// Gets or sets the device index.
        /// </summary>
        public uint DeviceIndex { get; set; }

        /// <summary>
        /// Gets or sets the endpoint name.
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// Gets or sets the data read from the device.
        /// </summary>
        public List<int> Data { get; set; }
    }

    /// <summary>
    /// Command to subscribe to notifications from a device endpoint.
    /// </summary>
    public class RawSubscribeCmd : MessageBase
    {
        /// <summary>
        /// Gets or sets the target device index.
        /// </summary>
        public uint DeviceIndex { get; set; }

        /// <summary>
        /// Gets or sets the endpoint name.
        /// </summary>
        public string Endpoint { get; set; }
    }

    /// <summary>
    /// Command to unsubscribe from notifications from a device endpoint.
    /// </summary>
    public class RawUnsubscribeCmd : MessageBase
    {
        /// <summary>
        /// Gets or sets the target device index.
        /// </summary>
        public uint DeviceIndex { get; set; }

        /// <summary>
        /// Gets or sets the endpoint name.
        /// </summary>
        public string Endpoint { get; set; }
    }

    #endregion

    #region Generic Device Messages

    /// <summary>
    /// Command to stop a specific device.
    /// </summary>
    public class StopDeviceCmd : MessageBase
    {
        /// <summary>
        /// Gets or sets the target device index.
        /// </summary>
        public uint DeviceIndex { get; set; }
    }

    /// <summary>
    /// Command to stop all connected devices.
    /// </summary>
    public class StopAllDevices : MessageBase
    {
    }

    /// <summary>
    /// Command to control device vibration.
    /// </summary>
    public class VibrateCmd : MessageBase
    {
        /// <summary>
        /// Gets or sets the target device index.
        /// </summary>
        public uint DeviceIndex { get; set; }

        /// <summary>
        /// Gets or sets the list of vibration speeds for each motor.
        /// </summary>
        public List<VibrateSpeed> Speeds { get; set; }
    }

    /// <summary>
    /// Represents vibration speed for a specific motor.
    /// </summary>
    public class VibrateSpeed
    {
        /// <summary>
        /// Gets or sets the motor index.
        /// </summary>
        public uint Index { get; set; }

        /// <summary>
        /// Gets or sets the vibration speed (0.0 to 1.0).
        /// </summary>
        public double Speed { get; set; }
    }

    /// <summary>
    /// Command to control linear actuators (strokers).
    /// </summary>
    public class LinearCmd : MessageBase
    {
        /// <summary>
        /// Gets or sets the target device index.
        /// </summary>
        public uint DeviceIndex { get; set; }

        /// <summary>
        /// Gets or sets the list of linear movements.
        /// </summary>
        public List<LinearVector> Vectors { get; set; }
    }

    /// <summary>
    /// Represents a linear movement to a position over time.
    /// </summary>
    public class LinearVector
    {
        /// <summary>
        /// Gets or sets the actuator index.
        /// </summary>
        public uint Index { get; set; }

        /// <summary>
        /// Gets or sets the movement duration in milliseconds.
        /// </summary>
        public uint Duration { get; set; }

        /// <summary>
        /// Gets or sets the target position (0.0 to 1.0).
        /// </summary>
        public double Position { get; set; }
    }

    /// <summary>
    /// Command to control device rotation.
    /// </summary>
    public class RotateCmd : MessageBase
    {
        /// <summary>
        /// Gets or sets the target device index.
        /// </summary>
        public uint DeviceIndex { get; set; }

        /// <summary>
        /// Gets or sets the list of rotation commands.
        /// </summary>
        public List<Rotations> Rotations { get; set; }
    }

    /// <summary>
    /// Represents rotation parameters for a motor.
    /// </summary>
    public class Rotations
    {
        /// <summary>
        /// Gets or sets the motor index.
        /// </summary>
        public uint Index { get; set; }

        /// <summary>
        /// Gets or sets the rotation speed (0.0 to 1.0).
        /// </summary>
        public double Speed { get; set; }

        /// <summary>
        /// Gets or sets whether rotation is clockwise.
        /// </summary>
        public bool Clockwise { get; set; }
    }
    #endregion

    #region Generic Sensor Messages

    /// <summary>
    /// Command to request device battery level.
    /// </summary>
    public class BatteryLevelCmd : MessageBase
    {
        /// <summary>
        /// Gets or sets the target device index.
        /// </summary>
        public uint DeviceIndex { get; set; }
    }

    /// <summary>
    /// Response containing device battery level.
    /// </summary>
    public class BatteryLevelReading : MessageBase
    {
        /// <summary>
        /// Gets or sets the device index.
        /// </summary>
        public uint DeviceIndex { get; set; }

        /// <summary>
        /// Gets or sets the battery level (0.0 to 1.0).
        /// </summary>
        public double BatteryLevel { get; set; }
    }

    /// <summary>
    /// Command to request device signal strength (RSSI).
    /// </summary>
    public class RSSILevelCmd : MessageBase
    {
        /// <summary>
        /// Gets or sets the target device index.
        /// </summary>
        public uint DeviceIndex { get; set; }
    }

    /// <summary>
    /// Response containing device signal strength.
    /// </summary>
    public class RSSILevelReading : MessageBase
    {
        /// <summary>
        /// Gets or sets the device index.
        /// </summary>
        public uint DeviceIndex { get; set; }

        /// <summary>
        /// Gets or sets the RSSI level in dBm.
        /// </summary>
        public int RSSILevel { get; set; }
    }

    #endregion
}
