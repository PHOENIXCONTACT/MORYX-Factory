// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Runtime.Serialization;
using Moryx.Modules;
using Moryx.Serialization;

namespace Moryx.ControlSystem.Setups
{
    /// <summary>
    /// Config for all implementations of <see cref="ISetupTrigger"/>
    /// </summary>
    [DataContract]
    public class SetupTriggerConfig : IPluginConfig
    {
        /// <summary>
        /// Name of the plugin to instantiate for this trigger
        /// </summary>
        [DataMember, PluginNameSelector(typeof(ISetupTrigger))]
        public virtual string PluginName { get; set; }

        /// <summary>
        /// Sort order of the trigger instance in the resulting workplan
        /// </summary>
        [DataMember]
        public int SortOrder { get; set; }
    }
}
