using Chirp.Core;

using SimpleDB;

namespace Chirp.CSVDBService;

public class Program
{
    public static void Main(string[] args)
    {
        DirectoryFixer.SetWorkingDirectoryToProjectRoot();
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        var communicator = new DBCommunicator(CSVDatabase<Cheep>.Instance);
        app.MapGet("/cheeps", communicator.ReadCheeps);
        app.MapPost("/cheep", communicator.WriteCheep);

        app.Run("http://localhost:5127");
    }
}