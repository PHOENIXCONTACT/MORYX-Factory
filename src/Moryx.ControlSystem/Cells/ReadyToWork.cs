// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer;
using Moryx.ControlSystem.Activities;

namespace Moryx.ControlSystem.Cells
{
    /// <summary>
    /// Different request types for ReadyToWork
    /// </summary>
    public enum ReadyToWorkType
    {
        /// <summary>
        /// Define a special default to avoid default=Push
        /// </summary>
        Unset,
        /// <summary>
        /// The push
        /// </summary>
        Push,
        /// <summary>
        /// The pull
        /// </summary>
        Pull
    }

    /// <summary>
    /// Message send by the resource when it
    /// </summary>
    public class ReadyToWork : Session, ICompletableSession
    {
        /// <summary>
        /// Signal ready to work
        /// </summary>
        internal ReadyToWork(ActivityClassification classification, ReadyToWorkType type, ProcessReference reference, IConstraint[] constraints)
            : base(classification, reference)
        {
            ReadyToWorkType = type;
            Constraints = constraints;
        }

        /// <summary>
        /// Constructor to resume session with new ready to work
        /// </summary>
        /// <param name="currentSession"></param>
        internal ReadyToWork(Session currentSession)
            : base(currentSession)
        {
            // Constraints are dropped on resume
            Constraints = EmptyConstraints;
            // We always resume a session as pull
            ReadyToWorkType = ReadyToWorkType.Pull;
        }

        /// <summary>
        /// Gets or sets the type of the ready to work.
        /// </summary>
        /// <value>
        /// The type of the ready to work.
        /// </value>
        public ReadyToWorkType ReadyToWorkType { get; }

        /// <summary>
        /// Checks if the context matches to the constrains given by the resource
        /// </summary>
        public IConstraint[] Constraints { get; }

        /// <summary>
        /// Creates the start activity message to send an activity to a resource.
        /// </summary>
        /// <param name="activity">The activity.</param>
        public ActivityStart StartActivity(IActivity activity)
        {
            // Update process before next stage
            Process = activity.Process;
            return new ActivityStart(this, activity);
        }

        /// <summary>
        /// Creates the SequenceCompleted message
        /// </summary>
        public SequenceCompleted CompleteSequence(IProcess process, bool processActive, params long[] nextCells)
        {
            // Update process before next stage
            Process = process;
            return new SequenceCompleted(this, processActive, nextCells);
        }

        /// <summary>
        /// Interrupt the currently running session
        /// </summary>
        public NotReadyToWork PauseSession()
        {
            return new NotReadyToWork(this);
        }
    }
}
