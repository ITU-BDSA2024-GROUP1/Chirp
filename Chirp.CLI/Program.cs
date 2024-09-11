using System.CommandLine;

namespace Chirp.CLI
{
    internal class Program
    {
        const string CheepCsvPath = "data/chirp_cli_db.csv";
        static async Task<int> Main(string[] args)
        {
            UserInterface.SetCheepsCsvPath(CheepCsvPath);
            var rootCommand = new RootCommand("Chirp where you can send cheeps and read others");

            var readCommand = new Command("read", "Read informantion stored in database");
            var readArgument = new Argument<int?>("value", "The amount of latest cheeps you want to read");
            readCommand.AddArgument(readArgument);
            readCommand.SetHandler(UserInterface.ReadCheeps, readArgument);

            var storeCommand = new Command("cheep", "Add a cheep to the database");
            var cheepArgument = new Argument<string>("message", "The cheep you want to send");
            storeCommand.AddArgument(cheepArgument);
            storeCommand.SetHandler(UserInterface.WriteCheep, cheepArgument);

            rootCommand.AddCommand(readCommand);
            rootCommand.AddCommand(storeCommand);

            return await rootCommand.InvokeAsync(args);
        }
    }
}