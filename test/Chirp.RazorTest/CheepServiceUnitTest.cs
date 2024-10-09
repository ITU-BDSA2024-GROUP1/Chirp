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
        CheepViewModel expected = new CheepViewModel("Jacqualine Gilcoine", "They were married in Chicago, with old Smith, and was expected aboard every day; meantime, the two went past me.", 1690895677);

        // Act
        List<CheepViewModel> test = cheepService.GetCheeps();

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
        CheepViewModel notExpected = new CheepViewModel("Jacqualine Gilcoine", "They were married in Chicago, with old Smith, and was expected aboard every day; meantime, the two went past me.", 1690895677);
        CheepViewModel expected = new CheepViewModel("Quintin Sitts", "Unless we succeed in establishing ourselves in some monomaniac way whatever significance might lurk in them.", 1690895674);

        // Act
        List<CheepViewModel> test = cheepService.GetCheepsFromAuthor("Quintin Sitts");

        // Assert
        Assert.Equal(expected, test[0]);
        Assert.NotEqual(notExpected, test[0]);

    }
}