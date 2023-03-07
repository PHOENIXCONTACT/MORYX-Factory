using System.ComponentModel;
using System.Runtime.Serialization;
using Moryx.AbstractionLayer.Resources;

using Moryx.Serialization;

namespace Moryx.Factory
{
    public class ManufacturingFactory : PublicResource, IManufacturingFactory
    {
        [DataMember, EntrySerialize, DefaultValue("assets/Fabrik_Hintergrund.png"), Description("URL of the background picture of the Factory Monitor")]
        public string BackgroundUrl { get; set; }
    }
}
