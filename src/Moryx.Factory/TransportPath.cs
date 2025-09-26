// Copyright (c) 2023, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Moryx.AbstractionLayer.Resources;
using Moryx.Serialization;
using Moryx.Factory.Localizations;

namespace Moryx.Factory
{
    /// <summary>
    /// Transport path in a factory
    /// </summary>
    [Display(Name = nameof(Strings.TRANSPORT_PATH), Description = nameof(Strings.TRANSPORT_PATH_DESCRIPTION), ResourceType = typeof(Localizations.Strings))]
    public class TransportPath : Resource, ITransportPath
    {
        [ResourceReference(ResourceRelationType.TransportRoute, ResourceReferenceRole.Source)]
        public ILocation Origin { get; set; }

        [ResourceReference(ResourceRelationType.TransportRoute, ResourceReferenceRole.Target)]
        public ILocation Destination { get; set; }

        [DataMember, EntrySerialize]
        [Display(Name = nameof(Strings.WAY_POINTS), ResourceType = typeof(Localizations.Strings))]
        public List<Position> WayPoints { get; set; } = new List<Position>();
    }
}
