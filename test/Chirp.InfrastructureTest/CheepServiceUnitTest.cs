using Chirp.Core.DataTransferObject;
using Chirp.Core.Entities;
using Chirp.Core.Models;
using Chirp.Core.Repositories;

namespace Chirp.InfrastructureTest;

public class CheepServiceUnitTest
{
    
    [Fact]
    public void MakeCheepViewModel()
    {
        // Act
        CheepViewModel cvm = new CheepViewModel("te", "st", "0");

        // Assert
        Assert.NotNull(cvm);
    }


    [Fact]
    public async Task MakeCheepService()
    {
        // Arrange
        using var connection = new SqliteConnection("Data Source=memory");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();

        ICheepRepository cheepRepository = new CheepRepository(context);
        IAuthorRepository authorRepository = new AuthorRepository(context);
        AuthorDTO[] knownAuthors = await SetUpTestAuthorDB(authorRepository, connection);
        CheepDTO[] knownCheeps = await SetUpTestCheepDB(cheepRepository, connection, knownAuthors);

        // Act
        CheepService cheepService = new CheepService(cheepRepository, authorRepository);

        // Assert
        Assert.NotNull(cheepService);

    }

    [Fact]
    public async Task Read()
    {
        // Arrange
        using var connection = new SqliteConnection("Data Source=memory");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();

        ICheepRepository cheepRepository = new CheepRepository(context);
        IAuthorRepository authorRepository = new AuthorRepository(context);
        AuthorDTO[] knownAuthors = await SetUpTestAuthorDB(authorRepository, connection);
        CheepDTO[] knownCheeps = await SetUpTestCheepDB(cheepRepository, connection, knownAuthors);
        
        CheepService cheepService = new CheepService(cheepRepository, authorRepository);

        // Act
        PagedResult<CheepViewModel> cheeps = await cheepService.GetCheeps(knownCheeps.Length/32, 32);

        CheepViewModel
            expected = CheepService.CheepDTOToCheepViewModel(knownCheeps[knownCheeps.Length - 1]),
            actual = cheeps.Items[cheeps.Items.Count() - 1];

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task ReadAuthor()
    {
        // Arrange
        using var connection = new SqliteConnection("Data Source=memory");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();

        ICheepRepository cheepRepository = new CheepRepository(context);
        IAuthorRepository authorRepository = new AuthorRepository(context);
        AuthorDTO[] knownAuthors = await SetUpTestAuthorDB(authorRepository, connection);
        CheepDTO[] knownCheeps = await SetUpTestCheepDB(cheepRepository, connection, knownAuthors);

        CheepService cheepService = new CheepService(cheepRepository, authorRepository);

        // Act
        int pageNumber = knownCheeps.Length;
        pageNumber /= knownAuthors.Length;
        pageNumber /= 32;
        pageNumber += (knownCheeps.Length / knownAuthors.Length) % 32 == 0 ? 0 : 1;
        PagedResult<CheepViewModel> cheeps = await cheepService.GetCheepsFromAuthor(knownAuthors[1].Name, pageNumber, 32);

        CheepViewModel 
            expected = CheepService.CheepDTOToCheepViewModel(knownCheeps[knownCheeps.Length - 2]),
            actual = cheeps.Items[cheeps.Items.Count() - 1];

        // Assert
        Assert.Equal(expected, actual);
    }

    public static async Task<AuthorDTO[]> SetUpTestAuthorDB(IAuthorRepository authorRepository, SqliteConnection connection)
    {
        using (var command = new SqliteCommand("DELETE FROM authors;", connection))
        {
            command.ExecuteNonQuery();
        }

        AuthorDTO[] authors = new AuthorDTO[4];
        for (int i = 0; i < authors.Length; i++)
        {
            authors[i] = new AuthorDTO
            {
                Id = i+1,
                Name = $"Test{i+1}",
                Email = $"Test{i+1}@Tester.com"
            };
            authors[i].Id = await authorRepository.AddAuthorAsync(authors[i]);
        }

        List<AuthorDTO> repoAuthors = new List<AuthorDTO> (await authorRepository.GetAllAuthorsAsync());
        for (int i = 0; i < authors.Length; i++) authors[i].Id = repoAuthors[i].Id;

        return authors;

    }
    public static async Task<CheepDTO[]> SetUpTestCheepDB(ICheepRepository cheepRepository, SqliteConnection connection, AuthorDTO[] authors)
    {
        using (var command = new SqliteCommand("DELETE FROM cheeps;", connection))
        {
            command.ExecuteNonQuery();
        }

        DateTime timeStamp = DateTime.Now;
        long timeStampLong = timeStamp.Ticks;
        CheepDTO[] cheeps = new CheepDTO[160];
        for (int i = 0; i < cheeps.Length; i++)
        {
            timeStampLong += 10000000;
            timeStamp = new DateTime(timeStampLong);
            cheeps[i] = new CheepDTO
            {
                Id = i + 1,
                Name = authors[i % authors.Length].Name,
                Message = $"Text{i + 1}",
                TimeStamp = timeStamp.ToString("yyyy\\-MM\\-dd HH\\:mm\\:ss"),
                AuthorId = authors[i % authors.Length].Id,
                AuthorEmail = authors[i % authors.Length].Email
            };
            cheeps[i].Id = await cheepRepository.AddCheepAsync(cheeps[i]);
        }

        PagedResult<CheepDTO> pagedCheeps = await cheepRepository.GetAllCheepsAsync(1, cheeps.Length);
        List<CheepDTO> repoCheeps = pagedCheeps.Items.ToList();
        for (int i = 0; i < cheeps.Length; i++) cheeps[i].Id = repoCheeps[i].Id;

        Array.Reverse(cheeps);
        return cheeps;

    }
}