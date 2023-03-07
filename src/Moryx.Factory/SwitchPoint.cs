using System.Collections.Generic;
using System.Runtime.Serialization;
using Moryx.AbstractionLayer.Resources;
using Moryx.Serialization;

namespace Moryx.Factory
{
    /// <summary>
    /// Point where the direction changes in a transport path.
    /// </summary>
    public class SwitchPoint : Resource, ILocation
    {

        [DataMember, EntrySerialize]
        public int PositionX { get; set; }

        [DataMember, EntrySerialize]
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

        public IEnumerable<ITransportPath> Origins { get; set; }

        public IEnumerable<ITransportPath> Destinations { get; set; }

        public string Image { get; set; }
    }
}
