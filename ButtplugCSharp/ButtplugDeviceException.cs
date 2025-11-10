using System;

namespace ButtplugManaged
{
    /// <summary>
    /// Exception thrown when device communication or operation fails.
    /// </summary>
    public class ButtplugDeviceException : ButtplugException
    {
        /// <inheritdoc />
        public ButtplugDeviceException()
        {
        }

        /// <inheritdoc />
        public ButtplugDeviceException(string aMessage) : base(aMessage)
        {
        }

        /// <inheritdoc />
        public ButtplugDeviceException(string aMessage, Exception aInner) : base(aMessage, aInner)
        {
        }
    }
}