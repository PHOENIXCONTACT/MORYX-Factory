// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using Moryx.Users;

namespace Moryx.Orders
{
    /// <summary>
    /// Order management facade. Exports the important events to other modules
    /// </summary>
    public interface IOrderManagement
    {
        /// <summary>
        /// Returns all current available operations by the given filter
        /// </summary>
        IReadOnlyList<Operation> GetOperations(Func<Operation, bool> filter);

        /// <summary>
        /// Will return the operation with the given identifier
        /// </summary>
        Operation GetOperation(Guid identifier);

        /// <summary>
        /// Will return the operation with the given order and operation numbers
        /// </summary>
        Operation GetOperation(string orderNumber, string operationNumber);

        /// <summary>
        /// Will add a new operation to the pool.
        /// </summary>
        Operation AddOperation(OperationCreationContext context);

        /// <summary>
        /// Will add a new operation to the pool.
        /// </summary>
        Operation AddOperation(OperationCreationContext context, IOperationSource source);

        /// <summary>
        /// Returns a report context of the given operation
        /// </summary>
        BeginContext GetBeginContext(Operation operation);

        /// <summary>
        /// Begins the given operation
        /// </summary>
        void BeginOperation(Operation operation, int amount);

        /// <summary>
        /// Begins the given operation
        /// </summary>
        void BeginOperation(Operation operation, int amount, User user);

        /// <summary>
        /// Aborts the given operation if it was not started before
        /// </summary>
        /// <param name="operation"></param>
        void AbortOperation(Operation operation);

        /// <summary>
        /// Sets the sort order of the given operation
        /// </summary>
        void SetOperationSortOrder(int sortOrder, Operation operation);

        /// <summary>
        /// Returns a report context of the given operation
        /// </summary>
        ReportContext GetReportContext(Operation operation);

        /// <summary>
        /// Processes a report for the given operation
        /// </summary>
        void ReportOperation(Operation operation, OperationReport report);

        /// <summary>
        /// Returns a report context of the given operation to interrupt the operation
        /// </summary>
        ReportContext GetInterruptContext(Operation operation);

        /// <summary>
        /// Processes a interrupt for the given operation
        /// </summary>
        void InterruptOperation(Operation operation, OperationReport report);

        /// <summary>
        /// Updates the operation source
        /// </summary>
        void UpdateSource(IOperationSource source, Operation operation);

        /// <summary>
        /// Will be raised if the progress of an operation was changed
        /// </summary>
        event EventHandler<OperationChangedEventArgs> OperationProgressChanged;

        /// <summary>
        /// Will be raised if an operation was started
        /// </summary>
        event EventHandler<OperationStartedEventArgs> OperationStarted;

        /// <summary>
        /// Will be raised if an operation was closed
        /// </summary>
        event EventHandler<OperationReportEventArgs> OperationCompleted;

        /// <summary>
        /// Will be raised if an operation was interrupted
        /// </summary>
        event EventHandler<OperationReportEventArgs> OperationInterrupted;

        /// <summary>
        /// Will be raised if an operation was partially reported
        /// </summary>
        event EventHandler<OperationReportEventArgs> OperationPartialReport;

        /// <summary>
        /// Will be raised if an operation was adviced
        /// </summary>
        event EventHandler<OperationAdviceEventArgs> OperationAdviced;

        /// <summary>
        /// Will be raised if an operation was changed
        /// </summary>
        event EventHandler<OperationChangedEventArgs> OperationUpdated;

        /// <summary>
        /// Event which will be raised when the begin context of the operation will be requested
        /// </summary>
        event EventHandler<OperationBeginRequestEventArgs> OperationBeginRequest;

        /// <summary>
        /// Event which will be raised when the report context of the operation will be requested
        /// </summary>
        event EventHandler<OperationReportRequestEventArgs> OperationReportRequest;
    }
}
