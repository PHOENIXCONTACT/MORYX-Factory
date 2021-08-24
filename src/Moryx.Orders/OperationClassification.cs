// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

namespace Moryx.Orders
{
    /// <summary>
    /// Classification of the internal state machine of the operation for an external representation
    /// </summary>
    public enum OperationClassification
    {
        /// <summary>
        /// Classification during the creation
        /// </summary>
        Initial,

        /// <summary>
        /// Created operation and ready to start the production
        /// </summary>
        Ready,

        /// <summary>
        /// There is currently a working progress like the production or a reporting
        /// </summary>
        Running,

        /// <summary>
        /// The operation was interrupted but the production is currently running for the last parts
        /// </summary>
        Interrupting,

        /// <summary>
        /// The operation reached the current amount or the user has interrupted the operation
        /// </summary>
        Interrupted,

        /// <summary>
        /// The operation was declared as finished and can not be started again
        /// </summary>
        Completed,

        /// <summary>
        /// This operation was declared as aborted and was never started.
        /// </summary>
        Aborted
    }
}
