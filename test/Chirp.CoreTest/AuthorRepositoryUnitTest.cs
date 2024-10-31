using Chirp.Core.DataTransferObject;
using Chirp.Core.Repositories;

namespace Chirp.CoreTest;

public class AuthorRepositoryUnitTest : CoreRepositoryTester
{
    private readonly AuthorRepository _authorRepository;

    public AuthorRepositoryUnitTest() => _authorRepository = SetUpAuthorRepository().Result;

    [Fact]
    public async Task AddAuthor()
    {
        // Act
        AuthorDTO testAuthor = new()
        {
            Id = 1,
            Name = "Test Testerson",
            Email = "Test.Testerson@Tester.com"
        };
        await _authorRepository.AddAuthorAsync(testAuthor);
        
        // Assert
        Assert.NotEmpty(_context.Authors);
    }

    [Fact]
    public async Task GetAllAuthors()
    {
        // Arrange
        IEnumerable<AuthorDTO> testAuthors = await PopulateAuthorRepository(_authorRepository, 10);
        
        // Act
        IEnumerable<AuthorDTO> retrievedAuthors = await _authorRepository.GetAllAuthorsAsync();
        
        // Assert
        Assert.Equal(retrievedAuthors, testAuthors);
    }

    [Fact]
    public async Task GetAuthorById()
    {
        // Act
        AuthorDTO specificAuthor = new()
        {
            Id = -1,
            Name = "Specific Testerson",
            Email = "Specific.Testerson@Tester.com",
        };
        specificAuthor.Id = await _authorRepository.AddAuthorAsync(specificAuthor);
        AuthorDTO retrievedAuthor = await _authorRepository.GetAuthorByIdAsync(specificAuthor.Id);
        
        // Assert
        Assert.Equal(retrievedAuthor, specificAuthor);
    }
    
    [Fact]
    public async Task GetAuthorByName()
    {
        // Act
        AuthorDTO specificAuthor = new()
        {
            Id = -1,
            Name = "Specific Testerson",
            Email = "Specific.Testerson@Tester.com",
        };
        specificAuthor.Id = await _authorRepository.AddAuthorAsync(specificAuthor);
        AuthorDTO retrievedAuthor = await _authorRepository.GetAuthorByNameAsync(specificAuthor.Name);
        
        // Assert
        Assert.Equal(specificAuthor, retrievedAuthor);
    }
    
    [Fact]
    public async Task GetAuthorByEmail()
    {
        // Act
        AuthorDTO specificAuthor = new()
        {
            Id = -1,
            Name = "Specific Testerson",
            Email = "Specific.Testerson@Tester.com",
        };
        specificAuthor.Id = await _authorRepository.AddAuthorAsync(specificAuthor);
        AuthorDTO retrievedAuthor = await _authorRepository.GetAuthorByEmailAsync(specificAuthor.Email);
        
        // Assert
        Assert.Equal(specificAuthor, retrievedAuthor);
    }

    [Fact]
    public async Task UpdateAuthor()
    {
        // Arrange
        AuthorDTO testerson = new() { Id = -1, Name = "Test Testerson", Email = "Test.Testerson@Tester.com" };
        testerson.Id = await _authorRepository.AddAuthorAsync(testerson);
        
        // Act
        AuthorDTO newTesterson = new()
        {
            Id = testerson.Id,
            Name = "NewTest Testerson",
            Email = "NewTest.Testerson@Tester.com"
        };
        await _authorRepository.UpdateAuthorAsync(newTesterson);
        
        // Assert
        AuthorDTO retrievedAuthor = await _authorRepository.GetAuthorByIdAsync(testerson.Id);
        Assert.NotEqual(retrievedAuthor, testerson);
        Assert.Equal(retrievedAuthor, newTesterson);
    }

    [Fact]
    public async Task DeleteAuthor()
    {
        // Arrange
        AuthorDTO testerson = new() { Id = -1, Name = "Test Testerson", Email = "Test.Testerson@Tester.com" };
        testerson.Id = await _authorRepository.AddAuthorAsync(testerson);
        
        // Act
        Assert.NotNull(await _authorRepository.GetAuthorByIdAsync(testerson.Id));
        await _authorRepository.DeleteAuthorAsync(testerson.Id);
        
        // Assert
        Assert.Null(await _authorRepository.GetAuthorByIdAsync(testerson.Id));
    }
}