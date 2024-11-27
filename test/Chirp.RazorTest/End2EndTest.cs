using Microsoft.Playwright;

namespace Chirp.RazorTest;

public class End2EndTest : PlaywrightPageTester
{
    private const string
        DefaultEmail = "ropf@itu.dk",
        DefaultUsername = "Helge",
        DefaultPassword = "LetM31n!";
    
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
        await AssertContainsText("#messagelist", cheep);
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
        await AssertContainsText("#messagelist", cheep);
    }

    [Test]
    public async Task LoginWithMissingFields()
    {
        await Login("", "");
        await AssertContainsText("#account", "The Username field is required.");
        await AssertContainsText("#account", "The Password field is required.");
    }

    [Test]
    public async Task BadLogin()
    {
        await Login(DefaultUsername, "Pa$$w0rd");
        await AssertContainsText(AriaRole.Listitem, "Invalid login attempt.");
    }

    [Test]
    public async Task RegisterWithMissingFields()
    {
        await Register("", "", "");
        await AssertContainsText("#registerForm", "The Email field is required.");
        await AssertContainsText("#registerForm", "The Username field is required.");
        await AssertContainsText("#registerForm", "The Password field is required.");
    }
    
    [Test]
    public async Task UsernameTaken()
    {
        await Register("name@example.com", DefaultUsername, "password");
        await AssertContainsText(AriaRole.Listitem, $"Username {DefaultUsername} is already taken.");
    }

    [Test]
    public async Task EmailTaken()
    {
        await Register(DefaultEmail, "Testerson", "Pa$$w0rd");
        await AssertContainsText(AriaRole.Listitem, $"Email '{DefaultEmail}' is already taken.");
    }
    
    [Test]
    public async Task PasswordsDoNotMatch()
    {
        await Register("name@example.com", "Testerson", DefaultPassword, "Pa$$w0rd");
        await AssertContainsText("#registerForm", "The password and confirmation password do not match.");
    }

    [Test]
    public async Task PasswordTooShort()
    {
        await Register("name@example.com", "Test Testerson", "a");
        await AssertContainsText("#registerForm", "The Password must be at least 6 and at max 100 characters long.");
    }
    
    [Test]
    public async Task BadPassword()
    {
        await Register("name@example.com", "Test Testerson", new('a', 6));
        await AssertContainsText(AriaRole.List, "Passwords must have at least one non alphanumeric character.");
        await AssertContainsText(AriaRole.List, "Passwords must have at least one digit ('0'-'9').");
        await AssertContainsText(AriaRole.List, "Passwords must have at least one uppercase ('A'-'Z').");
    }
}