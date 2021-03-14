// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using Moryx.AbstractionLayer.Resources;

namespace Moryx.ControlSystem.Processes
{
    /// <summary>
    /// Generic interface for groups of types derived from <see cref="IProcessHolderPosition"/>
    /// </summary>
    /// <typeparam name="TPosition"></typeparam>
    public interface IProcessHolderGroup<out TPosition> : IResource
        where TPosition : IProcessHolderPosition
    {
        /// <summary>
        /// All positions of the group
        /// </summary>
        IEnumerable<TPosition> Positions { get; }

        /// <summary>
        /// The entire group is reset
        /// </summary>
        void Reset();
    }
}
