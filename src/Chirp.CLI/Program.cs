using System.CommandLine;

using Chirp.Core;

using SimpleDB;

namespace Chirp.CLI;

internal class Program
{
    static async Task<int> Main(string[] args)
    {
        DirectoryFixer.SetWorkingDirectoryToProjectRoot();
        var rootCommand = new RootCommand("Chirp where you can send cheeps and read others");
        
        UserInterface userInterface = new(new WebDB<Cheep>("http://localhost:5000"));
        
        var readCommand = new Command("read", "Read information stored in database");
        var readArgument = new Argument<int?>("value", "The amount of latest cheeps you want to read");
        readCommand.AddArgument(readArgument);
        readCommand.SetHandler(userInterface.ReadCheeps, readArgument);

        var storeCommand = new Command("cheep", "Add a cheep to the database");
        var cheepArgument = new Argument<string>("message", "The cheep you want to send");
        storeCommand.AddArgument(cheepArgument);
        storeCommand.SetHandler(userInterface.WriteCheep, cheepArgument);

        rootCommand.AddCommand(readCommand);
        rootCommand.AddCommand(storeCommand);

        return await rootCommand.InvokeAsync(args);
    }
}