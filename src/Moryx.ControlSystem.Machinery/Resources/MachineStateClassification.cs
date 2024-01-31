using System;

namespace Moryx.ControlSystem.Resources
{
    /// <summary>
    /// Different state flags of the machine or a single part
    /// </summary>
    [Flags]
    public enum MachineStateClassification : uint
    {
        /// <summary>
        /// Defined default for the enum
        /// </summary>
        Unset = 0,

        /// <summary>
        /// Machine is switched off
        /// </summary>
        Off = 1,

        /// <summary>
        /// Machine is waiting
        /// </summary>
        Standby = 1 << 4,

        /// <summary>
        /// Machine is running
        /// </summary>
        Running = 1 << 15,

        /// <summary>
        /// Machine is in observation mode to find errors
        /// or optimize behavior
        /// </summary>
        Observation = 1 << 20,

        /// <summary>
        /// Machine is in error state
        /// </summary>
        Error = 1 << 30,

        /// <summary>
        /// Machine has encountered an emergency stop
        /// </summary>
        Reset = 1U << 31,
    }
}