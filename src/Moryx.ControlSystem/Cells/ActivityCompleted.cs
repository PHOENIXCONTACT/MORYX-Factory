// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer;

namespace Moryx.ControlSystem.Cells
{
    /// <summary>
    /// Message send by the resource managment when it completed an activity
    /// </summary>
    public class ActivityCompleted : Session, ICompletableSession
    {
        /// <summary>
        /// Initialize a new resource request for a certain resource
        /// </summary>
        internal ActivityCompleted(IActivity completed, Session currentSession)
            : base(currentSession)
        {
            CompletedActivity = completed;
        }

        /// <summary>
        /// Initialize a new resource request for a certain resource
        /// </summary>
        internal ActivityCompleted(IActivity completed, Session currentSession, object tag)
            : base(currentSession)
        {
            CompletedActivity = completed;
            Tag = tag;
        }

        /// <summary>
        /// Activity that was completed
        /// </summary>
        public IActivity CompletedActivity { get; }

        /// <summary>
        /// Complete the current sequence to await new ready to work
        /// </summary>
        public SequenceCompleted CompleteSequence(IProcess process, bool processActive, params long[] nextCells)
        {
            // Ignore process as its still set
            return new SequenceCompleted(this, processActive, nextCells);
        }
    }
}
