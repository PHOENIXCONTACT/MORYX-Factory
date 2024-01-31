using System.Collections.Generic;

namespace Moryx.ControlSystem.Machines
{
    /// <summary>
    /// Event args for requesting a machine state change
    /// </summary>
    public class MachineStateChangeRequestEventArgs : MachineStateEventArgs
    {
        /// <summary>
        /// List of restrictions which prevents the state to be set
        /// </summary>
        public IReadOnlyCollection<string> Restrictions => _restrictions;

        private readonly List<string> _restrictions = new List<string>();

        /// <summary>
        /// Creates a new instance of the <see cref="MachineStateChangeRequestEventArgs"/>
        /// </summary>
        /// <param name="state"></param>
        public MachineStateChangeRequestEventArgs(MachineState state) : base(state)
        {
        }

        /// <summary>
        /// Adds a restriction to prevent changing this state
        /// </summary>
        public void AddRestriction(string restriction)
        {
            _restrictions.Add(restriction);
        }
    }
}