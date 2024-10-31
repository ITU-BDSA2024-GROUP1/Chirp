using Chirp.Core.DataTransferObject;
using Chirp.Core.Models;
using Chirp.Infrastructure.CheepService;

namespace Chirp.InfrastructureTest;

public class CheepServiceUnitTest : InfrastructureServiceTester
{
    private readonly CheepDTO[] _knownCheeps;
    private readonly CheepService _cheepService;
    
    public CheepServiceUnitTest()
    {
        ICheepRepository cheepRepository = new CheepRepository(_context);
        _knownCheeps = SetUpTestCheepDB(cheepRepository, _knownAuthors).Result;

        _cheepService = new(cheepRepository, _authorRepository);
    }
    
    [Fact]
    public void MakeCheepService()
    {
        // Assert
        Assert.NotNull(_cheepService);
    }

    [Fact]
    public async Task GetCheeps()
    {
        // Act
        PagedResult<CheepViewModel> cheeps = await _cheepService.GetCheeps(_knownCheeps.Length/32, 32);

        CheepViewModel
            expected = CheepService.CheepDTOToCheepViewModel(_knownCheeps[^1]),
            actual = cheeps.Items[^1];

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetCheepsFromAuthor()
    {
        // Act
        int pageNumber = _knownCheeps.Length;
        pageNumber /= _knownAuthors.Length;
        pageNumber /= 32;
        pageNumber += (_knownCheeps.Length / _knownAuthors.Length) % 32 == 0 ? 0 : 1;
        PagedResult<CheepViewModel> cheeps = await _cheepService.GetCheepsFromAuthor(_knownAuthors[1].Name, pageNumber, 32);

        CheepViewModel 
            expected = CheepService.CheepDTOToCheepViewModel(_knownCheeps[^2]),
            actual = cheeps.Items[^1];

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetCheepById()
    {
        // Act
        CheepViewModel expected = CheepService.CheepDTOToCheepViewModel(_knownCheeps[0]);
        CheepViewModel actual = await _cheepService.GetCheepById(_knownCheeps[0].Id);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task PostCheep()
    {
        // Arrange
        await ClearDB("cheeps");
        
        // Act
        CheepViewModel expected = new("Test1", "Testing, attention please", DateTime.Now.ToString(@"yyyy\-MM\-dd HH\:mm\:ss"));
        int cheepId = await _cheepService.PostCheep(expected);
        
        CheepViewModel actual = await _cheepService.GetCheepById(cheepId);

        // Assert
        Assert.Equal(expected, actual);
    }
}