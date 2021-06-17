// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using Moryx.ControlSystem.Processes;

namespace Moryx.ControlSystem.Routing
{
    /// <summary>
    /// Flags of supported routing modes
    /// </summary>
    [Flags]
    public enum RoutingMode
    {
        /// <summary>
        /// No supported routing
        /// </summary>
        None,

        /// <summary>
        /// Routing per <see cref="IProcessHolderGroup"/>
        /// </summary>
        GroupRouting = 0x1,

        /// <summary>
        /// Routing per <see cref="IProcessHolderPosition"/>
        /// </summary>
        PositionRouting = 0x2
    }
}
