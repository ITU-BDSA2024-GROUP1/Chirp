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

        await Logout();
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
        await Login(DefaultUsername, DefaultPassword);
        await FollowUser("Mellie Yost");
        await ClickLink("About me");
        await AssertContainsText("body", "Mellie Yost");
        
        // Clean-up
        await UnfollowUser("Mellie Yost");
    }

    [Test]
    public async Task CanUnfollow()
    {
        await Login(DefaultUsername, DefaultPassword);
        await UnfollowUser("emilbks");
        await ClickLink("About me");
        await AssertContainsText("body", "You are not following anybody.");
        
        // Clean-up
        await FollowUser("emilbks");
    }

    [Test]
    public async Task DeleteCheep()
    {
        await Login(DefaultUsername, DefaultPassword);
        
        const string cheep = "This cheep should disappear in a bit!";
        await PostCheep(cheep);
        await AssertCheepPosted(DefaultUsername, cheep);

        await ClickFirstListItemButton(DefaultUsername); // Deletes most recent tweet by default user.
        await Expect(Page.Locator("#messagelist")).Not.ToContainTextAsync($"{DefaultUsername} {cheep}");
        
    }
}