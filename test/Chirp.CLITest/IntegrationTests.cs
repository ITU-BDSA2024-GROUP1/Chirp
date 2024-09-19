using System.IO;

using Chirp.CLI;
using Chirp.Core;

using SimpleDB;

namespace Chirp.CLITest
{
    [Collection("Chirp.CLI Collection")]
    public class IntegrationTests
    {
        private readonly CLIFixture _fixture;
    
        public IntegrationTests(CLIFixture fixture) => _fixture = fixture;
        
        [Fact]
        public void UserInterface_WriteReadCheeps()
        {
            // Arrange
            int limit = 1;
            string path = "data/test.csv";
            string message = "Test message";
            string expected = $@"{Environment.UserName,-15} @ {((DateTimeOffset)DateTime.UtcNow),17:MM\/dd\/yy HH\:mm\:ss}: {message}{Environment.NewLine}";
            string cheep = $"{Environment.UserName},{message},{(DateTimeOffset)DateTime.UtcNow}";
            _fixture.UserInterface.WriteCheep(message);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                _fixture.UserInterface.ReadCheeps(limit);

                // Assert
                Assert.Equal(expected, sw.ToString());
            }

            // Clean up - Remove the cheep
            string[] cheeps = File.ReadAllLines(path);
            File.WriteAllLines(path, cheeps.Take(cheeps.Length - 1).ToArray());

            // Verify the cheep has been removed
            cheeps = File.ReadAllLines(path);
            Assert.DoesNotContain(cheep, cheeps[^1]);
        }
    }
}
