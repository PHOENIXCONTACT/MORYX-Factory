// Copyright (c) 2023, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Resources;

namespace Moryx.Factory
{
    /// <summary>
    /// A resource/machine location inside the factory
    /// </summary>
    public interface IMachineLocation : ILocation, IPublicResource
    {
        /// <summary>
        /// Resource/Machine at this location
        /// </summary>
        IPublicResource Machine { get; }

        /// <summary>
        /// Icon for the machine at this location
        /// </summary>
        string SpecificIcon { get; set; }
    }
}