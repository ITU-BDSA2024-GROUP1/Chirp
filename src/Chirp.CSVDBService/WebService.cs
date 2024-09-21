using Chirp.Core;

using SimpleDB;

namespace Chirp.CSVDBService;

public class WebService : IAsyncDisposable
{
    private readonly WebApplication _app;

    public WebService(IDatabaseRepository<Cheep> repository)
    {
        var builder = WebApplication.CreateBuilder();
        _app = builder.Build();
        
        _app.MapGet("/cheeps", repository.Read);
        _app.MapPost("/cheep", repository.Store);
    }

    public static void Main(string[] args)
    {
        DirectoryFixer.SetWorkingDirectoryToProjectRoot();
        var webService = new WebService(new CSVDatabase<Cheep>(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "chirp_cli_db.csv")));
        webService.Run();
    }

    public void Run() => _app.Run($"https://bdsagroup1chirpremotedb.azurewebsites.net");

    public ValueTask DisposeAsync() => _app.DisposeAsync();
}