using System;
using System.IO;
using Xunit;
using ResourceShortageManager;

namespace ResourceShortageManager.Tests
{
    public class UtilsTests
    {
        [Fact]
        public void PromptUser_ValidInput_ReturnsInput()
        {
            // Arrange
            var input = "TestInput";
            var stringReader = new StringReader(input);
            Console.SetIn(stringReader);

            // Act
            var result = Utils.PromptUser("Test message");

            // Assert
            Assert.Equal(input, result);
        }

        [Fact]
        public void PromptUser_ExitInput_ReturnsNull()
        {
            // Arrange
            var input = "exit";
            var stringReader = new StringReader(input);
            Console.SetIn(stringReader);

            // Act
            var result = Utils.PromptUser("Test message");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void PromptUserInt_ValidInput_ReturnsParsedInt()
        {
            // Arrange
            var input = "123";
            var stringReader = new StringReader(input);
            Console.SetIn(stringReader);

            // Act
            var result = Utils.PromptUserInt("Test message");

            // Assert
            Assert.Equal(123, result);
        }

        [Fact]
        public void PromptUserInt_ExitInput_ReturnsNull()
        {
            // Arrange
            var input = "exit";
            var stringReader = new StringReader(input);
            Console.SetIn(stringReader);

            // Act
            var result = Utils.PromptUserInt("Test message");

            // Assert
            Assert.Null(result);
        }
    }
}
