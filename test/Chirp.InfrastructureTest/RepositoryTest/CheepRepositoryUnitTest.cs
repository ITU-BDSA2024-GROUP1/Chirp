using System.ComponentModel.DataAnnotations;

using Chirp.Core.DataTransferObject;
using Chirp.Infrastructure.Models;
using Chirp.Infrastructure.Repositories;

namespace Chirp.InfrastructureTest.RepositoryTest;

public class CheepRepositoryUnitTest : CoreRepositoryTester
{
    private readonly AuthorDTO[] _testAuthors;
    private readonly AuthorDTO _firstAuthor;
    private readonly CheepRepository _cheepRepository;

    public CheepRepositoryUnitTest()
    {
        AuthorRepository authorRepository = SetUpAuthorRepository().Result;
        _testAuthors = PopulateAuthorRepository(authorRepository).Result;
        _firstAuthor = _testAuthors.First();
        _cheepRepository = SetUpCheepRepository().Result;
    }

    private CheepDTO[] PopulateCheepRepository() => PopulateCheepRepository(_cheepRepository, _testAuthors).Result;
    
    [Fact]
    public async Task AddCheep()
    {
        // Act
        CheepDTO testCheep = new()
        {
            Id = -1,
            Name = "Cheep Testerson",
            Message = "Test Cheep",
            TimeStamp = DateTime.Now.ToString(@"yyyy\-MM\-dd HH\:mm\:ss"),
            AuthorId = _firstAuthor.Id,
            AuthorEmail = _firstAuthor.Email
        };
        await _cheepRepository.AddCheepAsync(testCheep);
        
        // Assert
        Assert.NotEmpty(_context.Cheeps);
    }

    [Fact]
    public async Task AddCheepExceedingLimit()
    {
        // Arrange
        CheepDTO testCheep = new()
        {
            Id = -1,
            Name = "Cheep Testerson",
            Message = new('e', 161),  // Exceeding the limit
            TimeStamp = DateTime.Now.ToString(@"yyyy\-MM\-dd HH\:mm\:ss"),
            AuthorId = _firstAuthor.Id,
            AuthorEmail = _firstAuthor.Email
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _cheepRepository.AddCheepAsync(testCheep)); 
        Assert.Contains("must be at most 160 characters long", exception.Message);
    }

    [Fact]
    public async Task GetAllCheeps()
    {
        // Arrange
        CheepDTO[] testCheeps = PopulateCheepRepository();
        
        // Act
        PagedResult<CheepDTO> retrievedCheeps = await _cheepRepository.GetAllCheepsAsync(testCheeps.Length / 32, 32);
        
        // Assert
        Assert.Equal(testCheeps[^1], retrievedCheeps.Items[^1]);
        Assert.Equal(5, retrievedCheeps.TotalPages);
    }

    [Fact]
    public async Task GetCheepsByAuthor()
    {
        // Arrange
        PopulateCheepRepository();
        
        // Act
        PagedResult<CheepDTO> authorCheeps = await _cheepRepository.GetCheepsByAuthorNameAsync(_firstAuthor.Name, 1, 32);
        
        // Assert
        foreach (var cheep in authorCheeps.Items)
        {
            Assert.Equal(cheep.Name, _firstAuthor.Name);
        }
    }
    
    [Fact]
    public async Task GetCheepById()
    {
        // Arrange
        CheepDTO[] testCheeps = PopulateCheepRepository();
        
        // Act
        CheepDTO testCheep = testCheeps.First();
        CheepDTO retrievedCheep = await _cheepRepository.GetCheepByIdAsync(testCheep.Id);
        
        // Assert
        Assert.Equal(testCheep, retrievedCheep);
    }
    
    [Fact]
    public async Task UpdateCheep()
    {
        // Arrange
        CheepDTO[] testCheeps = PopulateCheepRepository();
        
        // Act
        CheepDTO testCheep = testCheeps.First();
        string originalMessage=testCheep.Message;
        Assert.NotEqual("New Test Message", testCheep.Message);
        testCheep.Message = "New Test Message";
        
        await _cheepRepository.UpdateCheepAsync(testCheep,originalMessage);
        
        // Assert
        CheepDTO updatedCheep = await _cheepRepository.GetCheepByIdAsync(testCheep.Id);
        Assert.Equal("New Test Message", updatedCheep.Message);
    }

    [Fact]
    public async Task DeleteCheep()
    {
        // Arrange
        CheepDTO[] testCheeps = PopulateCheepRepository();
        CheepDTO testCheep = testCheeps.First();
        
        // Act
        Assert.NotNull(await _cheepRepository.GetCheepByIdAsync(testCheep.Id));
        Assert.NotNull(await _cheepRepository.DeleteCheepAsync(testCheep.Id));
        
        // Assert
        Assert.Null(await _cheepRepository.GetCheepByIdAsync(testCheep.Id));
    }

    [Fact]
    public async Task GetCheepCountByAuthor()
    {
        // Arrange
        CheepDTO[] testCheeps = PopulateCheepRepository();
        string authorName = _firstAuthor.Name;

        // Act
        int actualCount = await _cheepRepository.GetCheepCountByAuthor(authorName);

        // Assert
        int expectedCount = testCheeps.Count(c => c.Name == authorName);
        Assert.Equal(expectedCount, actualCount);
    }
}