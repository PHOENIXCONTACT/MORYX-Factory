using Moryx.ControlSystem.Resources;

namespace Moryx.ControlSystem.Machines
{
    /// <summary>
    /// Represents the state of a part of the machine
    /// </summary>
    public readonly struct PartialMachineState
    {
        /// <summary>
        /// Name of the part
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// State of the part
        /// </summary>
        public MachineStateClassification State { get; }

        /// <summary>
        /// Machine part of the state
        /// </summary>
        public int MachinePart { get; }

        /// <summary>
        /// Initialize the <see cref="PartialMachineState"/>
        /// </summary>
        public PartialMachineState(string name, MachineStateClassification stateClassification) : this(name, stateClassification, 0)
        {
        }

        /// <summary>
        /// Initialize the <see cref="PartialMachineState"/>
        /// </summary>
        public PartialMachineState(string name, MachineStateClassification stateClassification, int machinePart)
        {
            Name = name;
            State = stateClassification;
            MachinePart = machinePart;
        }
    }
}