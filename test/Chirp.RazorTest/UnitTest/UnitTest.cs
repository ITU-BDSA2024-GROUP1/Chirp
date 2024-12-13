using Azure;

using Microsoft.Playwright;

namespace Chirp.RazorTest.UnitTest;

public class UnitTest : PlaywrightPageTester
{
    [Test] 
    public async Task CheepBoxTest(){
       await Login(DefaultUsername, DefaultPassword);
       await Expect(Page.Locator("#cheepText")).ToBeVisibleAsync();
    }
    [Test]
    public async Task SendCheepVisualTest(){
       await Login(DefaultUsername, DefaultPassword);
       string testCheep = "123";
       
       await PostCheep(testCheep);
       await AssertContainsText(AriaRole.Button, "Delete");
    }
    [Test]
    public async Task ToolongCheepTest(){
       await Login(DefaultUsername, DefaultPassword);
       string tooLongCheep =
           "Hello, my baby! Hello, my honey! Hello, my ragtime gal! Send me a kiss by wire. Baby, my heart's on fire! If you refuse me, Honey, you loose me. Then you'll be left alone";
       string expectedLongCheep =
           "Hello, my baby! Hello, my honey! Hello, my ragtime gal! Send me a kiss by wire. Baby, my heart's on fire! If you refuse me, Honey, you loose me. Then you'll be";
       await PostCheep(tooLongCheep);
       await AssertCheepPosted(DefaultUsername, expectedLongCheep);
    }

    [Test]
    public async Task ForgetmeTest()
    {
        await Register("BazzB@burner1.burner", "BazzB", "Scrh1ft_H");
        await ClickLink("About me");
        await ClickButton("Forget me");
    }
    
}
