using System;
using System.Collections.Generic;

namespace Moryx.ControlSystem.Machines
{
    /// <summary>
    /// Machine connector module facade to handle machine states
    /// </summary>
    public interface IMachineConnector
    {
        /// <summary>
        /// Returns the current machine state of this machine
        /// </summary>
        MachineState CurrentState { get; }

        /// <summary>
        /// Indicator if the MachineState is locked to avoid automatic state changes
        /// </summary>
        bool IsLocked { get; }

        /// <summary>
        /// Returns a list of all available states
        /// </summary>
        IReadOnlyList<MachineState> AvailableStates { get; }

        /// <summary>
        /// Unlocks the connector to handle also automatic states
        /// </summary>
        void Unlock();

        /// <summary>
        /// Returns an information if the state can be set as the current state
        /// </summary>
        MachineStateChangeContext GetMachineStateChangeContext(MachineState state);

        /// <summary>
        /// Sets the given state as the current
        /// </summary>
        bool SetState(MachineState state);

        /// <summary>
        /// Adds a new state to the machine
        /// </summary>
        /// <param name="systemState">Corresponding system state</param>
        /// <param name="name">Name of the state</param>
        /// <param name="colorCode">Code of the color for this state</param>
        /// <param name="role">Role of the state</param>
        MachineState AddState(ManufacturingSystemState? systemState, string name, string colorCode, MachineStateRole role);

        /// <summary>
        /// Adds a new state to the machine
        /// </summary>
        /// <param name="systemState">Corresponding system state</param>
        /// <param name="name">Name of the state</param>
        /// <param name="colorCode">Code of the color for this state</param>
        /// <param name="role">Role of the state</param>
        /// <param name="context">Additional information of the state</param>
        MachineState AddState(ManufacturingSystemState? systemState, string name, string colorCode, MachineStateRole role, IMachineStateContext context);

        /// <summary>
        /// Will be raised if the machine state was changed
        /// </summary>
        event EventHandler<MachineStateEventArgs> MachineStateChanged;

        /// <summary>
        /// Event which will be raised when the machine state should be changed
        /// </summary>
        event EventHandler<MachineStateChangeRequestEventArgs> MachineStateChangeRequest;
    }
}
