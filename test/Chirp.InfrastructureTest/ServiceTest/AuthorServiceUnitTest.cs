using Chirp.Core.DataTransferObject;
using Chirp.Infrastructure.Services.AuthorService;

namespace Chirp.InfrastructureTest.ServiceTest;

public class AuthorServiceUnitTest : InfrastructureServiceTester
{
    private readonly AuthorService _authorService;
    
    public AuthorServiceUnitTest() => _authorService = new(_authorRepository);

    private static AuthorViewModel AuthorDTOToAuthorViewModel(AuthorDTO authorDTO)
    {
        return new(authorDTO.Id, authorDTO.Name, authorDTO.Email);
    }
    
    [Fact]
    public void MakeAuthorService()
    {
        // Assert
        Assert.NotNull(_authorService);
    }

    [Fact]
    public async Task GetAuthorByName()
    {
        // Act
        AuthorViewModel 
            expected = AuthorDTOToAuthorViewModel(_knownAuthors[0]), 
            actual = await _authorService.GetAuthorByName(_knownAuthors[0].Name);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetAuthorByEmail()
    {
        // Act
        AuthorViewModel 
            expected = AuthorDTOToAuthorViewModel(_knownAuthors[2]), 
            actual = await _authorService.GetAuthorByEmail(_knownAuthors[2].Email);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetAuthorById()
    {
        // Act
        AuthorViewModel
            expected = AuthorDTOToAuthorViewModel(_knownAuthors[1]),
            actual = await _authorService.GetAuthorById(_knownAuthors[1].Id);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task CreateAuthor()
    {
        // Act
        AuthorViewModel expected = new(_knownAuthors[^1].Id + 1, "Test5", "Test5@Tester.com");
        string authorId = await _authorService.CreateAuthor(expected);
        
        AuthorViewModel actual = await _authorService.GetAuthorById(authorId);

        // Assert
        Assert.Equal(expected, actual);
    }
}