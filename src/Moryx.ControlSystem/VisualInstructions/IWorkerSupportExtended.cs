using System.Collections.Generic;

namespace Moryx.ControlSystem.VisualInstructions
{
	public interface IWorkerSupportExtended : IWorkerSupport
	{
		/// <summary>
		/// Get a list of all available instructors
		/// </summary>
		public IReadOnlyList<string> GetInstructors();
	}
}