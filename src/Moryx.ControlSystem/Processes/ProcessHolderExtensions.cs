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
        /// <summary>
        /// Get the position by number
        /// </summary>
        public static TPosition GetPositionByIdentifier<TPosition>(this IProcessHolderGroup<TPosition> group, string identifier) 
            where TPosition : IProcessHolderPosition
        {
            return GetPosition(group.Positions, t => t.Identifier == identifier);
        }

        /// <summary>
        /// Get the position by number
        /// </summary>
        public static TPosition GetPositionByIdentifier<TPosition>(this IEnumerable<TPosition> positions, string identifier)
            where TPosition : IProcessHolderPosition
        {
            return GetPosition(positions, t => t.Identifier == identifier);
        }

        /// <summary>
        /// Get the position by session
        /// </summary>
        public static TPosition GetPositionBySession<TPosition>(this IProcessHolderGroup<TPosition> group, Session session)
            where TPosition : IProcessHolderPosition
        {
            return GetPosition(group.Positions, t => t.Session?.Id == session.Id);
        }

        /// <summary>
        /// Get the position by session
        /// </summary>
        public static TPosition GetPositionBySession<TPosition>(this IEnumerable<TPosition> positions, Session session)
            where TPosition : IProcessHolderPosition
        {
            return GetPosition(positions, t => t.Session?.Id == session.Id);
        }

        /// <summary>
        /// Get the position by its process id
        /// </summary>
        public static TPosition GetPositionByProcessId<TPosition>(this IProcessHolderGroup<TPosition> group, long processId)
            where TPosition : IProcessHolderPosition
        {
            return GetPosition(group.Positions, t => t.Process?.Id == processId);
        }

        /// <summary>
        /// Get the position by its process id
        /// </summary>
        public static TPosition GetPositionByProcessId<TPosition>(this IEnumerable<TPosition> positions, long processId)
            where TPosition : IProcessHolderPosition
        {
            return GetPosition(positions, t => t.Process?.Id == processId);
        }

        /// <summary>
        /// Get the position by id of the running activity
        /// </summary>
        public static TPosition GetPositionByActivityId<TPosition>(this IProcessHolderGroup<TPosition> group, long activityId)
            where TPosition : IProcessHolderPosition
        {
            return GetPosition(group.Positions, t => (t.Session as ActivityStart)?.Activity.Id == activityId);
        }

        /// <summary>
        /// Checks if the group holds a process with a finished activity having the matching result
        /// </summary>
        public static bool HasFinishedActivity<TPosition>(this IProcessHolderGroup<TPosition> group, long activityId, long activityResult)
            where TPosition : IProcessHolderPosition
        {
            return group.Positions.Any(position => position.HasFinishedActivity(activityId, activityResult));
        }
        /// <summary>
        /// Checks if the position holds a process with a finished activity having the matching result.
        /// </summary>
        public static bool HasFinishedActivity(this IProcessHolderPosition holderPosition, long activityId, long activityResult)
        {
            return holderPosition.Process?.GetActivities(activity => activity.Id == activityId && activity.Result?.Numeric == activityResult).Any() == true;
        }

        /// <summary>
        /// Get the position by id of the running activity
        /// </summary>
        public static TPosition GetPositionByActivityId<TPosition>(this IEnumerable<TPosition> positions, long activityId)
            where TPosition : IProcessHolderPosition
        {
            return GetPosition(positions, t => (t.Session as ActivityStart)?.Activity.Id == activityId);
        }

        private static TPosition GetPosition<TPosition>(IEnumerable<TPosition> positions, Func<TPosition, bool> filter)
            where TPosition : IProcessHolderPosition
        {
            return positions.SingleOrDefault(filter);
        }

        /// <summary>
        /// Try cast and return the holders session property
        /// </summary>
        public static TSession ConvertSession<TSession>(this IProcessHolderPosition holderPosition)
            where TSession : Session
        {
            return holderPosition.Session as TSession;
        }

        /// <summary>
        /// Access tracing of the current activity
        /// </summary>
        public static TTracing Tracing<TTracing>(this IProcessHolderPosition position)
            where TTracing : Tracing, new()
        {
            var currentActivity = (position.Session as ActivityStart)?.Activity;
            return currentActivity?.TransformTracing<TTracing>();
        }

        /// <summary>
        /// Create sessions for all positions on a holder group, that have a process
        /// </summary>
        public static IEnumerable<Session> Attach(this ProcessHolderPosition position)
        {
            if (position.Session != null)
                return new[] { position.Session };

            return position.Process == null ? Enumerable.Empty<Session>() : new [] { position.StartSession() };
        }

        /// <summary>
        /// Create sessions for all positions on a holder group, that have a process
        /// </summary>
        public static IEnumerable<Session> Attach(this IEnumerable<ProcessHolderPosition> positions)
        {
            return positions.SelectMany(p => p.Attach());
        }

        /// <summary>
        /// Create sessions for all positions on a holder group, that have a process
        /// </summary>
        public static IEnumerable<Session> Detach(this ProcessHolderPosition position)
        {
            return position.Session != null ? new[] { position.Session } : Enumerable.Empty<Session>();
        }

        /// <summary>
        /// Create sessions for all positions on a holder group, that have a process
        /// </summary>
        public static IEnumerable<Session> Detach(this IEnumerable<ProcessHolderPosition> positions)
        {
            return positions.Where(p => p.Session != null).Select(p => p.Session);
        }

        /// <summary>
        /// Assign process and session to this position
        /// </summary>
        public static void Mount(this IProcessHolderPosition position, IProcess process)
        {
            position.Mount(new MountInformation(process, null));
        }

        /// <summary>
        /// Assign process and session to this position
        /// </summary>
        public static void Mount(this IProcessHolderPosition position, IProcess process, Session session)
        {
            position.Mount(new MountInformation(process, session));
        }
    }
}