using System;
using System.Reflection;

using Chirp.CLI;

using Moq;
using SimpleDB;

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
        [InlineData("Author", "Message", -315619200, "Author          @ 01-01-60 01:00:00: Message")] // Date before Unix epoch
        public void Cheep_ToString_ExtremeTimestamps(string author, string message, long timestamp, string expected)
        {
            // Arrange
            var cheep = new Cheep(author, message, timestamp);

            // Act
            string result = cheep.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void UserInterface_SetCheepsCsvPath()
        {
            // Act
            string path = "file.csv";
            UserInterface.SetCheepsCsvPath(path);

            // Assert
            // Gets the values from variables that are not public or are static
            var cheepsCsvPathField = typeof(UserInterface).GetField("cheepsCsvPath", BindingFlags.NonPublic | BindingFlags.Static);
            var cheepBaseField = typeof(UserInterface).GetField("cheepBase", BindingFlags.NonPublic | BindingFlags.Static);

            // Check the variables are not null
            Assert.NotNull(cheepsCsvPathField);
            Assert.NotNull(cheepBaseField);

            // Retuns the values of the two variables declared above
            string? actualPath = cheepsCsvPathField.GetValue(null) as string;
            var actualCheepBase = cheepBaseField.GetValue(null);

            Assert.Equal(path, actualPath);
            Assert.NotNull(actualCheepBase);
        }

        [Fact]
        public void UserInterace_ReadCheeps_ReadAll()
        {
            // Arrange
            string relativePath = Path.Combine("..", "..", "..", "..", "..", "data", "testRead.csv");
            string path = Path.GetFullPath(relativePath);
            UserInterface.SetCheepsCsvPath(path);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                UserInterface.ReadCheeps(null);

                // Assert
                string expectedOutput = "Author1         @ 08-01-21 21:31:01: Message1" + Environment.NewLine +
                                        "Author2         @ 08-01-21 21:31:02: Message2" + Environment.NewLine;
                Assert.Equal(expectedOutput, sw.ToString());
            }
        }

        [Theory]
        [InlineData(0, "")]
        [InlineData(1, "Author2         @ 08-01-21 21:31:02: Message2\r\n")]
        [InlineData(2, "Author1         @ 08-01-21 21:31:01: Message1\r\nAuthor2         @ 08-01-21 21:31:02: Message2\r\n")]
        [InlineData(null, "Author1         @ 08-01-21 21:31:01: Message1\r\nAuthor2         @ 08-01-21 21:31:02: Message2\r\n")]
        public void UserInterace_ReadCheeps_ReadSpecific(int? limit, string expected)
        {
            // Arrange
            string relativePath = Path.Combine("..", "..", "..", "..", "..", "data", "testRead.csv");
            string path = Path.GetFullPath(relativePath);
            UserInterface.SetCheepsCsvPath(path);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                UserInterface.ReadCheeps(limit);

                // Assert
                Assert.Equal(expected, sw.ToString());
            }
        }

        [Fact]
        public void UserInterace_ReadCheeps_NegativeRead()
        {
            // Arrange
            int limit = -1;
            string relativePath = Path.Combine("..", "..", "..", "..", "..", "data", "testRead.csv");
            string path = Path.GetFullPath(relativePath);
            UserInterface.SetCheepsCsvPath(path);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                UserInterface.ReadCheeps(limit);

                // Assert
                Assert.Equal("", sw.ToString());
            }
        }

        [Fact]
        public void UserInterace_ReadCheeps_Exception()
        {
            // Act & Assert
            Assert.Throws(typeof(InvalidOperationException), () =>  UserInterface.ReadCheeps(null));
        }

        [Fact]
        public void UserInterface_WriteCheep()
        {
            // Arrange
            var mockRepository = new Mock<IDatabaseRepository<Cheep>>();
            UserInterface.SetCheepsCsvPath("testWrite.csv");
            UserInterface.SetCheepBase(mockRepository.Object);
            string message = "Test message";
            string expectedUserName = Environment.UserName;
            long expectedTimestamp = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();

            // Act
            UserInterface.WriteCheep(message);

            // Assert
            mockRepository.Verify(repo => repo.Store(It.Is<Cheep>(cheep =>
                cheep.Author == expectedUserName &&
                cheep.Message == message &&
                cheep.Timestamp == expectedTimestamp
            )), Times.Once);
        }

        [Fact]
        public void UserInterace_WriteCheep_Exception()
        {
            // Act & Assert
            Assert.Throws(typeof(InvalidOperationException), () => UserInterface.WriteCheep(null));
        }
    }
}