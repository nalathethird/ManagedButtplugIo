using System;

namespace ButtplugManaged
{
    /// <summary>
    /// Exception thrown when an unknown or unexpected error occurs.
    /// </summary>
    public class ButtplugUnknownException : ButtplugException
    {
        /// <inheritdoc />
        public ButtplugUnknownException()
        {
        }

        /// <inheritdoc />
        public ButtplugUnknownException(string aMessage) : base(aMessage)
        {
        }

        /// <inheritdoc />
        public ButtplugUnknownException(string aMessage, Exception aInner) : base(aMessage, aInner)
        {
        }
    }
}
