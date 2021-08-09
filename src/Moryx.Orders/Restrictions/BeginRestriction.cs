// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

namespace Moryx.Orders.Restrictions
{
    /// <summary>
    /// Restriction to limit possibility to begin an operation
    /// </summary>
    public class BeginRestriction : IOperationRestriction
    {
        /// <summary>
        /// Creates a new instance of <see cref="BeginRestriction"/>
        /// </summary>
        public BeginRestriction(bool canBegin, RestrictionDescription description)
        {
            CanBegin = canBegin;
            Description = description;
        }

        /// <summary>
        /// Creates a new instance of <see cref="BeginRestriction"/>
        /// </summary>
        public BeginRestriction(bool canBegin, string text, RestrictionSeverity severity)
            : this(canBegin, new RestrictionDescription(text, severity))
        {
        }

        /// <summary>
        /// Indicator if a begin is possible
        /// </summary>
        public bool CanBegin { get; }

        /// <summary>
        /// Description of the Result why the rule was not complied
        /// </summary>
        public RestrictionDescription Description { get; }
    }
}