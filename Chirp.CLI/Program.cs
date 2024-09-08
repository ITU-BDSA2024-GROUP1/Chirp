using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.CommandLine;
using SimpleDB;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chirp.CLI
{
    internal class Program
    {
        const string cheepCsvPath = "data/chirp_cli_db.csv";
        static async Task<int> Main(string[] args)
        {
            SetWorkingDirectoryToProjectRoot();
            UserInterface.setCheepsCsvPath(cheepCsvPath);
            var rootCommand = new RootCommand("Chirp where you can send cheeps and read others");

            var readCommand = new Command("read", "Read informantion stored in database");
            readCommand.SetHandler(async () =>
            {
                await UserInterface.ReadCheeps();
            });

            var storeCommand = new Command("cheep", "Add a cheep to the databas");
            var cheepArgument = new Argument<string>("message", "The cheep you want to send");
            storeCommand.AddArgument(cheepArgument);
            storeCommand.SetHandler(async (string message) =>
             {
                 await Cheep(message);   
             }, cheepArgument);

            rootCommand.AddCommand(readCommand);
            rootCommand.AddCommand(storeCommand);

            return await rootCommand.InvokeAsync(args);
        }

        // With the help of ChatGPT since I could not find a solution through searching google.
        static void SetWorkingDirectoryToProjectRoot()
        {
            // Locate the project directory by navigating up from the binary directory
            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;

            // Set the working directory to the project directory
            Directory.SetCurrentDirectory(projectDirectory);

            Console.WriteLine("Current Working Directory Set To: " + Directory.GetCurrentDirectory());
        }

        static long ParseDateTimeToUnixTime(DateTime date)
        {
            return ((DateTimeOffset)date).ToUnixTimeSeconds();
        }
        
        static async Task Cheep(string content)
        {
            string csvLine = $"\n{Environment.UserName},\"{content}\",{ParseDateTimeToUnixTime(DateTime.UtcNow)}";
            await File.AppendAllTextAsync(cheepCsvPath, csvLine);
        }
    }
}