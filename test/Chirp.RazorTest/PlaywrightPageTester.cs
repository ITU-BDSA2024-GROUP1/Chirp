using Microsoft.Playwright;

namespace Chirp.RazorTest;

[Parallelizable(ParallelScope.None)]
[TestFixture]
public abstract class PlaywrightPageTester : PageTest
{
    private const string HomepageUrl = "http://localhost:5273/";
    private protected const string
        DefaultEmail = "ropf@itu.dk",
        DefaultUsername = "Helge",
        DefaultPassword = "LetM31n!";
    
    // Locators
    private ILocator GetLocator(string elementName) => Page.Locator(elementName);
    private ILocator GetByRole(AriaRole role) => Page.GetByRole(role);
    private ILocator GetByRole(AriaRole role, string elementName, bool exact) => Page.GetByRole(role, new() { Name = elementName, Exact = exact });
    private ILocator GetByLabel(string labelName, bool exact) => Page.GetByLabel(labelName, new() { Exact = exact });
    
    // Atomic Actions
    private async Task GotoHomePage() => await Page.GotoAsync(HomepageUrl);
    private protected async Task ClickLink(string linkName, bool exact = false) => await GetByRole(AriaRole.Link, linkName, exact).ClickAsync();
    private protected async Task ClickButton(string buttonName, bool exact = false) => await GetByRole(AriaRole.Button, buttonName, exact).ClickAsync();
    private async Task FillInputField(string labelName, string fillText, bool exact = false) => await GetByLabel(labelName, exact).ClickAndFill(fillText);
    
    // Composite Actions
    private protected async Task Register(string email, string username, string password, string? confirmationPassword = null)
    {
        await GotoHomePage();
        await ClickLink("register");
        
        await FillInputField("Email", email);
        await FillInputField("Username", username);
        await FillInputField("Password", password, true);
        await FillInputField("Confirm Password", confirmationPassword ?? password);
        
        await ClickButton("Register");
    }
    private protected async Task Login(string username, string password)
    {
        await GotoHomePage();
        await ClickLink("login");

        await FillInputField("Username", username);
        await FillInputField("password", password);
        
        await ClickButton("Log in", true);
    }
    private protected async Task PostCheep(string cheep)
    {
        await GotoHomePage();
        await GetLocator("#cheepText").ClickAndFill(cheep);
        await ClickButton("Share");
    }
    
    // Atomic Assertions
    private protected async Task AssertVisible(string locatorName) => await Expect(GetLocator(locatorName)).ToBeVisibleAsync();
    private async Task AssertContainsText(ILocator locator, string text) => await Expect(locator).ToContainTextAsync(text);
    private protected async Task AssertContainsText(string locatorName, string text) => await AssertContainsText(GetLocator(locatorName), text);
    private protected async Task AssertContainsText(AriaRole role, string text) => await AssertContainsText(GetByRole(role), text);
    
    // Composite Assertions
    private protected async Task AssertNotLoggedIn()
    {
        await GotoHomePage();
        await AssertContainsText("body", "public timeline");
        await AssertContainsText("body", "register");
        await AssertContainsText("body", "login");
    }
    private protected async Task AssertLoggedInAs(string username)
    {
        await GotoHomePage();
        await AssertContainsText("body", $"logout [{username}]");
        await AssertContainsText("h3", $"What's on your mind {username}?");
        await AssertContainsText("body", "my timeline");
        await AssertContainsText("body", "About me");
    }
    private protected async Task AssertCheepPosted(string username, string cheep)
    {
        await AssertContainsText("#messagelist", $"{username} {cheep}");
    }
}