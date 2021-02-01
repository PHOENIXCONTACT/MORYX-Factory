// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using Moryx.ControlSystem.Cells;

namespace Moryx.ControlSystem.VisualInstructions
{
    /// <summary>
    /// Specialized instruction result for activities
    /// </summary>
    internal class ActivityInstructionResult : EnumInstructionResult
    {
        private readonly Action<int, ActivityStart> _callback;
        private readonly ActivityStart _activityStart;

        /// <summary>
        /// Create a new result instance
        /// </summary>
        public ActivityInstructionResult(Type resultEnum, Action<int, ActivityStart> callback, ActivityStart activityStart)
            : base(resultEnum)
        {
            _callback = callback;
            _activityStart = activityStart;
        }

        /// <inheritdoc />
        public override void Invoke(string result)
        {
            _callback(ParseEnum(result), _activityStart);
        }
    }
}
