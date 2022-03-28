// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using Moryx.AbstractionLayer;
using Moryx.AbstractionLayer.Identity;
using Moryx.ControlSystem.Activities;

namespace Moryx.ControlSystem.Cells
{
    /// <summary>
    /// Base class for all steps in the production process
    /// </summary>
    public abstract class Session
    {
        /// <summary>
        /// Empty array of constraints
        /// </summary>
        protected static readonly IConstraint[] EmptyConstraints = new IConstraint[0];

        /// <summary>
        /// Initialize a new resource request for a certain resource
        /// </summary>
        protected Session(ActivityClassification classification, ProcessReference reference, object tag)
        {
            _context = new SessionContext(classification, Guid.NewGuid(), reference, tag);
        }

        /// <summary>
        /// Internal constructor to forward the production context
        /// </summary>
        protected Session(Session currentSession)
        {
            _context = currentSession._context;
        }

        /// <summary>
        /// Context class holding all session information
        /// </summary>
        private readonly SessionContext _context;

        /// <summary>
        /// Unique id of the current production transaction
        /// </summary>
        public Guid Id => _context.SessionId;

        /// <summary>
        /// The resource accepted activity classification
        /// </summary>
        public ActivityClassification AcceptedClassification => _context.Classification;

        /// <summary>
        /// Id of the process the cell is working on
        /// </summary>
        public IProcess Process
        {
            get => _context.Process;
            internal set => _context.Process = value;
        }

        /// <summary>
        /// Id of the process the cell is working on
        /// </summary>
        public ProcessReference Reference
        {
            get => _context.Reference;
            internal set => _context.Reference = value;
        }

        /// <summary>
        /// User object to identify a session or to carry information until a session response.
        /// </summary>
        public object Tag
        {
            get => _context.Tag;
            set => _context.Tag = value;
        }

        #region Factory methods
        /// <summary>
        /// Start a new production session
        /// </summary>
        public static ReadyToWork StartSession(ActivityClassification classification, ReadyToWorkType type)
        {
            return CreateSession(classification, type, ProcessReference.Empty, null, EmptyConstraints);
        }

        /// <summary>
        /// Start a new production session
        /// </summary>
        public static ReadyToWork StartSession(ActivityClassification classification, ReadyToWorkType type, object tag)
        {
            return CreateSession(classification, type, ProcessReference.Empty, tag, EmptyConstraints);
        }

        /// <summary>
        /// Start a new production session
        /// </summary>
        public static ReadyToWork StartSession(ActivityClassification classification, ReadyToWorkType type, long processId)
        {
            return CreateSession(classification, type, ProcessReference.ProcessId(processId), null, EmptyConstraints);
        }
        /// <summary>
        /// Start a new production session
        /// </summary>
        public static ReadyToWork StartSession(ActivityClassification classification, ReadyToWorkType type, long processId, object tag)
        {
            return CreateSession(classification, type, ProcessReference.ProcessId(processId), tag, EmptyConstraints);
        }

        /// <summary>
        /// Start a new production session
        /// </summary>
        public static ReadyToWork StartSession(ActivityClassification classification, ReadyToWorkType type, IIdentity identity)
        {
            return CreateSession(classification, type, ProcessReference.InstanceIdentity(identity), null, EmptyConstraints);
        }

        /// <summary>
        /// Start a new production session
        /// </summary>
        public static ReadyToWork StartSession(ActivityClassification classification, ReadyToWorkType type, IIdentity identity, object tag)
        {
            return CreateSession(classification, type, ProcessReference.InstanceIdentity(identity), tag, EmptyConstraints);
        }

        /// <summary>
        /// Start a new production session
        /// </summary>
        public static ReadyToWork StartSession(ActivityClassification classification, ReadyToWorkType type, params IConstraint[] constraints)
        {
            return CreateSession(classification, type, ProcessReference.Empty, null, constraints);
        }

        /// <summary>
        /// Start a new production session
        /// </summary>
        public static ReadyToWork StartSession(ActivityClassification classification, ReadyToWorkType type, object tag, params IConstraint[] constraints)
        {
            return CreateSession(classification, type, ProcessReference.Empty, tag, constraints);
        }

        /// <summary>
        /// Start a new production session
        /// </summary>
        public static ReadyToWork StartSession(ActivityClassification classification, ReadyToWorkType type, long processId, params IConstraint[] constraints)
        {
            return CreateSession(classification, type, ProcessReference.ProcessId(processId), null, constraints);
        }

        /// <summary>
        /// Start a new production session
        /// </summary>
        public static ReadyToWork StartSession(ActivityClassification classification, ReadyToWorkType type, long processId, object tag, params IConstraint[] constraints)
        {
            return CreateSession(classification, type, ProcessReference.ProcessId(processId), tag, constraints);
        }

        /// <summary>
        /// Start a new production session
        /// </summary>
        public static ReadyToWork StartSession(ActivityClassification classification, ReadyToWorkType type, IIdentity identity, params IConstraint[] constraints)
        {
            return CreateSession(classification, type, ProcessReference.InstanceIdentity(identity), null, constraints);
        }

        /// <summary>
        /// Start a new production session
        /// </summary>
        public static ReadyToWork StartSession(ActivityClassification classification, ReadyToWorkType type, IIdentity identity, object tag,  params IConstraint[] constraints)
        {
            return CreateSession(classification, type, ProcessReference.InstanceIdentity(identity), tag, constraints);
        }

        private static ReadyToWork CreateSession(ActivityClassification classification, ReadyToWorkType type, ProcessReference reference, object tag, IConstraint[] constraints)
        {
            if (constraints == null)
                throw new ArgumentNullException(nameof(constraints));
            return new ReadyToWork(classification, type, reference, tag, constraints);
        }

        #endregion

        private class SessionContext
        {
            internal SessionContext(ActivityClassification classification, Guid sessionId, ProcessReference reference, object tag)
            {
                Classification = classification;
                SessionId = sessionId;
                Reference = reference;
                Tag = tag;
            }

            public Guid SessionId { get; }

            public IProcess Process { get; set; }

            public ProcessReference Reference { get; set; }

            public ActivityClassification Classification { get; }

            public object Tag { get; set; }
        }
    }
}
