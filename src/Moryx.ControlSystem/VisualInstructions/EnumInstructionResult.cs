// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Moryx.ControlSystem.VisualInstructions
{
    /// <summary>
    /// Represents an <see cref="IInstructionResults"/> which will handle enums to generate results and convert them back
    /// </summary>
    public class EnumInstructionResult : IInstructionInputResults, IInstructionKeyResults
    {
        private readonly Action<int, object> _callback;
        private readonly Dictionary<string, int> _valueMap = new Dictionary<string, int>();

        /// <inheritdoc />
        public string[] Results => _valueMap.Keys.ToArray();

        /// <inheritdoc />
        public InstructionResult[] PossibleResults => _valueMap.Select(pair => new InstructionResult
        {
            Key = pair.Value.ToString("D"),
            DisplayValue = pair.Key
        }).ToArray();

        /// <summary>
        /// Creates a new instance of <see cref="EnumInstructionResult"/>
        /// </summary>
        public EnumInstructionResult(Type resultEnum, Action<int> callback, params string[] exceptions)
            : this(resultEnum, (result, input) => callback(result), exceptions)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="EnumInstructionResult"/>
        /// </summary>
        /// <param name="resultEnum">Enum type which will be used to create instruction results</param>
        /// <param name="callback">Callback with enum result value of the executed instruction</param>
        /// <param name="exceptions">Excepted enum value names. Will be ignored for result</param>
        public EnumInstructionResult(Type resultEnum, Action<int, object> callback, params string[] exceptions)
        {
            _callback = callback;

            var allHidden = true;
            var allValues = new Dictionary<string, int>();
            foreach (var name in Enum.GetNames(resultEnum).Except(exceptions))
            {
                var member = resultEnum.GetMember(name)[0];
                // Try to fetch display name or title from attribute
                var displayName = member.GetDisplayName();
                var attribute = (EnumInstructionAttribute)member.GetCustomAttributes(typeof(EnumInstructionAttribute), false).FirstOrDefault();

                var text = displayName ?? attribute?.Title ?? name;
                allValues[text] = (int)Enum.Parse(resultEnum, name);

                if(attribute == null)
                {
                    allHidden = false;
                }
                else if(!attribute.Hide)
                {
                    allHidden = false;
                    _valueMap[text] = allValues[text];
                }
            }

            // If we found no entries, the display attribute was not used and we take all entries
            if (_valueMap.Count == 0 && !allHidden)
                _valueMap = allValues;
        }

        /// <summary>
        /// Invokes the callback with the given string result
        /// Will parse the string to the enum value
        /// </summary>
        public virtual void Invoke(string result)
        {
            var enumValue = _valueMap[result];
            _callback(enumValue, null);
        }

        /// <summary>
        /// Invokes the callback with the given string result
        /// Will parse the string to the enum value
        /// </summary>
        public void Invoke(string result, object input)
        {
            var enumValue = _valueMap[result];
            _callback(enumValue, input);
        }

        /// <summary>
        /// Invokes the callback with the given string result
        /// Will parse the string to the enum value
        /// </summary>
        public void Invoke(InstructionResult result, object input)
        {
            var enumValue = int.Parse(result.Key);
            _callback(enumValue, input);
        }
    }
}
