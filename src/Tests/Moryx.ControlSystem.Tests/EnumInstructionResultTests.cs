// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.ControlSystem.VisualInstructions;
using NUnit.Framework;
using System.Linq;

namespace Moryx.ControlSystem.Tests
{
    [TestFixture]
    public class EnumInstructionResultTests
    {
        private enum TestResults1
        {
            Value1,
            Value2
        }

        [Test]
        public void GetAllValuesIfNoResultIsDecorated()
        {
            // Act
            var instructionResult = new EnumInstructionResult(typeof(TestResults1), result => { });

            // Assert
            Assert.AreEqual(2, instructionResult.Results.Count(), "There should be 2 results because all of the results are not decorated");
        }

        private enum TestResults2
        {
            [EnumInstruction("Value1")]
            Value1,
            [EnumInstruction("Value2")]
            Value2
        }

        [Test]
        public void GetAllValuesIfAllResultsAreDecorated()
        {
            // Act
            var instructionResult = new EnumInstructionResult(typeof(TestResults2), result => { });

            // Assert
            Assert.AreEqual(2, instructionResult.Results.Count(), "There should be 2 results because all of the results are decorated");
        }

        private enum TestResults3
        {
            [EnumInstruction("Value1")]
            Value1,
            Value2
        }

        [Test]
        public void GetValuesWithAnAttribute()
        {
            // Act
            var instructionResult = new EnumInstructionResult(typeof(TestResults3), result => { });

            // Assert
            Assert.AreEqual(1, instructionResult.Results.Count(), "There should be 1 result because only one value is decorated");
        }

        private enum TestResults4
        {
            [EnumInstruction("Value1", Hide = true)]
            Value1,
            Value2
        }

        [Test]
        public void GetAllValuesIfThereAreHiddenAndNotDecoratedResults()
        {
            // Act
            var instructionResult = new EnumInstructionResult(typeof(TestResults4), result => { });

            // Assert
            Assert.AreEqual(2, instructionResult.Results.Count(), "There should be no results because there are hidden and not decorated results");
        }

        private enum TestResults5
        {
            [EnumInstruction("Value1", Hide = true)]
            Value1,
            [EnumInstruction("Value2", Hide = true)]
            Value2
        }

        [Test]
        public void GetNoValuesIfAllResultsAreHidden()
        {
            // Act
            var instructionResult = new EnumInstructionResult(typeof(TestResults5), result => { });

            // Assert
            Assert.AreEqual(0, instructionResult.Results.Count(), "There should be no results because all of them are hidden");
        }

        [Test]
        public void ProvidePopulatedInputs()
        {
            // Arrange
            int value = 0;
            var input = new MyInput();
            var instructionResult = new EnumInstructionResult(typeof(TestResults1), input, (result, inputs) => value = ((MyInput)inputs).Foo);

            // Act
            Assert.NotNull(instructionResult.Input);
            instructionResult.Invoke("Value1", new MyInput { Foo = 42 });

            // Assert
            Assert.AreEqual(42, value);
        }

        private class MyInput
        {
            public int Foo { get; set; }
        }
    }
}
