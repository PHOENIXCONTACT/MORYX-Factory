namespace Moryx.ControlSystem.Machines
{
    /// <summary>
    /// Simple representation of a state for the whole machine
    /// </summary>
    public class MachineState
    {
        /// <summary>
        /// Display name of the state
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// Color code of the state
        /// </summary>
        public virtual string ColorCode { get; protected set; }

        /// <summary>
        /// Manufacturing system state pendant to for mapping
        /// </summary>
        public virtual ManufacturingSystemState? ManufacturingSystemState { get; protected set; }

        /// <summary>
        /// Context object for information storage from external modules
        /// </summary>
        public virtual IMachineStateContext Context { get; protected set; }

        /// <summary>
        /// Role of the state
        /// </summary>
        public virtual MachineStateRole Role { get; protected set; }
    }
}