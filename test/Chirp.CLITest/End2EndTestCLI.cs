using System.Diagnostics;

namespace Chirp.CLITest;

public class End2EndTestCLI : CLITester
{
    public End2EndTestCLI(CLIFixture fixture) : base(fixture)
    {
        var webService = new CSVDBService.WebService(fixture.TestBase);
        Task.Run(() => webService.Run());
        Thread.Sleep(1000); // Just to make sure the webservice has enough time to boot up
    }
    
    [Fact]
    public void TestReadCheeps()
    {
        // Act
        RunProgram("read", out string output);

        // Assert
        Assert.StartsWith("Author1", output);
        Assert.EndsWith("Message2", output);
    }

    [Fact]
    public void TestWriteCheep()
    {
        // Arrange
        const string cheepMessage = "Test cheep message";
        string expected = $"{Environment.UserName},{cheepMessage}";

        // Act
        RunProgram($"cheep \"{cheepMessage}\"", out string _);

        // Assert
        // Verify the cheep has been stored
        string[] cheeps = ReadFile();
        Assert.Contains(expected, cheeps[^1]);
    }

    private static void RunProgram(string command, out string output)
    {
        using (var process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            const string projectPath = "src/Chirp.CLI/Chirp.CLI.csproj";
            process.StartInfo.Arguments = $"run --project {projectPath} {command}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            // Synchronously read the standard output of the spawned process.
            using (StreamReader reader = process.StandardOutput)
            {
                output = reader.ReadToEnd().Trim();
            }
            process.WaitForExit();
        }
    }
}