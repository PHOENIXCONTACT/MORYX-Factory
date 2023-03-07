using Moryx.AbstractionLayer.Resources;

namespace Moryx.Factory
{
    /// <summary>
    /// Group of resources inside the factory
    /// </summary>
    public interface IMachineGroup : IPublicResource
    {
        /// <summary>
        /// Default icon for this resource group
        /// </summary>
        string DefaultIcon { get; set; }
    }
}
