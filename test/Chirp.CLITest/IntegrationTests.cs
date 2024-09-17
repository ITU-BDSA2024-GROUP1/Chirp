using System.IO;

using Chirp.CLI;

using SimpleDB;

namespace Chirp.CLITest
{
    [Collection("Non-Parallel Collection")]
    public class IntegrationTests
    {
        [Fact]
        public void UserInterface_WriteReadCheeps()
        {
            // Arrange
            int limit = 1;
            string path = "data/test.csv";
            string message = "Test message";
            string expected = $"{Environment.UserName,-15} @ {((DateTimeOffset)DateTime.Now),17:MM\\/dd\\/yy HH\\:mm\\:ss}: {message}\r\n";
            string cheep = $"{Environment.UserName},{message},{(DateTimeOffset)DateTime.Now}";
            Program.SetWorkingDirectoryToProjectRoot();
            CSVDatabase<Cheep>.InTestingDatabase = true;
            UserInterface.WriteCheep(message);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                UserInterface.ReadCheeps(limit);

                // Assert
                Assert.Equal(expected, sw.ToString());
            }

            // Clean up - Remove the cheep
            string[] cheeps = File.ReadAllLines(path);
            File.WriteAllLines(path, cheeps.Take(cheeps.Length - 1).ToArray());

            // Verify the cheep has been removed
            cheeps = File.ReadAllLines(path);
            Assert.DoesNotContain(cheep, cheeps[cheeps.Length - 1]);
        }
    }
}
