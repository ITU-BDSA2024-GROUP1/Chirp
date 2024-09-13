using Chirp.CLI;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Chirp.CLITest
{
    public class IntegrationTests
    {
        [Fact]
        public void UserInterface_WriteReadCheeps()
        {
            // Arrange
            int limit = 1;
            string message = "Test message";
            string expected = $"{Environment.UserName}           @ {((DateTimeOffset)DateTime.Now),17:MM/dd/yy HH:mm:ss}: {message}\r\n";
            string relativePath = Path.Combine("..", "..", "..", "..", "..", "data", "testWrite.csv");
            string path = Path.GetFullPath(relativePath);
            UserInterface.SetCheepsCsvPath(path);
            UserInterface.WriteCheep(message);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                UserInterface.ReadCheeps(limit);

                // Assert
                Assert.Equal(expected, sw.ToString());
            }
        }
    }
}
