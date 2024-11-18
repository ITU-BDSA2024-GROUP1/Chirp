using Chirp.Core.DataTransferObject;
using Chirp.Core.Models;
using Chirp.Infrastructure.Repositories;

namespace Chirp.InfrastructureTest.ServiceTest;

public abstract class InfrastructureServiceTester
{
    private readonly SqliteConnection _connection;
    private protected readonly ChirpDBContext _context;
    
    private protected readonly IAuthorRepository _authorRepository;
    private protected readonly AuthorDTO[] _knownAuthors;
    
    private protected InfrastructureServiceTester()
    {
        _connection = new("Data Source=:memory:");
        _connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(_connection);

        _context = new(builder.Options);
        _context.Database.EnsureCreatedAsync();
        
        _authorRepository = new AuthorRepository(_context);
        _knownAuthors = SetUpTestAuthorDB(_authorRepository).Result;
    }
    
    private protected async Task ClearDB(string tableName)
    {
        await using (var command = new SqliteCommand($"DELETE FROM {tableName};", _connection))
        {
            command.ExecuteNonQuery();
        }
    }
    
    private async Task<AuthorDTO[]> SetUpTestAuthorDB(IAuthorRepository authorRepository)
    {
        await ClearDB("AspNetUsers");

        AuthorDTO[] authors = new AuthorDTO[4];
        for (int i = 0; i < authors.Length; i++)
        {
            int id = i + 1;
            authors[i] = new()
            {
                Id = id.ToString(),
                Name = $"Test{id}",
                Email = $"Test{id}@Tester.com"
            };
            authors[i].Id = await authorRepository.AddAuthorAsync(authors[i]);
        }

        List<AuthorDTO> repoAuthors = [..await authorRepository.GetAllAuthorsAsync()];
        for (int i = 0; i < authors.Length; i++) authors[i].Id = repoAuthors[i].Id;

        return authors;
    }
    
    private protected async Task<CheepDTO[]> SetUpTestCheepDB(ICheepRepository cheepRepository, AuthorDTO[] authors, int n = 160)
    {
        await ClearDB("Cheeps");

        DateTime timeStamp = DateTime.Now;
        CheepDTO[] cheeps = new CheepDTO[n];
        for (int i = 0; i < cheeps.Length; i++)
        {
            timeStamp = timeStamp.AddTicks(10000000);
            int id = i + 1;
            int authorIndex = i % authors.Length;
            cheeps[i] = new()
            {
                Id = id,
                Name = authors[authorIndex].Name,
                Message = $"Text{id}",
                TimeStamp = timeStamp.ToString(@"yyyy\-MM\-dd HH\:mm\:ss"),
                AuthorId = authors[authorIndex].Id,
                AuthorEmail = authors[authorIndex].Email
            };
            cheeps[i].Id = await cheepRepository.AddCheepAsync(cheeps[i]);
        }

        Array.Reverse(cheeps);

        PagedResult<CheepDTO> pagedCheeps = await cheepRepository.GetAllCheepsAsync(1, cheeps.Length);
        List<CheepDTO> repoCheeps = pagedCheeps.Items.ToList();
        for (int i = 0; i < cheeps.Length; i++) cheeps[i].Id = repoCheeps[i].Id;

        return cheeps;

    }
}