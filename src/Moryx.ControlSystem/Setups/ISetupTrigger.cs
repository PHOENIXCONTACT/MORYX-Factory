// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Capabilities;
using Moryx.AbstractionLayer.Recipes;
using Moryx.Collections;
using Moryx.Modules;
using Moryx.Workplans;
using System;
using System.Collections.Generic;

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
        SetupEvaluation Evaluate(IProductRecipe recipe);

        /// <summary>
        /// Create a setup task that performs the necessary setup actions
        /// </summary>
        [Obsolete("This method will be replaced by CreateSteps from IMultiSetupTrigger in the next version")]
        IWorkplanStep CreateStep(IProductRecipe recipe);
    }

    /// <summary>
    /// Extended interface for more flexbile setup triggers
    /// </summary>
    public interface IMultiSetupTrigger : ISetupTrigger
    {
        /// <summary>
        /// Create a list of setup tasks that performs the necessary setup actions
        /// </summary>
        IReadOnlyList<IWorkplanStep> CreateSteps(IProductRecipe recipe);
    }
}
