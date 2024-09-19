using Chirp.Core;

using SimpleDB;

namespace Chirp.CSVDBService;

public class WebService
{
    public static void Main(string[] args)
    {
        DirectoryFixer.SetWorkingDirectoryToProjectRoot();
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        var dbInstance = CSVDatabase<Cheep>.Instance;
        app.MapGet("/cheeps", dbInstance.Read);
        app.MapPost("/cheep", dbInstance.Store);
        
        app.MapGet("/test/cheeps", (int? limit) =>
        {
            dbInstance.InTestingDatabase = true;
            var response = dbInstance.Read(limit);
            dbInstance.InTestingDatabase = false;
            return response;
        });
        app.MapPost("/test/cheep", (Cheep record) =>
        {
            dbInstance.InTestingDatabase = true;
            dbInstance.Store(record);
            dbInstance.InTestingDatabase = false;
        });

        app.Run("http://localhost:5127");
    }
}