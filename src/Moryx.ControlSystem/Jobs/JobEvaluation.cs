// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.Diagnostics;
using Moryx.ControlSystem.Setups;

namespace Moryx.ControlSystem.Jobs
{
    /// <summary>
    /// Estimation by the <see cref="IJobManagement"/> how much additional effort
    /// is required for a certain job
    /// </summary>
    public class JobEvaluation
    {
        /// <summary>
        /// Preparation and clean-up efforts required for this Job
        /// </summary>
        public IReadOnlyList<SetupStep> RequiredSetup { get; set; }

        /// <summary>
        /// Possible errors in the workplan
        /// </summary>
        public IReadOnlyList<string> WorkplanErrors { get; set; }
    }

    /// <summary>
    /// Necessary setup step
    /// </summary>
    [DebuggerDisplay("{" + nameof(Name) + "}[{" + nameof(Classification) + "}]")]
    public struct SetupStep
    {
        /// <summary>
        /// Name of the step
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Classification of the setup
        /// </summary>
        public readonly SetupClassification Classification;

        /// <summary>
        /// Constructor for the <see cref="SetupStep"/>
        /// </summary>
        public SetupStep(string name, SetupClassification classification)
        {
            Name = name;
            Classification = classification;
        }
    }
}
