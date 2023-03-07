using Moryx.AbstractionLayer.Resources;
using Moryx.Serialization;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Moryx.Factory
{
    /// <summary>
    /// Class for all machine groups in manufacturing factory
    /// </summary>
    public class MachineGroup : PublicResource, IMachineGroup
    {
        [DataMember, EntrySerialize, DefaultValue("settings")]
        public string DefaultIcon { get; set; }
    }
}
