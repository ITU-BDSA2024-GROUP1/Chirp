using Microsoft.Playwright;

namespace Chirp.RazorTest;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class End2EndTest : PageTest
{
    private const string PageUrl = "http://localhost:5273/";

    [SetUp]
    public async Task SetUp()
    {
        await Page.GotoAsync(PageUrl);
    }

    [Test]
    public async Task IsRunning()
    {
        var response = await Page.APIRequest.HeadAsync(PageUrl);
        await Expect(response).ToBeOKAsync();
    }
    
    [Test]
    public async Task HasTitle()
    {
        await Expect(Page).ToHaveTitleAsync(Regex("Chirp"));
    }

    [Test]
    public async Task PublicTimelineLeadsToHomePage()
    {
        await ClickLink("public timeline");
        await Expect(Page.Locator("h2")).ToContainTextAsync("Public Timeline");
    }

    [Test]
    public async Task CanRegister()
    {
        await ClickLink("register");
        await Expect(Page.Locator("body")).ToContainTextAsync("Register");
    }

    [Test]
    public async Task CanLogin()
    {
        await ClickLink("login");
        await Expect(Page.Locator("body")).ToContainTextAsync("Log in");
    }

    [Test]
    public async Task RegisterAccount()
    {
        await Page.GetByRole(AriaRole.Link, new() { Name = "register" }).ClickAsync();
        
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("test@playwright.com");
        
        await Page.GetByLabel("Username").ClickAsync();
        await Page.GetByLabel("Username").FillAsync("Playwright Testerson");
        
        await Page.GetByLabel("Password", new() { Exact = true }).ClickAsync();
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Pa$$w0rd");
        
        await Page.GetByLabel("Confirm Password").ClickAsync();
        await Page.GetByLabel("Confirm Password").FillAsync("Pa$$w0rd");
        
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();

        await Expect(Page).ToHaveURLAsync(PageUrl);
        
        await Expect(Page.Locator("button")).ToContainTextAsync("logout [Playwright Testerson]");
        await Expect(Page.Locator("h3")).ToContainTextAsync("What's on your mind Playwright Testerson?");
        await Expect(Page.Locator("body")).ToContainTextAsync("my timeline");
    }

    [TearDown]
    public async Task TearDown()
    {
        
    }

    private async Task ClickLink(string byName)
    {
        await Page.GetByRole(AriaRole.Link, new() { Name = byName }).ClickAsync();
    }
    
    private static Regex Regex(string pattern) => new(pattern);
}