namespace Moryx.ControlSystem.Machines
{
    /// <summary>
    /// Enum representing the automatically detected overall state of the manufacturing system
    /// </summary>
    public enum ManufacturingSystemState
    {
        /// <summary>
        /// Initial unset state of the system
        /// </summary>
        Unset = 0,

        /// <summary>
        /// System is producing parts
        /// </summary>
        Production = 1,

        /// <summary>
        /// Some sort of machine/mechanical error
        /// </summary>
        Error = 4,

        /// <summary>
        /// Machine is setup for the next job
        /// </summary>
        Setup = 20,

        /// <summary>
        /// Machine in cleaning up after the last state
        /// </summary>
        CleanUp = 23,

        /// <summary>
        /// Repairs are performed on the machine
        /// </summary>
        Repairs = 41,

        /// <summary>
        /// Machine is on (Power on)
        /// </summary>
        On = 96,

        /// <summary>
        /// Machine is off (Power off)
        /// </summary>
        Off = 97,

        /// <summary>
        /// Machine is waiting
        /// </summary>
        Waiting = 98,

        /// <summary>
        /// Machine stands still by a unknown reason
        /// </summary>
        Unassigned = 30000
    }
}