namespace Chirp.RazorTest;

public class End2EndTest : PlaywrightPageTester
{
    private const string DefaultUsername = "Helge", DefaultPassword = "LetM31n!";
    
    [Test]
    public async Task AnonymousUserNotLoggedIn()
    {
        await AssertNotLoggedIn();
    }
    
    [Test]
    public async Task CanRegisterAccount()
    {
        int id = Random.Shared.Next(1_000_000);
        string email = $"testerson{id}@playwright.com";
        string username = $"Playwright Testerson {id}";
        
        await Register(email, username, "Pa$$w0rd");
        await AssertLoggedInAs(username);
    }

    [Test]
    public async Task CanLogin()
    {
        await Login(DefaultUsername, DefaultPassword);
        await AssertLoggedInAs(DefaultUsername);
    }

    [Test]
    public async Task CanLogout()
    {
        await Login(DefaultUsername, DefaultPassword);
        await AssertLoggedInAs(DefaultUsername);
        
        await Logout();
        await AssertNotLoggedIn();
    }
    
    [Test]
    public async Task CanCheep()
    {
        await Login(DefaultUsername, DefaultPassword);
        
        int id = Random.Shared.Next(1_000_000);
        string cheep = $"Cheeping with playwright! Edition: {id}";
        
        await PostCheep(cheep);
        await AssertCheepPostedOnHomepage(DefaultUsername, cheep);
    }

    [Test]
    public async Task CheepsAppearOnOwnTimeline()
    {
        await Login(DefaultUsername, DefaultPassword);
        
        const string cheep = "Cheeping for my homies with playwright!";
        await PostCheep(cheep);
        await AssertCheepPostedOnAuthorPage(DefaultUsername, cheep);
    }
}