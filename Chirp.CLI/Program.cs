using System.CommandLine;

namespace Chirp.CLI
{
    internal class Program
    {
        const string CheepCsvPath = "Chirp.CLI/data/chirp_cli_db.csv";
        static async Task<int> Main(string[] args)
        {
            UserInterface.setCheepsCsvPath(CheepCsvPath);
            var rootCommand = new RootCommand("Chirp where you can send cheeps and read others");

            var readCommand = new Command("read", "Read information stored in database");
            readCommand.SetHandler(async () =>
            {
                await UserInterface.ReadCheeps();
            });

            var storeCommand = new Command("cheep", "Add a cheep to the database");
            var cheepArgument = new Argument<string>("message", "The cheep you want to send");
            storeCommand.AddArgument(cheepArgument);
            storeCommand.SetHandler(async message =>
             {
                 await UserInterface.WriteCheep(message);   
             }, cheepArgument);

            rootCommand.AddCommand(readCommand);
            rootCommand.AddCommand(storeCommand);

            return await rootCommand.InvokeAsync(args);
        }
    }
}