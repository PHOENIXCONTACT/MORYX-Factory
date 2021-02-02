// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Capabilities;
using Moryx.AbstractionLayer.Recipes;
using Moryx.Collections;
using Moryx.Modules;
using Moryx.Workflows;

namespace Moryx.ControlSystem.Setups
{
    /// <summary>
    /// Trigger to determine if a production recipe requires a setup first
    /// </summary>
    public interface ISetupTrigger : IConfiguredPlugin<SetupTriggerConfig>, ISortableObject
    {
        /// <summary>
        /// Execution timing of the trigger
        /// </summary>
        SetupExecution Execution { get; }

        /// <summary>
        /// Determine the necessary setup action and optionally the capabilities
        /// If capabilities are not used, they should be set to <see cref="NullCapabilities.Instance"/>
        /// </summary>
        SetupEvaluation Evaluate(IProductionRecipe recipe);

        /// <summary>
        /// Create a setup task that performs the necessary setup actions
        /// </summary>
        IWorkplanStep CreateStep(IProductionRecipe recipe);
    }
}
