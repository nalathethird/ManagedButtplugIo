# ButtplugManaged - .NET 9 Client Library

[![License](https://img.shields.io/badge/License-BSD%203--Clause-blue.svg)](LICENSE)
[![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4)](https://dotnet.microsoft.com/)

A managed .NET 9 client library for the [Buttplug.io](https://buttplug.io) sex toy control protocol. This library provides a clean, modern async/await interface for connecting to Buttplug servers and controlling devices via WebSocket.

## ? What's New in 2.0.7

- ?? **Upgraded to .NET 9** - Modern runtime and language features
- ?? **Native WebSockets** - Replaced WebSocketSharp with `System.Net.WebSockets.ClientWebSocket`
- ? **Fully Async** - Complete async/await support throughout
- ?? **Modern C# 12** - Collection expressions, switch patterns, and more
- ?? **No External Dependencies** - Except Newtonsoft.Json for serialization

## ?? Installation

### Via NuGet (Coming Soon)
```bash
dotnet add package ButtplugManaged
```

### Via GitHub Releases
1. Download the latest release from [Releases](https://github.com/nalathethird/ManagedButtplugIo/releases)
2. Extract and reference `ButtplugManaged.dll` in your project

## ?? Quick Start

```csharp
using ButtplugManaged;

// Create a client
var client = new ButtplugClient("My Application");

// Set up event handlers
client.DeviceAdded += (sender, args) =>
{
    Console.WriteLine($"Device connected: {args.Device.Name}");
};

// Connect to Buttplug server (e.g., Intiface Central)
var connector = new ButtplugWebsocketConnectorOptions(
    new Uri("ws://localhost:12345")
);
await client.ConnectAsync(connector);

// Start scanning for devices
await client.StartScanningAsync();
await Task.Delay(5000);
await client.StopScanningAsync();

// Control a device
var device = client.Devices.FirstOrDefault();
if (device != null)
{
    await device.SendVibrateCmd(0.5); // 50% speed
    await Task.Delay(2000);
    await device.SendStopDeviceCmd();
}

// Disconnect
await client.DisconnectAsync();
```

## ?? Key Features

### ButtplugClient
- WebSocket connection management
- Device discovery and enumeration
- Event-based device notifications
- Automatic ping/keep-alive handling

### ButtplugClientDevice
- **Vibration Control**: Single or multi-motor vibration
- **Rotation Control**: Clockwise/counter-clockwise rotation
- **Linear Control**: Position-based movement (strokers)
- **Sensor Reading**: Battery level, RSSI
- **Raw Device Access**: Direct Bluetooth endpoint communication

## ?? Server Requirements

Requires a Buttplug server running locally or remotely:

- **[Intiface Central](https://intiface.com/central/)** - Recommended GUI application
- **[Intiface Engine](https://github.com/intiface/intiface-engine)** - CLI server
- Default WebSocket address: `ws://localhost:12345`

## ??? Requirements

- **.NET 9.0** or later
- **Windows, Linux, or macOS**
- A Buttplug server

## ?? Documentation

For detailed API documentation and examples, see:
- [Buttplug Protocol Spec](https://buttplug-spec.docs.buttplug.io/)
- [Buttplug Developer Guide](https://docs.buttplug.io/)
- [DEPLOYMENT_GUIDE.md](DEPLOYMENT_GUIDE.md) - Publishing information

## ?? Migration from WebSocketSharp

**Good news!** The public API is unchanged - only internal implementation improved:

- ? Better async/await patterns
- ? Improved error handling
- ? No external WebSocket dependencies
- ? Better cross-platform support

## ?? Contributing

Contributions welcome! Feel free to submit issues and pull requests.

See [DEPLOYMENT_GUIDE.md](DEPLOYMENT_GUIDE.md) for information on building and publishing.

## ?? License

Buttplug is BSD 3-Clause licensed. More information is available in the LICENSE file.

## ?? Credits

- Original FFI library by [Nonpolynomial Labs](https://nonpolynomial.com/)
- Built on [Buttplug Rust FFI](https://github.com/buttplugio/buttplug-rs-ffi)
- .NET 9 upgrade and native WebSocket implementation

## ?? Links

- [Buttplug.io](https://buttplug.io)
- [GitHub Repository](https://github.com/nalathethird/ManagedButtplugIo)
- [Issue Tracker](https://github.com/nalathethird/ManagedButtplugIo/issues)

---

Made with ?? for the Buttplug community
