using System;
using System.IO;
using System.Reflection;

using Chirp.CLI;
using Chirp.Core;

using SimpleDB;

namespace Chirp.CLITest
{
    [Collection("Non-Parallel Collection")]
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
            string expected = "Author          @ 08/01/21 19:31:01: Message";

            // Act
            string result = cheep.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Author", "Message", 0, "Author          @ 01/01/70 00:00:00: Message")] // Unix epoch start
        [InlineData("Author", "Message", 253402300799, "Author          @ 12/31/99 23:59:59: Message")] // End of 9999 year
        [InlineData("Author", "Message", 10000000000, "Author          @ 11/20/86 17:46:40: Message")] // Far future date
        [InlineData("Author", "Message", -315619200, "Author          @ 01/01/60 00:00:00: Message")] // Date before Unix epoch
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
        public void UserInterace_ReadCheeps_ReadAll()
        {
            // Arrange
            DirectoryFixer.SetWorkingDirectoryToProjectRoot();
            CSVDatabase<Cheep>.InTestingDatabase = true;

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                UserInterface.ReadCheeps();

                // Assert
                string expectedOutput = "Author1         @ 08/01/21 19:31:01: Message1" + Environment.NewLine +
                                        "Author2         @ 08/01/21 19:31:02: Message2" + Environment.NewLine;
                Assert.Equal(expectedOutput, sw.ToString());
            }
        }

        public static IEnumerable<object[]> GetTestData()
        {
            yield return new object[] { 0, "" };
            yield return new object[] { 1, $"Author2         @ 08/01/21 19:31:02: Message2{Environment.NewLine}" };
            yield return new object[] { 2, $"Author1         @ 08/01/21 19:31:01: Message1{Environment.NewLine}Author2         @ 08/01/21 19:31:02: Message2{Environment.NewLine}" };
            yield return new object[] { null!, $"Author1         @ 08/01/21 19:31:01: Message1{Environment.NewLine}Author2         @ 08/01/21 19:31:02: Message2{Environment.NewLine}" };
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void UserInterface_ReadCheeps_ReadSpecific(int? limit, string expected)
        {
            // Arrange
            DirectoryFixer.SetWorkingDirectoryToProjectRoot();
            CSVDatabase<Cheep>.InTestingDatabase = true;

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
            DirectoryFixer.SetWorkingDirectoryToProjectRoot();
            CSVDatabase<Cheep>.InTestingDatabase = true;

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
        public void UserInterface_WriteCheep()
        {
            // Arrange
            DirectoryFixer.SetWorkingDirectoryToProjectRoot();
            CSVDatabase<Cheep>.InTestingDatabase = true;
            string path = Path.Combine(End2EndTests.FindPathToMainDirectoryChirp(), "data/test.csv");
            string message = "Test message";
            string expectedUserName = Environment.UserName;
            long expectedTimestamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            string cheep = $"{expectedUserName},{message},{expectedTimestamp}";

            // Act
            UserInterface.WriteCheep(message);

            // Assert
            // Verify the cheep has been stored
            string[] cheeps = File.ReadAllLines(path);
            Assert.Contains(cheep, cheeps[^1]);

            // Clean up - Remove the cheep
            File.WriteAllLines(path, cheeps.Take(cheeps.Length - 1).ToArray());

            // Verify the cheep has been removed
            cheeps = File.ReadAllLines(path);
            Assert.DoesNotContain(cheep, cheeps[^1]);
        }
    }
}