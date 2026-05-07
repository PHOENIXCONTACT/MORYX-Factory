// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using Moryx.AbstractionLayer;

namespace Moryx.ControlSystem.Activities
{
    /// <summary>
    /// Mounting operation performed when this activity is completed
    /// </summary>
    public enum MountOperation
    {
        /// <summary>
        /// Previous state remains
        /// </summary>
        Unchanged = 0,

        /// <summary>
        /// Mount process on carrier
        /// </summary>
        Mount = 1,

        /// <summary>
        /// Remove process from carrier
        /// </summary>
        Unmount = 2
    }

    /// <summary>
    /// Special interface to identify activities that perform mount operations
    /// </summary>
    public interface IMountingActivity : IActivity
    {
        /// <summary>
        /// Operation this activity performs
        /// </summary>
        [Obsolete("Use IMountingActivityExtended.ExecutedMountOperation")]
        MountOperation Operation { get; }
    }

    /// <summary>
    /// Special interface to identify activities that perform mount operations
    /// </summary>
    [Obsolete("Will be merged into IMountingActivity within the next Major version!")]
    public interface IMountingActivityExtended : IMountingActivity
    {
        /// <summary>
        /// MountOperation that is the intended target of the Activity.
        /// </summary>
        MountOperation IntendedMountOperation { get; }

        /// <summary>
        /// MountOperation that was reached within the result of the Activity.
        /// </summary>
        MountOperation ExecutedMountOperation { get; }
    }


}
