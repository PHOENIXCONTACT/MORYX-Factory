using System;
using System.Collections.Generic;
using Moryx.AbstractionLayer.Capabilities;

namespace Moryx.ControlSystem.Machines
{
    /// <summary>
    /// Facade to access the overall machine state
    /// </summary>
    public interface IMachineSupervision
    {
        /// <summary>
        /// Automatically determined overall system state
        /// </summary>
        ManufacturingSystemState SystemState { get; }

        /// <summary>
        /// Current state classification of the machine
        /// </summary>
        IReadOnlyList<PartialMachineState> MachineStates { get; }

        /// <summary>
        /// Method to start preparation of a system shutdown
        /// </summary>
        void PrepareSystemShutdown();

        /// <summary>
        /// Event raised when the machine state changed. This could affect previously
        /// estimated efforts
        /// </summary>
        event EventHandler<ManufacturingSystemState> MachineStateChanged;

        /// <summary>
        /// Event raised when one of the partial machine states changed. This could affect previously
        /// estimated efforts
        /// </summary>
        event EventHandler<PartialMachineState> PartialMachineStateChanged;

        /// <summary>
        /// Event raised when a resources capability changed. This could affect previously
        /// estimated efforts
        /// </summary>
        event EventHandler<ICapabilities> CapabilitiesChanged;
    }
}