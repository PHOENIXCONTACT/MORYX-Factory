// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Resources;

namespace Moryx.ControlSystem.VisualInstructions
{
    /// <summary>
    /// Component for visual instructions black box reuse
    /// </summary>
    public interface IVisualInstructor : IResource
    {
        /// <summary>
        /// Only display these instructions
        /// Have to be cleared with the <see cref="Clear"/> method
        /// </summary>
        long Display(string sender, VisualInstruction[] instructions);

        /// <summary>
        /// Only display these instructions
        /// Instruction will automatically cleared after the given time
        /// </summary>
        void Display(string sender, VisualInstruction[] instructions, int autoClearMs);

        /// <summary>
        /// Execute these instructions based on the given activity and report the result on completion
        /// Can (but must not) be cleared with the <see cref="Clear"/> method
        /// </summary>
        long Execute(string sender, VisualInstruction[] parameter, IInstructionResults results);

        /// <summary>
        /// Clears specified Instruction from UI.
        /// </summary>
        void Clear(long instructionId);
    }
}
