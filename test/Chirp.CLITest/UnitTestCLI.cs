namespace Chirp.CLITest;

[Collection("Chirp.CLI Collection")]
public class UnitTestCLI
{
    private readonly CLIFixture _fixture;
    
    public UnitTestCLI(CLIFixture fixture) => _fixture = fixture;

    [Fact]
    public void UserInterface_ReadCheeps_ReadAll()
    {
        // Arrange
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);

            // Act
            _fixture.UserInterface.ReadCheeps();

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
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);

            // Act
            _fixture.UserInterface.ReadCheeps(limit);

            // Assert
            Assert.Equal(expected, sw.ToString());
        }
    }

    [Fact]
    public void UserInterface_ReadCheeps_NegativeRead()
    {
        // Arrange
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);

            // Act
            _fixture.UserInterface.ReadCheeps(-1);

            // Assert
            Assert.Equal("", sw.ToString());
        }
    }

    [Fact]
    public void UserInterface_WriteCheep()
    {
        // Arrange
        string path = "data/test.csv";
        const string message = "Test message";
        string expectedUserName = Environment.UserName;
        long expectedTimestamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
        string cheep = $"{expectedUserName},{message},{expectedTimestamp}";

        // Act
        _fixture.UserInterface.WriteCheep(message);

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