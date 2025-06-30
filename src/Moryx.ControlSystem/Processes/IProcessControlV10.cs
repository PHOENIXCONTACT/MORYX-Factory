// Copyright (c) 2025, Phoenix Contact GmbH & Co. KG

using Moryx.AbstractionLayer;

namespace Moryx.ControlSystem.Processes
{
    /// <summary>
    /// Actions to report a running process
    /// </summary>
    public enum ReportAction
    {
        /// <summary>
        /// The process is broken or damaged
        /// </summary>
        Broken,
        /// <summary>
        /// The process was physically removed
        /// </summary>
        Removed
    }

    /// <summary>
    /// Facade interface to get more information from the control system.
    /// </summary>
    public interface IProcessControlV10 : IProcessControl
    {

        /// <summary>
        /// Reports a process as broken or removed
        /// </summary>
        /// <param name="process">The process to report</param>
        /// <param name="action">The action to perform</param>
        void Report(IProcess process, ReportAction action);

        /// <summary>
        /// Get the running process with the given <paramref name="processId"/>
        /// </summary>
        /// <param name="processId">Id of the running process</param>
        /// <returns><see cref="IProcess"/></returns>
        IProcess GetProcess(long processId);
    }
}
