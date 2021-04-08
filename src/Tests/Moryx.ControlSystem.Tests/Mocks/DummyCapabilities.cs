using Moryx.AbstractionLayer.Capabilities;

namespace Moryx.ControlSystem.Tests.Mocks
{
    public class DummyCapabilities : CapabilitiesBase
    {
        protected override bool ProvidedBy(ICapabilities provided)
        {
            return provided is DummyCapabilities;
        }
    }
}