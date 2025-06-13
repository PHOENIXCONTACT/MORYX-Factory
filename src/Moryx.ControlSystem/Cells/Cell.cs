// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moryx.AbstractionLayer;
using Moryx.AbstractionLayer.Capabilities;
using Moryx.AbstractionLayer.Resources;
using Moryx.ControlSystem.Activities;
using static System.Collections.Specialized.BitVector32;

namespace Moryx.ControlSystem.Cells
{
    /// <summary>
    /// Base type for all implementations of <see cref="ICell"/>
    /// </summary>
    [Description("Base type for all cells within a production system")]
    public abstract class Cell : Resource, ICell
    {
        private readonly ConcurrentDictionary<Session, TaskCompletionSource<Session>> _sessionCompletionSources =
            new ConcurrentDictionary<Session, TaskCompletionSource<Session>>(new SessionComparer());

        /// <summary>
        /// CancellationTokenSource that must be canceled during <see cref="OnStop"/>.
        /// Used to cancel async operations during resource shutdown.
        /// </summary>
        protected readonly CancellationTokenSource LifeCycleTokenSource = new CancellationTokenSource();


        /// <inheritdoc />
        public abstract IEnumerable<Session> ControlSystemAttached();

        /// <inheritdoc />
        public abstract IEnumerable<Session> ControlSystemDetached();

        /// <summary>
        /// Callback to start an activity on the cell after a <see cref="ReadyToWork"/> event was raised.
        /// If not otherwise explicitly required all depending components will use the interface to refer to cells,
        /// making this the right place to interject and complete tasks
        /// started by async calls to <see cref="PublishReadyToWorkAsync(Moryx.ControlSystem.Cells.ReadyToWork)"/>.
        /// </summary>
        void ICell.StartActivity(ActivityStart activityStart)
        {
            // check if session was started async
            if (_sessionCompletionSources.TryRemove(activityStart, out var completionSource))
            {
                // by setting the result PublishReadyToWorkAsync will be completed.
                if (!completionSource.TrySetResult(activityStart))
                {
                    Logger.Log(LogLevel.Error,"Cannot set result of async request with session {sessionId}. [{sessionType}]", activityStart.Id,
                            nameof(ActivityStart));
                }
                // session was started async: do NOT forward activity start to StartActivity()!
                return; 
            }

            StartActivity(activityStart);
        }

        /// <summary>
        /// Callback to start an activity on the cell after a <see cref="ReadyToWork"/> event was raised.
        /// </summary>
        /// <param name="activityStart"></param>
        public abstract void StartActivity(ActivityStart activityStart);

        /// <summary>
        /// Callback to abort a running activity on the cell after a <see cref="StartActivity(Moryx.ControlSystem.Cells.ActivityStart)"/> was received.
        /// Aborting might occur due to a abort of the related Job.
        /// If not otherwise explicitly required all depending components will use the interface to refer to cells,
        /// making this the right place to interject suppress ProcessAborting for pending async result calls started with <see cref="PublishActivityCompletedAsync(Moryx.ControlSystem.Cells.ActivityCompleted)"/> .
        /// </summary>
        void ICell.ProcessAborting(IActivity affectedActivity)
        {
            var asyncResult = _sessionCompletionSources.SingleOrDefault(pair =>
                pair.Key is ActivityCompleted completed &&
                completed.CompletedActivity.Id == affectedActivity.Id);
            if (asyncResult.Key != null)
            {
                Logger.Log(LogLevel.Information, "ProcessAborting of activity {activityId} [{activityType}] was suppressed due to a pending async activity result. Session {sessionId}!", affectedActivity.Id, affectedActivity.GetType().Name, asyncResult.Key.Id);
                return;
            }
            ProcessAborting(affectedActivity);
        }

        /// <summary>
        /// Callback to abort a running activity on the cell after a <see cref="StartActivity(Moryx.ControlSystem.Cells.ActivityStart)"/> was received.
        /// Aborting might occur due to a abort of the related Job.
        /// </summary>
        public virtual void ProcessAborting(IActivity affectedActivity) { }

