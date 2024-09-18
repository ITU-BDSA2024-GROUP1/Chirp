using System.Diagnostics;

using Xunit.Abstractions;

namespace Chirp.CLITest
{
    [Collection("Non-Parallel Collection")]
    public class End2EndTests
    {
        private readonly ITestOutputHelper _output;

        public End2EndTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TestReadCheeps()
        {
            // Arrange
            string path = FindPathToMainDirectoryChirp();
            Directory.SetCurrentDirectory(path);

            // Act
            string output = "";
            using (var process = new Process())
            {
                process.StartInfo.FileName = "dotnet";
                string projectPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "src", "Chirp.CLI", "Chirp.CLI.csproj"));
                process.StartInfo.Arguments = $"run --project {projectPath} read";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

                // Synchronously read the standard output of the spawned process.
                using (StreamReader reader = process.StandardOutput)
                {
                    output = reader.ReadToEnd();
                }
                process.WaitForExit();
            }

            _output.WriteLine(output);

            // Split the output into lines
            string[] outputArray = output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            // Find the index of the first occurrence of "ropf"
            int startIndex = Array.FindIndex(outputArray, line => line.StartsWith("ropf"));

            // If "ropf" is found, skip lines before it
            string[] filteredOutputArray = startIndex >= 0 ? outputArray.Skip(startIndex).ToArray() : outputArray;

            // Process the filtered output
            string firstCheep = filteredOutputArray[0]; // Get the first matching line

            // Assert
            Assert.StartsWith("ropf", firstCheep);
            Assert.EndsWith("Hello, BDSA students!", firstCheep);
        }

        [Fact]
        public void TestWriteCheep()
        {
            // Arrange
            string cheepMessage = "Test cheep message";
            string author = Environment.UserName;
            string cheep = $"{author},{cheepMessage}";
            string path = FindPathToMainDirectoryChirp();
            string dbPath = Path.GetFullPath(Path.Combine(path, "data", "chirp_cli_db.csv"));

            // Act
            using (var process = new Process())
            {
                process.StartInfo.FileName = "dotnet";
                string projectPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "src", "Chirp.CLI", "Chirp.CLI.csproj"));
                process.StartInfo.Arguments = $"run --project {projectPath} cheep \"{cheepMessage}\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
            }

            // Assert
            // Verify the cheep has been stored
            string[] cheeps = File.ReadAllLines(dbPath);
            Assert.Contains(cheep, cheeps[^1]);

            // Clean up - Remove the cheep
            File.WriteAllLines(dbPath, cheeps.Take(cheeps.Length - 1).ToArray());

            // Verify the cheep has been removed
            cheeps = File.ReadAllLines(dbPath);
            Assert.DoesNotContain(cheep, cheeps[^1]);
        }

        public static string FindPathToMainDirectoryChirp()
        {
            string path = "";
            if (Path.GetFileName(Directory.GetCurrentDirectory()) == "Chirp")
            {
                path = Path.GetFullPath(Directory.GetCurrentDirectory());
            }
            else
            {
                while (true)
                {
                    if (Path.GetFileName(Directory.GetCurrentDirectory()) == "Chirp")
                    {
                        break;
                    }
                    else
                    {
                        Directory.SetCurrentDirectory(Path.GetFullPath(".."));
                    }
                }
                path = Directory.GetCurrentDirectory();
            }
            return path;
        }
    }
}
