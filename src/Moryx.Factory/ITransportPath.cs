﻿using System.Collections.Generic;
using Moryx.AbstractionLayer.Resources;

namespace Moryx.Factory
{
    /// <summary>
    /// A transport path inside a factory
    /// </summary>
    public interface ITransportPath : IResource
    {
        /// <summary>
        /// Origin of the transport path
        /// </summary>
        ILocation Origin { get; }

        /// <summary>
        /// Destination of the transport path
        /// </summary>
        ILocation Destination { get; }

        /// <summary>
        /// Trajectory to follow from origin to reach the destination
        /// </summary>
        List<Position> WayPoints { get; set; }
    }
}