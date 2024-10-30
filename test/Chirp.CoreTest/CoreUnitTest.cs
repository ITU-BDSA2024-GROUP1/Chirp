using Chirp.Core.Data;
using Chirp.Core.DataTransferObject;
using Chirp.Core.Models;
using Chirp.Core.Repositories;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.CoreTest;

public class CoreUnitTest
{
    private readonly SqliteConnection _connection;
    private readonly ChirpDBContext _context;

    public CoreUnitTest()
    {
        _connection = new("Data Source=memory");
        _connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(_connection);

        _context = new(builder.Options);
        _context.Database.EnsureCreatedAsync();
    }

    // Setup
    
    async Task ClearDB(string tableName)
    {
        await using (var command = new SqliteCommand($"DELETE FROM {tableName};", _connection))
        {
            command.ExecuteNonQuery();
        }
    }
    
    async Task<AuthorRepository> SetUpAuthorRepository()
    {
        AuthorRepository authorRepository = new(_context);
        
        await ClearDB("authors");
        Assert.Empty(_context.Authors);
        
        return authorRepository;
    }
    
    async Task<CheepRepository> SetUpCheepRepository()
    {
        CheepRepository cheepRepository = new(_context);
        
        await ClearDB("cheeps");
        Assert.Empty(_context.Cheeps);
        
        return cheepRepository;
    }
    
    async Task<AuthorDTO[]> PopulateAuthorRepository(AuthorRepository authorRepository, int n = 4)
    {
        AuthorDTO[] authors = new AuthorDTO[n];
        for (int i = 0; i < authors.Length; i++)
        {
            int id = i + 1;
            authors[i] = new()
            {
                Id = id,
                Name = $"Test Testerson {id}",
                Email = $"Test.Testerson{id}@Tester.com"
            };
            authors[i].Id = await authorRepository.AddAuthorAsync(authors[i]);
        }
        
        Assert.NotEmpty(_context.Authors);

        return authors;
    }

    async Task<CheepDTO[]> PopulateCheepRepository(CheepRepository cheepRepository, AuthorDTO[] authors, int n = 160)
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

        Assert.NotEmpty(_context.Cheeps);
        
