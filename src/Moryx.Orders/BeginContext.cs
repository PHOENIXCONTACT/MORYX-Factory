// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Orders.Restrictions;

namespace Moryx.Orders
{
    /// <summary>
    /// Context information to begin an operation production
    /// </summary>
    public class BeginContext : OperationInfo
    {
        /// <summary>
        /// Residual amount of the operation
        /// </summary>
        public int ResidualAmount { get; set; }

        /// <summary>
        /// Current target amount of the operation
        /// </summary>
        public int PartialAmount { get; set; }

        /// <summary>
        /// Indicator if a begin is possible
        /// </summary>
        public bool CanBegin { get; set; }

        /// <summary>
        /// Reasons why a begin is not possible
        /// </summary>
        public RestrictionDescription[] Restrictions { get; set; }
    }
}