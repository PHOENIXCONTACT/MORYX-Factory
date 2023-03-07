using System.Runtime.Serialization;

namespace Moryx.Factory
{
    /// <summary>
    /// Position of a resource/machine
    /// </summary>
    [DataContract]
    public class Position
    {
        [DataMember]
        public int PositionX { get; set; }

        [DataMember]
        public int PositionY { get; set; }
    }
}