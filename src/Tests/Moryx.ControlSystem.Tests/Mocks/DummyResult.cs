namespace Moryx.ControlSystem.Tests.Mocks
{
    public enum DummyResult
    {
        /// <summary>
        /// Production step was successful
        /// </summary>
        Done,

        /// <summary>
        /// Production step was not successful
        /// </summary>
        Failed,

        /// <summary>
        /// The activity could not be started at all.
        /// </summary>
        TechnicalFailure
    }
}