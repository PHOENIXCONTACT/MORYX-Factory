// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Moryx.ControlSystem.VisualInstructions
{
    /// <summary>
    /// Static helper class to convert result enums to button lables and parse the response to numeric values
    /// </summary>
    public static class EnumInstructionResult
    {
        /// <summary>
        /// Determine possbile string buttons from enum result
        /// </summary>
        public static IReadOnlyList<string> PossibleResults(Type resultEnum, params string[] exceptions)
        {
            return ParseEnum(resultEnum, exceptions).Keys.ToList();
        }

        /// <summary>
        /// Parse the given result back to an enum value
        /// </summary>
        public static int ResultToEnumValue(Type resultEnum, string result)
        {
            return ParseEnum(resultEnum)[result];
        }

        /// <summary>
        /// Parse the given enum to an dictionary of button text and value
        /// </summary>
        private static IDictionary<string, int> ParseEnum(Type resultEnum, params string[] exceptions)
        {
            var allValues = new Dictionary<string, int>();
            var displayValues = new Dictionary<string, int>();
            var hiddenValues = new Dictionary<string, int>();
            foreach (var name in Enum.GetNames(resultEnum).Except(exceptions))
            {
                var member = resultEnum.GetMember(name)[0];
                // Try to fetch display name or title from attribute
                var displayName = member.GetDisplayName();
                var attribute = (EnumInstructionAttribute)member.GetCustomAttributes(typeof(EnumInstructionAttribute), false).FirstOrDefault();

                var text = displayName ?? attribute?.Title ?? name;
                var numericValue = (int)Enum.Parse(resultEnum, name);
                allValues[text] = numericValue;

                if (attribute?.Hide == true)
                {
                    hiddenValues[text] = numericValue;
                }
                else if (attribute?.Hide == false)
                {
                    displayValues[text] = numericValue;
                }
            }

            // We have different cases
            // Case 1: A few values are explicity decorated => only display those
            // Note: In all following cases displayValues is 0
            if (displayValues.Count > 0)
            {
                return displayValues;
            }
            // Case 2: Nothing decorated or hidden => display all values
            if (hiddenValues.Count == 0)
            {
                return allValues;
            }
            
            // Case 3: All values are explicitly hidden => display nothing 
            if (allValues.Count == hiddenValues.Count)
            {
                return displayValues;
            }
            // Case 4: Some values hidden, nothing explicitly displayed => display all except hidden
            // This case does not a condition, since it's the only remaining option
            return allValues.Except(hiddenValues).ToDictionary(p => p.Key, p => p.Value);
        }
    }
}
