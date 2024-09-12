using System;

using Chirp.CLI;

namespace Chirp.CLITest
{
    public class UnitTests
    {
        [Fact]
        public void Cheep_Instantiation_ShouldCreateObject()
        {
            // Arrange
            string author = "Author";
            string message = "Message";
            long timestamp = 1627846261;

            // Act
            var cheep = new Cheep(author, message, timestamp);

            // Assert
            Assert.Equal(author, cheep.Author);
            Assert.Equal(message, cheep.Message);
            Assert.Equal(timestamp, cheep.Timestamp);
        }

        [Fact]
        public void Cheep_ToString_ShouldFormatCorrectly()
        {
            // Arrange
            string author = "Author";
            string message = "Message";
            long timestamp = 1627846261;
            var cheep = new Cheep(author, message, timestamp);
            string expected = "Author          @ 08-01-21 21:31:01: Message";

            // Act
            string result = cheep.ToString();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}