using Chirp.Core.Data;
using Chirp.Core.DataTransferObject;
using Chirp.Core.Repositories;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.CoreTest;

public abstract class CoreRepositoryTester
{
    private readonly SqliteConnection _connection;
    private protected readonly ChirpDBContext _context;

    private protected CoreRepositoryTester()
    {
        _connection = new("Data Source=:memory:");
        _connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(_connection);

        _context = new(builder.Options);
        _context.Database.EnsureCreatedAsync();
    }

    private async Task ClearDB(string tableName)
    {
        await using (var command = new SqliteCommand($"DELETE FROM {tableName};", _connection))
        {
            command.ExecuteNonQuery();
        }
    }
    
    private protected async Task<AuthorRepository> SetUpAuthorRepository()
    {
        AuthorRepository authorRepository = new(_context);
        
        await ClearDB("AspNetUsers");
        Assert.Empty(_context.Authors);
        
        return authorRepository;
    }
    
    private protected async Task<CheepRepository> SetUpCheepRepository()
    {
        CheepRepository cheepRepository = new(_context);
        
        await ClearDB("Cheeps");
        Assert.Empty(_context.Cheeps);
        
        return cheepRepository;
    }
    
    private protected async Task<AuthorDTO[]> PopulateAuthorRepository(AuthorRepository authorRepository, int n = 4)
    {
        AuthorDTO[] authors = new AuthorDTO[n];
        for (int i = 0; i < authors.Length; i++)
        {
            int id = i + 1;
            authors[i] = new()
            {
                Id = id.ToString(),
                Name = $"Test Testerson {id}",
                Email = $"Test.Testerson{id}@Tester.com"
            };
            authors[i].Id = await authorRepository.AddAuthorAsync(authors[i]);
        }
        
        Assert.NotEmpty(_context.Authors);
        return authors;
    }

    private protected async Task<CheepDTO[]> PopulateCheepRepository(CheepRepository cheepRepository, AuthorDTO[] authors, int n = 160)
    {
        DateTime timeStamp = DateTime.Now;
        CheepDTO[] cheeps = new CheepDTO[n];
        for (int i = 0; i < cheeps.Length; i++)
        {
            timeStamp = timeStamp.AddTicks(10000000);
            int authorIndex = i % authors.Length;
            cheeps[i] = new()
            {
                Id = -1,
                Name = authors[authorIndex].Name,
                Message = $"Test Message {i + 1}",
                TimeStamp = timeStamp.ToString(@"yyyy\-MM\-dd HH\:mm\:ss"),
                AuthorId = authors[authorIndex].Id,
                AuthorEmail = authors[authorIndex].Email
            };
            cheeps[i].Id = await cheepRepository.AddCheepAsync(cheeps[i]);
        }
        
        Array.Reverse(cheeps);
        
        Assert.NotEmpty(_context.Cheeps);
        return cheeps;
    }
}