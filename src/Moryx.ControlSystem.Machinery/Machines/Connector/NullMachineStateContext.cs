namespace Moryx.ControlSystem.Machines
{
    /// <summary>
    /// Null context for state without any additional information
    /// </summary>
    public class NullMachineStateContext : IMachineStateContext
    {
        /// <inheritdoc />
        public bool Equals(IMachineStateContext other)
        {
            return other != null && this == other;
        }
    }
}