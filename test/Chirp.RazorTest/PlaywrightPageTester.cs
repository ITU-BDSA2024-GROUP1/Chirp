using Microsoft.Playwright;

namespace Chirp.RazorTest;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public abstract class PlaywrightPageTester : PageTest
{
    private const string HomepageUrl = "http://localhost:5273/";

    // Locators
    private ILocator GetLocator(string elementName) => Page.Locator(elementName);
    private ILocator GetByRole(AriaRole role, string elementName, bool exact) => Page.GetByRole(role, new() { Name = elementName, Exact = exact });
    private ILocator GetByLabel(string labelName, bool exact) => Page.GetByLabel(labelName, new() { Exact = exact });
    
    // Atomic Actions
    private async Task GotoHomePage() => await Page.GotoAsync(HomepageUrl);
    private async Task ClickLink(string linkName, bool exact = false) => await GetByRole(AriaRole.Link, linkName, exact).ClickAsync();
    private async Task ClickButton(string buttonName, bool exact = false) => await GetByRole(AriaRole.Button, buttonName, exact).ClickAsync();
    private async Task FillInputField(string labelName, string fillText, bool exact = false) => await GetByLabel(labelName, exact).ClickAndFill(fillText);
    
    // Composite Actions
    private protected async Task Register(string email, string username, string password)
    {
        await GotoHomePage();
        await ClickLink("register");
        
        await FillInputField("Email", email);
        await FillInputField("Username", username);
        await FillInputField("Password", password, true);
        await FillInputField("Confirm Password", password);
        
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
    private protected async Task Logout()
    {
        await GotoHomePage();
        await ClickButton("logout");
    }
    private protected async Task PostCheep(string cheep)
    {
        await GotoHomePage();
        await GetLocator("#cheepText").ClickAndFill(cheep);
        await ClickButton("Share");
    }
    
    // Atomic Assertions
    private async Task AssertContainsText(string locatorName, string text) => await Expect(GetLocator(locatorName)).ToContainTextAsync(text);
    private async Task AssertCheepPosted(string authorName, string cheep) => await AssertContainsText("#messagelist", $"{authorName} {cheep}");
    
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
        await AssertContainsText("button", $"logout [{username}]");
        await AssertContainsText("h3", $"What's on your mind {username}?");
        await AssertContainsText("body", "my timeline");
    }
    private protected async Task AssertCheepPostedOnHomepage(string authorName, string cheep)
    {
        await GotoHomePage();
        await AssertCheepPosted(authorName, cheep);
    }
    private protected async Task AssertCheepPostedOnAuthorPage(string authorName, string cheep)
    {
        await GotoHomePage();
        await AssertLoggedInAs(authorName);
        
        await ClickLink("my timeline");
        await AssertContainsText("h2", $"{authorName}'s Timeline");
        await AssertCheepPosted(authorName, cheep);
    }
}