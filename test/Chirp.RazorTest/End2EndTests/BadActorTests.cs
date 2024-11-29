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
}