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
    public async Task RegisterAccount()
    {
        await ClickLink("register");

        int id = Random.Shared.Next(1_000_000);
        await FillInputField("Email", $"testerson{id}@playwright.com");
        await FillInputField("Username", $"Playwright Testerson {id}");
        await FillInputField("Password", "Pa$$w0rd", true);
        await FillInputField("Confirm Password", "Pa$$w0rd");
        
        await ClickButton("Register");
        
        await Expect(Page.Locator("button")).ToContainTextAsync($"logout [Playwright Testerson {id}]");
        await Expect(Page.Locator("h3")).ToContainTextAsync($"What's on your mind Playwright Testerson {id}?");
        await Expect(Page.Locator("body")).ToContainTextAsync("my timeline");
    }

    [Test]
    public async Task Login()
    {
        await ClickLink("login");

        await FillInputField("Username", "Helge");
        await FillInputField("password", "LetM31n!");
        
        await ClickButton("Log in", true);
        
        await Expect(Page.Locator("button")).ToContainTextAsync("logout [Helge]");
        await Expect(Page.Locator("h3")).ToContainTextAsync("What's on your mind Helge?");
        await Expect(Page.Locator("body")).ToContainTextAsync("my timeline");
    }

    [Test]
    public async Task WriteCheep()
    {
        await ClickLink("login");
        
        await FillInputField("Username", "Helge");
        await FillInputField("password", "LetM31n!");
        
        await ClickButton("Log in", true);
        
        int id = Random.Shared.Next(1_000_000);
        await Page.Locator("#cheepText").ClickAndFill($"Cheeping with playwright! Edition: {id}");
        
        await ClickButton("Share");
        
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync($"Helge Cheeping with playwright! Edition: {id}");
    }

    private async Task ClickLink(string linkName, bool exact = false) => await Page.GetByRole(AriaRole.Link, new() { Name = linkName, Exact = exact }).ClickAsync();
    private async Task ClickButton(string buttonName, bool exact = false) => await Page.GetByRole(AriaRole.Button, new() { Name = buttonName, Exact = exact }).ClickAsync();
    private async Task FillInputField(string labelName, string fillText, bool exact = false) => await Page.GetByLabel(labelName, new() { Exact = exact }).ClickAndFill(fillText);
}