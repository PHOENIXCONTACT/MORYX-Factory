// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Moryx.ControlSystem.VisualInstructions
{
    /// <summary>
    /// Represents an <see cref="IInstructionResults"/> which will handle enums to generate results and convert them back
    /// </summary>
    public class EnumInstructionResult : IInstructionResults
    {
        /// <inheritdoc />
        public string[] Results => _valueMap.Keys.ToArray();

        private readonly Action<int> _callback;

        private Dictionary<string, int> _valueMap = new Dictionary<string, int>();

        /// <summary>
        /// Creates a new instance of <see cref="EnumInstructionResult"/>
        /// </summary>
        protected EnumInstructionResult(Type resultEnum)
        {
            var allValues = new Dictionary<string, int>();
            foreach (var name in Enum.GetNames(resultEnum))
            {
                var member = resultEnum.GetMember(name)[0];
                var attribute = (DisplayAttribute)member.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
                var text = attribute?.Name ?? name;
                allValues[text] = (int)Enum.Parse(resultEnum, name);
                if (attribute != null)
                    _valueMap[text] = allValues[text];
            }

            // If we found no entries, the display attribute was not used and we take all entries
            if (_valueMap.Count == 0)
                _valueMap = allValues;
        }

        /// <summary>
        /// Creates a new instance of <see cref="EnumInstructionResult"/>
        /// </summary>
        /// <param name="resultEnum">Enum type which will be used to create instruction results</param>
        /// <param name="callback">Callback with enum result value of the executed instruction</param>
        public EnumInstructionResult(Type resultEnum, Action<int> callback) : this(resultEnum)
        {
            _callback = callback;
        }

        /// <summary>
        /// Invokes the callback with the given string result
        /// Will parse the string to the enum value
        /// </summary>
        /// <param name="result"></param>
        public virtual void Invoke(string result)
        {
            _callback(ParseEnum(result));
        }

        /// <summary>
        /// Parses the given string to the responsible enum value
        /// </summary>
        /// <param name="value">String to parse to enum value</param>
        protected int ParseEnum(string value)
        {
            return _valueMap[value];
        }
    }
}