        /// <summary>
        /// Callback to complete a sequence on the cell after a <see cref="ReadyToWork"/> or <see cref="ActivityCompleted"/> event was raised.
        /// If not otherwise explicitly required all depending components will use the interface to refer to cells,
        /// making this the right place to interject and complete tasks started by async calls
        /// to <see cref="PublishReadyToWorkAsync(Moryx.ControlSystem.Cells.ReadyToWork)"/> or <see cref="PublishActivityCompletedAsync(Moryx.ControlSystem.Cells.ActivityCompleted)"/> .
        /// </summary>
        void ICell.SequenceCompleted(SequenceCompleted completed)
        {
            // check if session was started async
            if (_sessionCompletionSources.TryRemove(completed, out var completionSource))
            {
                // by setting the result the related async call (PublishReadyToWorkAsync or PublishActivityCompletedAsync) will be completed.
                if (!completionSource.TrySetResult(completed))
                {
                    Logger.Log(LogLevel.Error, "Cannot set result of async request for session {sessionId}. [{sessionType}]", completed.Id,
                            nameof(SequenceCompleted));
                }
                // session was started async: do NOT forward SequenceCompleted to SequenceCompleted()!
                return;
            }

            SequenceCompleted(completed);
        }

        /// <summary>
        /// Callback to complete a sequence on the cell after a <see cref="ReadyToWork"/> or <see cref="ActivityCompleted"/> event was raised.
        /// </summary>
        public abstract void SequenceCompleted(SequenceCompleted completed);

        /// <inheritdoc />
        protected override void OnStop()
        {
            LifeCycleTokenSource.Cancel();
            LifeCycleTokenSource.Dispose();
            base.OnStop();
        }

        /// <summary>
        /// Publish a <see cref="ReadyToWork"/> from the resource.
        /// Returns the <see cref="ActivityStart"/> or <see cref="Moryx.ControlSystem.Cells.SequenceCompleted"/> when returned.
        /// </summary>
        /// <exception cref="OperationCanceledException">
        /// Operation might be canceled due to Lifecycle-CancellationToken,
        /// Personal-CancellationToken, request was not able to execute or
        /// <see cref="PublishNotReadyToWork"/> was called for the related session.
        /// </exception>
        public Task<Session> PublishReadyToWorkAsync(ReadyToWork readyToWork)
        {
            return PublishReadyToWorkAsync(readyToWork, CancellationToken.None);
        }

        /// <summary>
        /// Publish a <see cref="ReadyToWork"/> from the resource.
        /// Returns the <see cref="ActivityStart"/> or <see cref="Moryx.ControlSystem.Cells.SequenceCompleted"/> when returned.
        /// </summary>
        /// <exception cref="OperationCanceledException">
        /// Operation might be canceled due to Lifecycle-CancellationToken,
        /// Personal-CancellationToken, request was not able to execute or
        /// <see cref="PublishNotReadyToWork"/> was called for the related session.
        /// </exception>
        public Task<Session> PublishReadyToWorkAsync(ReadyToWork readyToWork, CancellationToken cancellationToken)
        {
            Logger.Log(LogLevel.Trace, "PublishReadyToWorkAsync Session {sessionId} Type {rtwType}, Classification {classification}, {reference}", readyToWork.Id,
                    readyToWork.ReadyToWorkType, readyToWork.AcceptedClassification, readyToWork.Reference);
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(LifeCycleTokenSource.Token, cancellationToken);
            var linkedToken = linkedTokenSource.Token;

            var completionSource = new TaskCompletionSource<Session>();
            linkedToken.Register(() => completionSource.TrySetCanceled());
            // throw exception if cancellation via token was requested
            linkedToken.ThrowIfCancellationRequested();

            // check event to be wired
            if (ReadyToWork == null)
            {
                Logger.Log(LogLevel.Error, "PublishReadyToWorkAsync for session {sessionId} canceled! ReadyToWork-Event not wired. Make sure to await ControlSystemAttached before starting any sessions!", readyToWork.Id);
                completionSource.TrySetCanceled();
                return completionSource.Task;
            }

            if (!_sessionCompletionSources.TryAdd(readyToWork, completionSource))
            {
                Logger.Log(LogLevel.Error, "There is already a running async operation for Session {sessionId}! Cancel the current request!", readyToWork.Id);
                completionSource.TrySetCanceled();
                return completionSource.Task;
            }

            ReadyToWork.Invoke(this, readyToWork);

            try
            {
                // now waiting for StartActivity() or SequenceCompleted()
                completionSource.Task.GetAwaiter().GetResult();
            }
            catch (TaskCanceledException)
            {
                Logger.Log(LogLevel.Information, "PublishReadyToWorkAsync canceled! Session {sessionId} Publish NotReadyToWork", readyToWork.Id);
                // NotReadyToWork must be wired because we raised ReadyToWork before!
                NotReadyToWork!.Invoke(this,readyToWork.PauseSession());
            }

            return completionSource.Task;
        }

