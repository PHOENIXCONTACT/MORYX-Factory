using System;

namespace Moryx.ControlSystem.Machines
{
    /// <summary>
    /// Machine connector module facade to handle machine states
    /// </summary>
    public interface IMachineConnectorExtended: IMachineConnector
    {
        /// <summary>
        /// Event which will be raised when the lock is set or lifted
        /// </summary>
        event EventHandler<bool> LockChanged;
    }
}
