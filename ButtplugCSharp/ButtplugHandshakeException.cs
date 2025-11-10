using System;

namespace ButtplugManaged
{
    /// <summary>
    /// Exception thrown when handshake with the server fails.
    /// </summary>
    public class ButtplugHandshakeException : ButtplugException
    {
        /// <inheritdoc />
        public ButtplugHandshakeException()
        {
        }

        /// <inheritdoc />
        public ButtplugHandshakeException(string aMessage) : base(aMessage)
        {
        }

        /// <inheritdoc />
        public ButtplugHandshakeException(string aMessage, Exception aInner) : base(aMessage, aInner)
        {
        }
    }
}