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

        IDatabaseRepository<Cheep> instance = CSVDatabase<Cheep>.Instance;
        app.MapGet("/cheeps", instance.Read);
        app.MapPost("/cheep", instance.Store);

        app.Run("http://localhost:5127");
    }
}