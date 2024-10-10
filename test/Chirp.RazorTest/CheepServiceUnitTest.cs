namespace Chirp.RazorTest;

public class CheepServiceUnitTest
{
    [Fact]
    public void MakeCheepViewModel()
    {
        // Act
        CheepViewModel cvm = new CheepViewModel("te", "st", 0);

        // Assert
        Assert.NotNull(cvm);
    }


    [Fact]
    public void MakeCheepService()
    {
        // Arrange
        string dbPath = Environment.GetEnvironmentVariable("CHIRPDBPATH") ?? System.IO.Path.Combine(System.IO.Path.GetTempPath(), "chirp.db");
        DBFacade dbFacade = new DBFacade(dbPath);

        // Act
        CheepService cheepService = new CheepService(dbFacade);

        // Assert
        Assert.NotNull(cheepService);

    }

    [Fact]
    public void Read()
    {
        // Arrange
        string dbPath = Environment.GetEnvironmentVariable("CHIRPDBPATH") ?? System.IO.Path.Combine(System.IO.Path.GetTempPath(), "chirp.db");
        DBFacade dbFacade = new DBFacade(dbPath);
        CheepService cheepService = new CheepService(dbFacade);
        CheepViewModel expected = new CheepViewModel("Jacqualine Gilcoine", "Starbuck now is what we hear the worst.", 1690895859);

        // Act
        List<CheepViewModel> test = cheepService.GetCheeps(0);

        // Assert
        Assert.Equal(expected, test[0]);

    }

    [Fact]
    public void ReadAuthor()
    {
        // Arrange
        string dbPath = Environment.GetEnvironmentVariable("CHIRPDBPATH") ?? System.IO.Path.Combine(System.IO.Path.GetTempPath(), "chirp.db");
        DBFacade dbFacade = new DBFacade(dbPath);
        CheepService cheepService = new CheepService(dbFacade);
        CheepViewModel notExpected = new CheepViewModel("Jacqualine Gilcoine", "Starbuck now is what we hear the worst.", 1690895859);
        CheepViewModel expected = new CheepViewModel("Quintin Sitts", "It's bad enough to appal the stoutest man who was my benefactor, and all for our investigation.", 1690895852);

        // Act
        List<CheepViewModel> test = cheepService.GetCheepsFromAuthor("Quintin Sitts", 0);

        // Assert
        Assert.Equal(expected, test[0]);
        Assert.NotEqual(notExpected, test[0]);

    }
}