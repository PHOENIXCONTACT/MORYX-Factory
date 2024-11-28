using System;
using System.Collections.Generic;
using System.Linq;
using Moryx.AbstractionLayer;
using Moryx.ControlSystem.Cells;

namespace Moryx.ControlSystem.Processes
{
    /// <summary>
    /// Extension methods to handle sessions on process holders
    /// </summary>
    public static class ProcessHolderExtensions
    {
        #region Get Position
        /// <summary>
        /// Get the position by its <see cref="IProcessHolderPosition.Identifier"/>
        /// </summary>
        public static TPosition GetPositionByIdentifier<TPosition>(this IProcessHolderGroup<TPosition> group, string identifier)
            where TPosition : IProcessHolderPosition => GetPosition(group.Positions, t => t.Identifier == identifier);

        /// <summary>
        /// Get the position by its <see cref="IProcessHolderPosition.Identifier"/>
        /// </summary>
        public static TPosition GetPositionByIdentifier<TPosition>(this IEnumerable<TPosition> positions, string identifier)
            where TPosition : IProcessHolderPosition => GetPosition(positions, t => t.Identifier == identifier);

        /// <summary>
        /// Get the position by its <see cref="IProcessHolderPosition.Session"/>
        /// </summary>
        public static TPosition GetPositionBySession<TPosition>(this IProcessHolderGroup<TPosition> group, Session session)
            where TPosition : IProcessHolderPosition => GetPosition(group.Positions, t => t.Session?.Id == session.Id);

        /// <summary>
        /// Get the position by its <see cref="IProcessHolderPosition.Session"/>
        /// </summary>
        public static TPosition GetPositionBySession<TPosition>(this IEnumerable<TPosition> positions, Session session)
            where TPosition : IProcessHolderPosition => GetPosition(positions, t => t.Session?.Id == session.Id);

        /// <summary>
        /// Get the position by its <see cref="IProcessHolderPosition.Process"/>
        /// </summary>
        public static TPosition GetPositionByProcessId<TPosition>(this IProcessHolderGroup<TPosition> group, long processId)
            where TPosition : IProcessHolderPosition => GetPosition(group.Positions, t => t.Process?.Id == processId);

        /// <summary>
        /// Get the position by its <see cref="IProcessHolderPosition.Process"/>
        /// </summary>
        public static TPosition GetPositionByProcessId<TPosition>(this IEnumerable<TPosition> positions, long processId)
            where TPosition : IProcessHolderPosition => GetPosition(positions, t => t.Process?.Id == processId);

        /// <summary>
        /// Get the position by id of the running activity
        /// </summary>
        public static TPosition GetPositionByActivityId<TPosition>(this IProcessHolderGroup<TPosition> group, long activityId)
            where TPosition : IProcessHolderPosition => GetPosition(group.Positions, t => (t.Session as ActivityStart)?.Activity.Id == activityId);

        /// <summary>
        /// Get the position by id of the running activity
        /// </summary>
        public static TPosition GetPositionByActivityId<TPosition>(this IEnumerable<TPosition> positions, long activityId)
            where TPosition : IProcessHolderPosition => GetPosition(positions, t => (t.Session as ActivityStart)?.Activity.Id == activityId);

        private static TPosition GetPosition<TPosition>(IEnumerable<TPosition> positions, Func<TPosition, bool> filter)
            where TPosition : IProcessHolderPosition => positions.SingleOrDefault(filter);

        #endregion

        #region Get or Create Sessions

        /// <summary>
        /// Try cast and return the holders session property
        /// </summary>
        public static TSession ConvertSession<TSession>(this IProcessHolderPosition holderPosition)
            where TSession : Session => holderPosition.Session as TSession;

        /// <summary>
        /// Get or create a session for the <paramref name="position"/>, if it holds a process. Otherwise returns an empty enumerable.
        /// This is usually used when attaching to the control system.
        /// </summary>
        public static IEnumerable<Session> Attach(this ProcessHolderPosition position)
        {
            if (position.Session != null)
                return new[] { position.Session };

            return position.Process == null ? Enumerable.Empty<Session>() : new [] { position.StartSession() };
        }

        /// <summary>
        /// Get or create sessions for all <paramref name="positions"/> that have a process. 
        /// This is usually used when attaching to the control system.
        /// </summary>
        public static IEnumerable<Session> Attach(this IEnumerable<ProcessHolderPosition> positions) 
            => positions.SelectMany(p => p.Attach());

        /// <summary>
        /// Get a session if <paramref name="position"/> has one. Otherwise returns an empty enumerable. 
        /// This is usually used when detaching from the control system.
        /// </summary>
        public static IEnumerable<Session> Detach(this ProcessHolderPosition position) 
            => position.Session != null ? new[] { position.Session } : Enumerable.Empty<Session>();

        /// <summary>
        /// Create sessions for all positions on a holder group, that have a process
        /// </summary>
        public static IEnumerable<Session> Detach(this IEnumerable<ProcessHolderPosition> positions) 
            => positions.Where(p => p.Session != null).Select(p => p.Session);

        #endregion

        #region Mounting

        /// <summary>
        /// Assign a <paramref name="process"/> to this position
        /// </summary>
        public static void Mount(this IProcessHolderPosition position, IProcess process) 
            => position.Mount(new MountInformation(process, null));

        /// <summary>
        /// Assign <paramref name="process"/> and <paramref name="session"/> to this position
        /// </summary>
        public static void Mount(this IProcessHolderPosition position, IProcess process, Session session) 
            => position.Mount(new MountInformation(process, session));

        #endregion

        #region Has

        /// <summary>
        /// Checks if the group holds a process with a finished activity having the matching result
        /// </summary>
        public static bool HasFinishedActivity<TPosition>(this IProcessHolderGroup<TPosition> group, long activityId, long activityResult)
            where TPosition : IProcessHolderPosition => group.Positions.Any(position => position.HasFinishedActivity(activityId, activityResult));

        /// <summary>
        /// Checks if the position holds a process with a finished activity having the matching result.
        /// </summary>
        public static bool HasFinishedActivity(this IProcessHolderPosition holderPosition, long activityId, long activityResult)
            => holderPosition.Process?.GetActivities(activity => activity.Id == activityId && activity.Result?.Numeric == activityResult).Any() == true;

        #endregion

        /// <summary>
        /// Access tracing of the current activity
        /// </summary>
        public static TTracing Tracing<TTracing>(this IProcessHolderPosition position)
            where TTracing : Tracing, new() => (position.Session as ActivityStart)?.Activity?.TransformTracing<TTracing>();
    }
}