        Array.Reverse(cheeps);
        return cheeps;
    }
    
    // Author Tests
    
    [Fact]
    public async Task AddAuthor()
    {
        // Arrange
        AuthorRepository authorRepository = await SetUpAuthorRepository();
        
        // Act
        AuthorDTO testAuthor = new()
        {
            Id = 1,
            Name = "Test Testerson",
            Email = "Test.Testerson@Tester.com"
        };
        await authorRepository.AddAuthorAsync(testAuthor);
        
        // Assert
        Assert.NotEmpty(_context.Authors);
    }

    [Fact]
    public async Task GetAllAuthors()
    {
        // Arrange
        AuthorRepository authorRepository = await SetUpAuthorRepository();
        IEnumerable<AuthorDTO> testAuthors = await PopulateAuthorRepository(authorRepository, 10);
        
        // Act
        IEnumerable<AuthorDTO> retrievedAuthors = await authorRepository.GetAllAuthorsAsync();
        
        // Assert
        foreach (var retrievedAuthor in retrievedAuthors)
        {
            bool contained = false;
            foreach (var testAuthor in testAuthors)
            {
                if (!CompareAuthors(retrievedAuthor, testAuthor)) continue; // Because the Assert.Contains() method didn't want to work

                contained = true;
                break;
            }
            
            Assert.True(contained);
        }
    }

    [Fact]
    public async Task GetAuthorById()
    {
        // Arrange
        AuthorRepository authorRepository = await SetUpAuthorRepository();
        
        // Act
        AuthorDTO specificAuthor = new()
        {
            Id = -1,
            Name = "Specific Testerson",
            Email = "Specific.Testerson@Tester.com",
        };
        specificAuthor.Id = await authorRepository.AddAuthorAsync(specificAuthor);
        AuthorDTO retrievedAuthor = await authorRepository.GetAuthorByIdAsync(specificAuthor.Id);
        
        // Assert
        Assert.True(CompareAuthors(specificAuthor, retrievedAuthor));
    }
    
    [Fact]
    public async Task GetAuthorByName()
    {
        // Arrange
        AuthorRepository authorRepository = await SetUpAuthorRepository();
        
        // Act
        AuthorDTO specificAuthor = new()
        {
            Id = -1,
            Name = "Specific Testerson",
            Email = "Specific.Testerson@Tester.com",
        };
        specificAuthor.Id = await authorRepository.AddAuthorAsync(specificAuthor);
        AuthorDTO retrievedAuthor = await authorRepository.GetAuthorByNameAsync(specificAuthor.Name);
        
        // Assert
        Assert.True(CompareAuthors(specificAuthor, retrievedAuthor));
    }
    
    [Fact]
    public async Task GetAuthorByEmail()
    {
        // Arrange
        AuthorRepository authorRepository = await SetUpAuthorRepository();
        
        // Act
        AuthorDTO specificAuthor = new()
        {
            Id = -1,
            Name = "Specific Testerson",
            Email = "Specific.Testerson@Tester.com",
        };
        specificAuthor.Id = await authorRepository.AddAuthorAsync(specificAuthor);
        AuthorDTO retrievedAuthor = await authorRepository.GetAuthorByEmailAsync(specificAuthor.Email);
        
        // Assert
        Assert.True(CompareAuthors(specificAuthor, retrievedAuthor));
    }

    [Fact]
    public async Task UpdateAuthor()
    {
        // Arrange
        AuthorRepository authorRepository = await SetUpAuthorRepository();
        AuthorDTO testerson = new() { Id = -1, Name = "Test Testerson", Email = "Test.Testerson@Tester.com" };
        testerson.Id = await authorRepository.AddAuthorAsync(testerson);
        
        // Act
        AuthorDTO newTesterson = new()
        {
            Id = testerson.Id,
            Name = "NewTest Testerson",
            Email = "NewTest.Testerson@Tester.com"
        };
        await authorRepository.UpdateAuthorAsync(newTesterson);
        
        // Assert
        AuthorDTO retrievedAuthor = await authorRepository.GetAuthorByIdAsync(testerson.Id);
        Assert.False(CompareAuthors(retrievedAuthor, testerson));
        Assert.True(CompareAuthors(retrievedAuthor, newTesterson));
    }

    [Fact]
    public async Task DeleteAuthor()
    {
        // Arrange
        AuthorRepository authorRepository = await SetUpAuthorRepository();
        AuthorDTO testerson = new() { Id = -1, Name = "Test Testerson", Email = "Test.Testerson@Tester.com" };
        testerson.Id = await authorRepository.AddAuthorAsync(testerson);
        
        // Act
        Assert.NotNull(await authorRepository.GetAuthorByIdAsync(testerson.Id));
        await authorRepository.DeleteAuthorAsync(testerson.Id);
        
        // Assert
        Assert.Null(await authorRepository.GetAuthorByIdAsync(testerson.Id));
    }
    
    static bool CompareAuthors(AuthorDTO a, AuthorDTO b)
    {
        return a.Id == b.Id && a.Name == b.Name && a.Email == b.Email;
    }
    
    // Cheep Tests

    [Fact]
    public async Task AddCheep()
    {
        // Arrange
        AuthorRepository authorRepository = await SetUpAuthorRepository();
        IEnumerable<AuthorDTO> testAuthors = await PopulateAuthorRepository(authorRepository);
        CheepRepository cheepRepository = await SetUpCheepRepository();
        
        // Act
        CheepDTO testCheep = new()
        {
            Id = -1,
            Name = "Cheep Testerson",
            Message = "Test Cheep",
            TimeStamp = DateTime.Now.ToString(@"yyyy\-MM\-dd HH\:mm\:ss"),
            AuthorId = testAuthors.First().Id,
            AuthorEmail = testAuthors.First().Email
        };
        await cheepRepository.AddCheepAsync(testCheep);
        
        // Assert
        Assert.NotEmpty(_context.Cheeps);
    }

    [Fact]
    public async Task GetAllCheeps()
    {
        // Arrange
        AuthorRepository authorRepository = await SetUpAuthorRepository();
        AuthorDTO[] testAuthors = await PopulateAuthorRepository(authorRepository);
        CheepRepository cheepRepository = await SetUpCheepRepository();
        CheepDTO[] testCheeps = await PopulateCheepRepository(cheepRepository, testAuthors, 32 * 5);
        
        // Act
        PagedResult<CheepDTO> retrievedCheeps = await cheepRepository.GetAllCheepsAsync(testCheeps.Length / 32, 32);
        
        // Assert
        Assert.True(CompareCheep(testCheeps[^1], retrievedCheeps.Items[^1]));
        Assert.Equal(5, retrievedCheeps.TotalPages);
    }

    [Fact]
    public async Task GetCheepsByAuthor()
    {
        // Arrange
        AuthorRepository authorRepository = await SetUpAuthorRepository();
        AuthorDTO[] testAuthors = await PopulateAuthorRepository(authorRepository);
        CheepRepository cheepRepository = await SetUpCheepRepository();
        await PopulateCheepRepository(cheepRepository, testAuthors);
        
        // Act
        AuthorDTO testAuthor = testAuthors.First();
        PagedResult<CheepDTO> authorCheeps = await cheepRepository.GetCheepsByAuthorNameAsync(testAuthor.Name, 1, 32);
        
        // Assert
        foreach (var cheep in authorCheeps.Items)
        {
            Assert.Equal(testAuthor.Name, cheep.Name);
        }
    }
    
    [Fact]
    public async Task GetCheepById()
    {
        // Arrange
        AuthorRepository authorRepository = await SetUpAuthorRepository();
        AuthorDTO[] testAuthors = await PopulateAuthorRepository(authorRepository);
        CheepRepository cheepRepository = await SetUpCheepRepository();
        CheepDTO[] testCheeps = await PopulateCheepRepository(cheepRepository, testAuthors);
        
        // Act
        CheepDTO testCheep = testCheeps.First();
        CheepDTO retrievedCheep = await cheepRepository.GetCheepByIdAsync(testCheep.Id);
        
        // Assert
        Assert.True(CompareCheep(testCheep, retrievedCheep));
    }
    
    [Fact]
    public async Task UpdateCheep()
    {
        // Arrange
        AuthorRepository authorRepository = await SetUpAuthorRepository();
        AuthorDTO[] testAuthors = await PopulateAuthorRepository(authorRepository);
        CheepRepository cheepRepository = await SetUpCheepRepository();
        CheepDTO[] testCheeps = await PopulateCheepRepository(cheepRepository, testAuthors);
        
        // Act
        CheepDTO testCheep = testCheeps.First();
        Assert.NotEqual("New Test Message", testCheep.Message);
        testCheep.Message = "New Test Message";
        
        await cheepRepository.UpdateCheepAsync(testCheep);
        
        // Assert
        CheepDTO updatedCheep = await cheepRepository.GetCheepByIdAsync(testCheep.Id);
        Assert.Equal("New Test Message", updatedCheep.Message);
    }

    [Fact]
    public async Task DeleteCheep()
    {
        // Arrange
        AuthorRepository authorRepository = await SetUpAuthorRepository();
        AuthorDTO[] testAuthors = await PopulateAuthorRepository(authorRepository);
        CheepRepository cheepRepository = await SetUpCheepRepository();
        CheepDTO[] testCheeps = await PopulateCheepRepository(cheepRepository, testAuthors);
        CheepDTO testCheep = testCheeps.First();
        
        // Act
        Assert.NotNull(await cheepRepository.GetCheepByIdAsync(testCheep.Id));
        Assert.NotNull(await cheepRepository.DeleteCheepAsync(testCheep.Id));
        
        // Assert
        Assert.Null(await cheepRepository.GetCheepByIdAsync(testCheep.Id));
    }

    static bool CompareCheep(CheepDTO a, CheepDTO b)
    {
        return a.Id == b.Id && a.Name == b.Name;
    }
}
