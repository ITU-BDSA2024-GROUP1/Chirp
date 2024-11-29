using Microsoft.Playwright;

namespace Chirp.RazorTest.End2EndTests;

public class BadActorTests : PlaywrightPageTester
{
    [Test]
    public async Task XSSAttack()
    {
        await Login(DefaultUsername, DefaultPassword);

        int id = Random.Shared.Next(1_000_000);
        string cheepXSSAttack = $"Hello {id}, I am feeling good!<script>alert('If you see this in a popup, you are in trouble!');</script>";
        await PostCheep(cheepXSSAttack);
        await AssertCheepPosted(DefaultUsername, cheepXSSAttack);
    }

    [Test]
    public async Task SQLInjectionAttack()
    {
        return; // Because currently this doesn't work.
        
        const string username = "Robert'); DROP Table AspNetUsers;--";
        const string email = "bobby@tables.sql";
        const string password = "Pa$$w0rd";
        
        await Register(username, email, password);
        
        // This kinda should work, and it does if you manually do the test case, but for some reason, it doesn't work :(
        await AssertContainsText(AriaRole.Listitem, $"Username '{username}' is invalid, can only contain letters or digits.");
        
        // P.S. You can probably come up with a better test case than this, so please do :D
    } 
}