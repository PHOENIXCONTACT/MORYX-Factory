// Copyright (c) 2022, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

namespace Moryx.ControlSystem.VisualInstructions
{
    /// <summary>
    /// Active instruction for a task
    /// </summary>
    public class ActiveInstruction
    {
        /// <summary>
        /// Runtime unique identifier of this instruction
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Title of the instruction
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Items of the instruction
        /// </summary>
        public VisualInstruction[] Instructions { get; set; }

        /// <summary>
        /// Results of the instruction
        /// </summary>
        public string[] PossibleResults { get; set; } = new string[0];
    }
}