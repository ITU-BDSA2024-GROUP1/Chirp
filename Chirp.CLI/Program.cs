using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.CommandLine;
using SimpleDB;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CsvHelper;

namespace Chirp.CLI
{
    internal class Program
    {
        const string cheepCsvPath = "Chirp.CLI/data/chirp_cli_db.csv";
        static async Task<int> Main(string[] args)
        {
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
                 await UserInterface.WriteCheep(message);   
             }, cheepArgument);

            rootCommand.AddCommand(readCommand);
            rootCommand.AddCommand(storeCommand);

            return await rootCommand.InvokeAsync(args);
        }
    }
}