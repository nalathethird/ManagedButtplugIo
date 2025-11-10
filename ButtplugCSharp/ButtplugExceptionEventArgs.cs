using System;

namespace ButtplugManaged
{
    /// <summary>
    /// Event arguments for Buttplug exception events.
    /// </summary>
    public class ButtplugExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the exception that occurred.
        /// </summary>
        public ButtplugException Exception { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtplugExceptionEventArgs"/> class.
        /// </summary>
        /// <param name="ex">The exception that occurred.</param>
        public ButtplugExceptionEventArgs(ButtplugException ex)
        {
            Exception = ex;
        }
    }
}
