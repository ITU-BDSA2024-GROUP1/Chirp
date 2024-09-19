using Chirp.Core;

namespace Chirp.CLITest;

public class IntegrationTests : CLITester
{
    public IntegrationTests(CLIFixture fixture) : base(fixture) { }
        
    [Fact]
    public void UserInterface_WriteReadCheeps()
    {
        // Arrange
        const string message = "Test message";
        string expected = $"{new Cheep(message)}{Environment.NewLine}";
        
        _fixture.UserInterface.WriteCheep(message);

        using (StringWriter sw = new())
        {
            Console.SetOut(sw);
            
            // Act
            _fixture.UserInterface.ReadCheeps(1);

            // Assert
            Assert.Equal(expected, sw.ToString());
        }
    }
}