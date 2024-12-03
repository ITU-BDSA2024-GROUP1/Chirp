using Microsoft.Playwright;

namespace Chirp.RazorTest.End2EndTests;

public class BadInteractionsTest : PlaywrightPageTester
{
    [Test]
    public async Task CheepNothing()
    {
        throw new NotImplementedException();
    }

    [Test]
    public async Task CheepTooLong()
    {
        throw new NotImplementedException();
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
    public async Task BadUsername()
    {
        const string badUsername = "\u00af\\_(ツ)_/\u00af"; // ¯\_(ツ)_/¯ <-- This emoji
        await Register("name@example.com", badUsername, "Pa$$w0rd");
        await AssertContainsText(AriaRole.Listitem, $"Username '{badUsername}' is invalid, can only contain letters or digits.");
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