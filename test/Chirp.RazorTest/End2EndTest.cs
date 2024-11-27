namespace Chirp.RazorTest;

public class End2EndTest : PlaywrightPageTester
{
    [Test]
    public async Task RegisterAccount()
    {
        int id = Random.Shared.Next(1_000_000);
        string email = $"testerson{id}@playwright.com";
        string username = $"Playwright Testerson {id}";
        
        await Register(email, username, "Pa$$w0rd");
        await AssertLoggedInAs(username);
    }

    [Test]
    public async Task Login()
    {
        const string username = "Helge";
        
        await Login(username, "LetM31n!");
        await AssertLoggedInAs(username);
    }

    [Test]
    public async Task WriteCheep()
    {
        const string username = "Helge";
        await Login(username, "LetM31n!");
        
        int id = Random.Shared.Next(1_000_000);
        string cheep = $"Cheeping with playwright! Edition: {id}";
        
        await PostCheep(cheep);
        await AssertCheepPosted($"{username} {cheep}");
    }
}