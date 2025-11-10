using System;

namespace ButtplugManaged
{
    /// <summary>
    /// Exception thrown when message parsing or validation fails.
    /// </summary>
    public class ButtplugMessageException : ButtplugException
    {
        /// <inheritdoc />
        public ButtplugMessageException()
        {
        }

        /// <inheritdoc />
        public ButtplugMessageException(string aMessage) : base(aMessage)
        {
        }

        /// <inheritdoc />
        public ButtplugMessageException(string aMessage, Exception aInner) : base(aMessage, aInner)
        {
        }
    }
}