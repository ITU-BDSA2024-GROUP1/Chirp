using Microsoft.Playwright;

namespace Chirp.RazorTest.UnitTest;

public class UnitTest : PlaywrightPageTester
{
    [Test] 
    public async Task CheepBoxTest(){
       await Login(DefaultUsername, DefaultPassword);
       await expect(page.locator('#cheepText')).toBeVisible();
    }
}
