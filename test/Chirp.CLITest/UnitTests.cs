using System;

using Chirp.CLI;

namespace Chirp.CLITest
{
    public class UnitTests
    {
        [Fact]
        public void Cheep_Instantiation()
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
        public void Cheep_ToString()
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

        [Theory]
        [InlineData("Author", "Message", 0, "Author          @ 01-01-70 01:00:00: Message")] // Unix epoch start
        [InlineData("Author", "Message", 253402300799, "Author          @ 12-31-99 23:59:59: Message")] // End of 9999 year
        [InlineData("Author", "Message", 10000000000, "Author          @ 11-20-86 18:46:40: Message")] // Far future date
        public void Cheep_ToString_ExtremeTimestamps(string author, string message, long timestamp, string expected)
        {
            // Arrange
            var cheep = new Cheep(author, message, timestamp);

            // Act
            string result = cheep.ToString();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}