using Chirp.Core;

using SimpleDB;

namespace Chirp.CSVDBService;

public class WebService : IAsyncDisposable
{
    private readonly WebApplication _app;

    public WebService(IDatabaseRepository<Cheep> repository)
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        _app = builder.Build();

        if (_app.Environment.IsDevelopment())
        {
            _app.UseSwagger();
            _app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API V1");
                if (_app.Environment.IsDevelopment())
                    options.RoutePrefix = "swagger";
                else
                    options.RoutePrefix = string.Empty;
            }
            );
        }

        _app.UseHttpsRedirection();
        _app.UseAuthorization();
        _app.MapControllers();

        _app.MapGet("/cheeps", repository.Read);
        _app.MapPost("/cheep", repository.Store);
    }

    public static void Main(string[] args)
    {
        DirectoryFixer.SetWorkingDirectoryToProjectRoot();
        var webService = new WebService(new CSVDatabase<Cheep>(Path.Combine(Directory.GetCurrentDirectory(), "chirp_cli_db.csv")));
        //var webService = new WebService(new CSVDatabase<Cheep>("data/chirp_cli_db.csv"));
        webService.Run();
    }

    public void Run()
    {
        //var port = Environment.GetEnvironmentVariable("WEBSITES_PORT") ?? "80"; // Default to 80 if not set
        _app.Run("http://*:8080");
    }

    public ValueTask DisposeAsync() => _app.DisposeAsync();
}
