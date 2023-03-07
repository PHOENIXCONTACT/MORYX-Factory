using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Moryx.AbstractionLayer.Resources;
using Moryx.ControlSystem.Cells;
using Moryx.Serialization;

namespace Moryx.Factory
{
    /// <summary>
    /// Class for MachineLocation in the factory
    /// </summary>
    public class MachineLocation : PublicResource, IMachineLocation
    {
        public IPublicResource Machine => Children.OfType<ICell>().FirstOrDefault();

        [DataMember, EntrySerialize]
        public string SpecificIcon { get; set; }

        [DataMember, EntrySerialize]
        public string Image { get; set; }

        /// <summary>
        /// X position of the location
        /// </summary>
        [DataMember, EntrySerialize, DefaultValue(10)]
        public int PositionX { get; set; }

        /// <summary>
        /// Y position of the location
        /// </summary>
        [DataMember, EntrySerialize, DefaultValue(10)]
        public int PositionY { get; set; }

        public Position Position
        {
            get => new() { PositionX = PositionX, PositionY = PositionY };
            set
            {
                PositionX = value.PositionX;
                PositionY = value.PositionY;
            }
        }

        [ResourceReference(ResourceRelationType.TransportRoute, ResourceReferenceRole.Source)]
        public IReferences<ITransportPath> Origins { get; set; }

        [ResourceReference(ResourceRelationType.TransportRoute, ResourceReferenceRole.Target)]
        public IReferences<ITransportPath> Destinations { get; set; }

        IEnumerable<ITransportPath> ILocation.Origins => Origins;
        IEnumerable<ITransportPath> ILocation.Destinations => Destinations;
    }
}
