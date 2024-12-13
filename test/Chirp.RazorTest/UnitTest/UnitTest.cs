namespace Chirp.RazorTest.UnitTest;

public class UnitTest : PlaywrightPageTester
{
    [Test] 
    public async Task CheepBoxVisible()
    {
       await Login(DefaultUsername, DefaultPassword);
       await AssertVisible("#cheepText");
    }
    
    [Test]
    public async Task CheepVisuallyShowsUp()
    {
       await Login(DefaultUsername, DefaultPassword);
       
       const string testCheep = "123";
       
       await PostCheep(testCheep);
       await AssertContainsText("#messagelist", "Delete");
    }
    
    [Test]
    public async Task CheepTooLong()
    {
       await Login(DefaultUsername, DefaultPassword);
       
       const string tooLongCheep = "Hello, my baby! Hello, my honey! Hello, my ragtime gal! Send me a kiss by wire. Baby, my heart's on fire! If you refuse me, Honey, you loose me. Then you'll be left alone";
       string expectedLongCheep = tooLongCheep[..159];
       
       await PostCheep(tooLongCheep);
       await AssertCheepPosted(DefaultUsername, expectedLongCheep);
    }

    [Test]
    public async Task ForgetMe()
    {
        await Register("BazzB@burner1.burner", "BazzB", "Scrh1ft_H");
        await ClickLink("About me");
        await ClickButton("Forget me");

        throw new NotImplementedException("We just need to assert that it worked by attempting to register the account again and having that be successful");
    }
}