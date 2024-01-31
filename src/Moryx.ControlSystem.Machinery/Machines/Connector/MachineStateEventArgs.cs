using System;

namespace Moryx.ControlSystem.Machines
{
    /// <summary>
    /// Event args for updated machine state
    /// </summary>
    public class MachineStateEventArgs : EventArgs
    {
        /// <summary>
        /// Updated machine state instance
        /// </summary>
        public MachineState State { get; }

        /// <summary>
        /// Creates a new instance of <see cref="MachineStateEventArgs"/>
        /// </summary>
        public MachineStateEventArgs(MachineState state)
        {
            State = state;
        }
    }
}