using NUnit.Framework.Internal;

namespace TestProject1;

public class UnitTest : PageTest
{
    [SetUp]
    public async Task login()
    {
        await Page.GotoAsync("https://bdsagroup1chirpremotedb.azurewebsites.net/Identity/Account/Login");
        await Page.FillAsync("Input.Email", "Adrian");
        await Page.FillAsync("Input_Password", "M32Want_Access");
        await Page.ClickAsync("Log in");
        await Page.WaitForURLAsync("https://bdsagroup1chirpremotedb.azurewebsites.net"); 
    }

    [TearDown]
    public async Task teardown()
    {
        Page.ClickAsync("logout[Adrian]");
    }

    [Test]
    public async Task Login()   
    {
        await Expect(Page.GetByText("Share")).ToBeVisibleAsync();
    } 
}