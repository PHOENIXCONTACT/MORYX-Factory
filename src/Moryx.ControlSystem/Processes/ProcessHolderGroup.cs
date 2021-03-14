// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using Moryx.AbstractionLayer.Resources;

namespace Moryx.ControlSystem.Processes
{
    /// <summary>
    /// Default implementation for <see cref="IProcessHolderGroup{IProcessHolderPosition}"/>
    /// </summary>
    public class ProcessHolderGroup : Resource, IProcessHolderGroup<ProcessHolderPosition>
    {
        /// <summary>
        /// All positions of this carrier
        /// </summary>
        [ReferenceOverride(nameof(Children))]
        public IReferences<ProcessHolderPosition> Positions { get; set; }

        /// <inheritdoc />
        IEnumerable<ProcessHolderPosition> IProcessHolderGroup<ProcessHolderPosition>.Positions => Positions;

        /// <inheritdoc />
        public void Reset()
        {
            foreach (var position in Positions)
            {
                position.Reset();
            }
        }
    }
}
