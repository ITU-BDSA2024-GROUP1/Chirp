using Microsoft.Playwright;

namespace Chirp.RazorTest;

public static class PlaywrightHelper
{
    public static async Task ClickAndFill(this ILocator locator, string fillText)
    {
        await locator.ClickAsync();
        await locator.FillAsync(fillText);
    }

    public static ILocator Filter(this ILocator locator, string filterText)
    {
        return locator.Filter(new() { HasText = filterText });
    }
}