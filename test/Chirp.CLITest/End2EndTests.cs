using System.Diagnostics;

namespace Chirp.CLITest
{
    [Collection("Non-Parallel Collection")]
    public class End2EndTests
    {
        [Fact]
        public void TestReadCheeps()
        {
            // Arrange
            string testPath = Path.Combine("..", "..", "..", "..");
            string path = Path.GetFileName(Directory.GetCurrentDirectory()) == "Chirp" ? Path.GetFullPath(Directory.GetCurrentDirectory()) : Path.GetFullPath(testPath);
            Directory.SetCurrentDirectory(Path.GetDirectoryName(path));

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
            string basePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "chirp_cli_db.csv");
            string testPath = Path.Combine("..", "..", "..", "..", "..", "data", "chirp_cli_db.csv");
            string path = Path.GetFileName(Directory.GetCurrentDirectory()) == "Chirp" ? Path.GetFullPath(basePath) : Path.GetFullPath(testPath);
            Directory.SetCurrentDirectory(Path.GetDirectoryName(path));

            // Act
            using (var process = new Process())
            {
                process.StartInfo.FileName = "dotnet";
                string projectPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "src", "Chirp.CLI", "Chirp.CLI.csproj"));
                process.StartInfo.Arguments = $"run --project {projectPath} cheep \"{cheepMessage}\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
            }

            // Assert
            // Verify the cheep has been stored
            string[] cheeps = File.ReadAllLines(path);
            Assert.Contains(cheep, cheeps[cheeps.Length-1]);

            // Clean up - Remove the cheep
            File.WriteAllLines(path, cheeps.Take(cheeps.Length - 1).ToArray());

            // Verify the cheep has been removed
            cheeps = File.ReadAllLines(path);
            Assert.DoesNotContain(cheep, cheeps[cheeps.Length-1]);
        }
    }
}
