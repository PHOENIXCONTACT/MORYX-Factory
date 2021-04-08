using Moryx.AbstractionLayer;
using Moryx.ControlSystem.Activities;

namespace Moryx.ControlSystem.Tests.Mocks
{
    public class DummyActivityParameters : Parameters, IActivityTimeoutParameters
    {
        public int Timeout { get; set; }

        protected override void Populate(IProcess process, Parameters instance)
        {
        }
    }
}