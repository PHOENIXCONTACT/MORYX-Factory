// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moryx.Logging;

namespace Moryx.Orders
{
    /// <summary>
    /// Logger for operation create steps
    /// </summary>
    public interface IOperationLogger
    {
        /// <summary>
        /// Logged messages while creation
        /// </summary>
        IReadOnlyCollection<OperationLogMessage> Messages { get; }

        /// <summary>
        /// <see cref="IModuleLogger.Log"/> will be extended by the order and operation number
        /// </summary>
        void Log(LogLevel logLevel, string message, params object[] parameters);

        /// <summary>
        /// <see cref="IModuleLogger.LogException"/> will be extended by the order and operation number
        /// </summary>
        void LogException(LogLevel logLevel, Exception exception, string message, params object[] parameters);
    }
}