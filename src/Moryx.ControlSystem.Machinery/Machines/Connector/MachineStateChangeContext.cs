namespace Moryx.ControlSystem.Machines
{
    /// <summary>
    /// Information about changing a machine state
    /// </summary>
    public class MachineStateChangeContext
    {
        /// <summary>
        /// Indicator if the state can be set
        /// </summary>
        public bool CanBeSet { get; set; }

        /// <summary>
        /// If state cannot be set there are a bunch of restrictions
        /// </summary>
        public string[] Restrictions { get; set; }
    }
}