        /// <summary>
        /// Publish a <see cref="ReadyToWork"/> from the resource
        /// </summary>
        public void PublishReadyToWork(ReadyToWork readyToWork)
        {
            ReadyToWork?.Invoke(this, readyToWork);
        }

        /// <inheritdoc />
        public event EventHandler<ReadyToWork> ReadyToWork;

        /// <summary>
        /// Publish a <see cref="NotReadyToWork"/> from the resource.
        /// If the session was started within <see cref="PublishReadyToWorkAsync(Moryx.ControlSystem.Cells.ReadyToWork)"/> that async call is canceled
        /// </summary>
        public void PublishNotReadyToWork(NotReadyToWork notReadyToWork)
        {
            if (_sessionCompletionSources.TryRemove(notReadyToWork, out var completionSource))
            {
                completionSource.TrySetCanceled();
                // cancellation of the completionSource will send a NotReadyToWork 
                return;
            }
            NotReadyToWork?.Invoke(this, notReadyToWork);
        }

        /// <inheritdoc />
        public event EventHandler<NotReadyToWork> NotReadyToWork;

        /// <summary>
        /// Publish <see cref="ActivityCompleted"/> from the resource
        /// Returns the <see cref="Moryx.ControlSystem.Cells.SequenceCompleted"/> when returned.
        /// </summary>
        /// <exception cref="OperationCanceledException">
        /// Operation might be canceled due to Lifecycle-CancellationToken,
        /// Personal-CancellationToken or request was not able to execute.
        /// </exception>
        public Task<Session> PublishActivityCompletedAsync(ActivityCompleted activityResult)
        {
            return PublishActivityCompletedAsync(activityResult, CancellationToken.None);
        }

        /// <summary>
        /// Publish <see cref="ActivityCompleted"/> from the resource
        /// Returns the <see cref="Moryx.ControlSystem.Cells.SequenceCompleted"/> when returned.
        /// </summary>
        /// <exception cref="OperationCanceledException">
        /// Operation might be canceled due to Lifecycle-CancellationToken,
        /// Personal-CancellationToken or request was not able to execute.
        /// </exception>
        public Task<Session> PublishActivityCompletedAsync(ActivityCompleted activityResult, CancellationToken cancellationToken)
        {
            Logger.Log(LogLevel.Trace,"PublishActivityCompletedAsync Session {sessionId}, Classification {classification}, {reference}", activityResult.Id,
                    activityResult.AcceptedClassification, activityResult.Reference);
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(LifeCycleTokenSource.Token, cancellationToken);
            var linkedToken = linkedTokenSource.Token;

            var completionSource = new TaskCompletionSource<Session>();
            linkedToken.Register(() => completionSource.TrySetCanceled());
            // throw exception if cancellation via token was requested
            linkedToken.ThrowIfCancellationRequested();

            if (_sessionCompletionSources.TryAdd(activityResult, completionSource))
            {
                Logger.Log(LogLevel.Warning, "There is already a running async operation for Session {sessionId}! Cancel current request!", activityResult.Id);
                completionSource.TrySetCanceled();
                return completionSource.Task;
            }
            // ActivityCompleted must be wired because we received the ActivityStart before!
            ActivityCompleted!.Invoke(this, activityResult);
            return completionSource.Task;
        }

        /// <summary>
        /// Publish <see cref="ActivityCompleted"/> from the resource
        /// </summary>
        public void PublishActivityCompleted(ActivityCompleted activityResult)
        {
            ActivityCompleted?.Invoke(this, activityResult);
        }

        /// <inheritdoc />
        public event EventHandler<ActivityCompleted> ActivityCompleted;

        /// <summary>
        /// IEqualityComparer to compare different sessions in a dictionary by their Id.
        /// </summary>
        private class SessionComparer : IEqualityComparer<Session>
        {
            /// <inheritdoc />
            public bool Equals(Session x, Session y)
            {
                if (x == null || y == null)
                    return false;
                return x.Id.Equals(y.Id);
            }

            /// <inheritdoc />
            public int GetHashCode(Session obj)
            {
                return obj.Id.GetHashCode();
            }
        }
    }
}
