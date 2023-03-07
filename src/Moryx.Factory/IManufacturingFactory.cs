using Moryx.AbstractionLayer.Resources;

namespace Moryx.Factory
{
    /// <summary>
    /// A manufacturing factory interface
    /// </summary>
    public interface IManufacturingFactory : IPublicResource
    {
        /// <summary>
        /// Background URL of the factory monitor
        /// </summary>
        string BackgroundUrl { get; set; }
    }
}
