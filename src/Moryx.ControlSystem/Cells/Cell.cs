// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moryx.AbstractionLayer;
using Moryx.AbstractionLayer.Capabilities;
using Moryx.AbstractionLayer.Resources;

namespace Moryx.ControlSystem.Cells
{
    /// <summary>
    /// Base type for all implementations of <see cref="ICell"/>
    /// </summary>
    [Description("Base type for all cells within a production system")]
    public abstract class Cell : Resource, ICell
    {
        private readonly Dictionary<Guid, TaskCompletionSource<Session>> _sessionCompletionSources =
            new Dictionary<Guid, TaskCompletionSource<Session>>();

        /// <summary>
        /// CancellationTokenSource that must be canceled during <see cref="OnStop"/>.
        /// Used to cancel async operations during resource shutdown.
        /// </summary>
        protected CancellationTokenSource LifeCycleTokenSource = new CancellationTokenSource();


        /// <inheritdoc />
        public abstract IEnumerable<Session> ControlSystemAttached();

        /// <inheritdoc />
        public abstract IEnumerable<Session> ControlSystemDetached();

        /// <summary>
        /// Callback of the control system, to start an activity in the cell.
        /// ProcessEngine will call the ICell interface.
        /// Requests started with the async api will be redirected to the async call <see cref="PublishReadyToWorkAsync(Moryx.ControlSystem.Cells.ReadyToWork)"/>.
        /// </summary>
        void ICell.StartActivity(ActivityStart activityStart)
        {
            // check if session was started async
            if (_sessionCompletionSources.TryGetValue(activityStart.Id, out var completionSource))
            {
                _sessionCompletionSources.Remove(activityStart.Id);
                if (!completionSource.TrySetResult(activityStart))
                    Logger.Log(LogLevel.Error, $"Cannot set result of async request. [{nameof(ActivityStart)}]");
                return;
            }

            StartActivity(activityStart);
        }

        /// <summary>
        /// Callback of the control system, to start an activity in the cell.
        /// </summary>
        /// <param name="activityStart"></param>
        public abstract void StartActivity(ActivityStart activityStart);

        /// <inheritdoc />
        public virtual void ProcessAborting(IActivity affectedActivity) { }
		
        /// <summary>
        /// Callback from the control system, that the sequence was completed.
        /// ProcessEngine will call the ICell interface.
        /// Requests started with the Async api will be redirected to the async call <see cref="PublishReadyToWorkAsync(Moryx.ControlSystem.Cells.ReadyToWork)"/>
        /// or <see cref="PublishActivityCompletedAsync(Moryx.ControlSystem.Cells.ActivityCompleted)"/>.
        /// </summary>
        void ICell.SequenceCompleted(SequenceCompleted completed)
        {
            if (_sessionCompletionSources.TryGetValue(completed.Id, out var completionSource))
            {
                _sessionCompletionSources.Remove(completed.Id);
                if (!completionSource.TrySetResult(completed))
                    Logger.Log(LogLevel.Error, $"Cannot set result of async request. [{nameof(SequenceCompleted)}]");
                return;
            }

            SequenceCompleted(completed);
        }

        /// <summary>
        /// Callback from the control system, that the sequence was completed.
        /// </summary>
        /// <param name="completed"></param>
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
        public Task<Session> PublishReadyToWorkAsync(ReadyToWork readyToWork)
        {
            return PublishReadyToWorkAsync(readyToWork, CancellationToken.None);
        }

        /// <summary>
        /// Publish a <see cref="ReadyToWork"/> from the resource.
        /// Returns the <see cref="ActivityStart"/> or <see cref="Moryx.ControlSystem.Cells.SequenceCompleted"/> when returned.
        /// </summary>
        public async Task<Session> PublishReadyToWorkAsync(ReadyToWork readyToWork, CancellationToken cancellationToken)
        {
            Logger.Log(LogLevel.Trace, $"PublishReadyToWorkAsync Session {readyToWork.Id} Type {readyToWork.ReadyToWorkType}, Classification {readyToWork.AcceptedClassification}, {readyToWork.Reference}");
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(LifeCycleTokenSource.Token, cancellationToken);
            var linkedToken = linkedTokenSource.Token;
            // throw exception if cancellation via token was requested
            linkedToken.ThrowIfCancellationRequested();
            // double check if any of the combined Tokens are already canceled.
            cancellationToken.ThrowIfCancellationRequested();
            LifeCycleTokenSource.Token.ThrowIfCancellationRequested();

            var completionSource = new TaskCompletionSource<Session>(linkedToken);
            _sessionCompletionSources.Add(readyToWork.Id, completionSource);
            ReadyToWork!.Invoke(this, readyToWork);
            var result = await completionSource.Task;
            if (completionSource.Task.IsCanceled)
            {
                Logger.Log(LogLevel.Information, $"PublishReadyToWorkAsync canceled! Session {readyToWork.Id} Publish NotReadyToWork");
                NotReadyToWork!.Invoke(this,readyToWork.PauseSession());
            }

            return result;
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
            if (_sessionCompletionSources.TryGetValue(notReadyToWork.Id, out var completionSource))
            {
                _sessionCompletionSources.Remove(notReadyToWork.Id);
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
        /// </summary>
        /// <param name="activityResult"></param>
        public Task<Session> PublishActivityCompletedAsync(ActivityCompleted activityResult)
        {
            return PublishActivityCompletedAsync(activityResult, CancellationToken.None);
        }

        /// <summary>
        /// Publish <see cref="ActivityCompleted"/> from the resource
        /// </summary>
        /// <param name="activityResult"></param>
        /// <param name="cancellationToken"></param>
        public Task<Session> PublishActivityCompletedAsync(ActivityCompleted activityResult, CancellationToken cancellationToken)
        {
            Logger.Log(LogLevel.Trace, $"PublishActivityCompletedAsync Session {activityResult.Id}, Classification {activityResult.AcceptedClassification}, {activityResult.Reference}");
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(LifeCycleTokenSource.Token, cancellationToken);
            var linkedToken = linkedTokenSource.Token;
            // throw exception if cancellation via token was requested
            linkedToken.ThrowIfCancellationRequested();
            // double check if any of the combined Tokens are already canceled.
            cancellationToken.ThrowIfCancellationRequested();
            LifeCycleTokenSource.Token.ThrowIfCancellationRequested();

            var completionSource = new TaskCompletionSource<Session>();
            _sessionCompletionSources.Add(activityResult.Id, completionSource);
            ActivityCompleted!.Invoke(this, activityResult);
            return completionSource.Task;
        }

        /// <summary>
        /// Publish <see cref="ActivityCompleted"/> from the resource
        /// </summary>
        /// <param name="activityResult"></param>
        public void PublishActivityCompleted(ActivityCompleted activityResult)
        {
            ActivityCompleted?.Invoke(this, activityResult);
        }

        /// <inheritdoc />
        public event EventHandler<ActivityCompleted> ActivityCompleted;
    }
}
