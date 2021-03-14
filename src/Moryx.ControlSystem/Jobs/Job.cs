// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using Moryx.AbstractionLayer;
using Moryx.AbstractionLayer.Recipes;

namespace Moryx.ControlSystem.Jobs
{
    /// <summary>
    /// External representation of a Job
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Create job
        /// </summary>
        public Job(IRecipe recipe, int amount)
        {
            Recipe = recipe;
            Amount = amount;
        }

        /// <summary>
        /// This Job's ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the recipe.
        /// </summary>
        public IRecipe Recipe { get; protected set; }

        /// <summary>
        /// The number of items to produce
        /// </summary>
        public int Amount { get; protected set; }

        /// <summary>
        /// Number of successful processes
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// Number of failed processes
        /// </summary>
        public int FailureCount { get; set; }

        /// <summary>
        /// Number of reworked processes
        /// </summary>
        public int ReworkedCount { get; set; }

        /// <summary>
        /// Classification of the job
        /// </summary>
        public JobClassification Classification { get; set; }

        /// <summary>
        /// Currently running processes of the job
        /// </summary>
        public IReadOnlyList<IProcess> RunningProcesses { get; protected set; }

        /// <summary>
        /// All processes of the job including running and completed processes
        /// </summary>
        public IReadOnlyList<IProcess> AllProcesses { get; protected set; }
    }
}
