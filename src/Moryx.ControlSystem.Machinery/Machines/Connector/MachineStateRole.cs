using System;

namespace Moryx.ControlSystem.Machines
{
    /// <summary>
    /// Possible roles of a machine state
    /// </summary>
    [Flags]
    public enum MachineStateRole
    {
        /// <summary>
        /// Manual state which can be set manually or automatically
        /// </summary>
        Manual = 1,

        /// <summary>
        /// A state which can only be set automatically
        /// </summary>
        Auto = 2,

        /// <summary>
        /// Marks a role which will be used for a shutdown
        /// </summary>
        Off = 4
    }
}
