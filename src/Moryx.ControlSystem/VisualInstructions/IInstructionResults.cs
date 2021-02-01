// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

namespace Moryx.ControlSystem.VisualInstructions
{
    /// <summary>
    /// Interface to adapt different types of objects to instruction results
    /// Results will be provided by the visual instructor
    /// </summary>
    public interface IInstructionResults
    {
        /// <summary>
        /// Possibile results for the instructions
        /// </summary>
        string[] Results { get; }

        /// <summary>
        /// Converts the string representation of an result to any kind of object
        /// <example>
        /// String result will be converted to an enum value
        /// </example>
        /// </summary>
        void Invoke(string result);
    }
}
