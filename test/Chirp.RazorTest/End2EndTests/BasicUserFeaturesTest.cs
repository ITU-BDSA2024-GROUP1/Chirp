namespace Chirp.RazorTest.End2EndTests;

public class BasicUserFeaturesTest : PlaywrightPageTester
{
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
        
        await ClickButton("logout");
        await AssertNotLoggedIn();
    }
    
    [Test]
    public async Task CanCheep()
    {
        await Login(DefaultUsername, DefaultPassword);
        
        int id = Random.Shared.Next(1_000_000);
        string cheep = $"Cheeping with playwright! {id}";
        
        await PostCheep(cheep);
        await AssertCheepPosted(DefaultUsername, cheep);
    }

    [Test]
    public async Task CheepsAppearOnOwnTimeline()
    {
        await Login(DefaultUsername, DefaultPassword);
        
        int id = Random.Shared.Next(1_000_000);
        string cheep = $"Cheeping for my homies with playwright! {id}";
        
        await PostCheep(cheep);
        
        await ClickLink("my timeline");
        await AssertContainsText("h2", $"{DefaultUsername}'s Timeline");
        await AssertCheepPosted(DefaultUsername, cheep);
    }

    [Test]
    public async Task CanFollow()
    {
        throw new NotImplementedException();
    }

    [Test]
    public async Task CanUnfollow()
    {
        throw new NotImplementedException();
    }
}