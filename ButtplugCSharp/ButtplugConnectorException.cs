using System;

namespace ButtplugManaged
{
    /// <summary>
    /// Exception thrown when connection to the server fails.
    /// </summary>
    public class ButtplugConnectorException : ButtplugException
    {
        /// <inheritdoc />
        public ButtplugConnectorException()
        {
        }

        /// <inheritdoc />
        public ButtplugConnectorException(string aMessage) : base(aMessage)
        {
        }

        /// <inheritdoc />
        public ButtplugConnectorException(string aMessage, Exception aInner) : base(aMessage, aInner)
        {
        }
    }
}
