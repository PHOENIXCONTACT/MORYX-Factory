﻿using Moryx.AbstractionLayer;
using Moryx.AbstractionLayer.Capabilities;
using Moryx.AbstractionLayer.Identity;
using Moryx.ControlSystem.Activities;

namespace Moryx.ControlSystem.Tests.Mocks
{
    [ActivityResults(typeof(DummyResult))]
    public class DummyActivity : Activity<DummyActivityParameters>, IInstanceModificationActivity
    {
        public IIdentity InstanceIdentity { get; set; }

        public InstanceModificationType ModificationType { get; set; }

        protected override ActivityResult CreateResult(long resultNumber)
        {
            return ActivityResult.Create((DummyResult)resultNumber);
        }

        protected override ActivityResult CreateFailureResult()
        {
            return ActivityResult.Create(DummyResult.TechnicalFailure);
        }

        public override ICapabilities RequiredCapabilities => new DummyCapabilities();

        public override ProcessRequirement ProcessRequirement => ProcessRequirement.Required;
    }
}