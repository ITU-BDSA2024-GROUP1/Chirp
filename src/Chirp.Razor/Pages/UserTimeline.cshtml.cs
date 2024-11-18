using Chirp.Infrastructure.Services.CheepService;
using Chirp.Razor.Pages.Shared;

using Microsoft.AspNetCore.Mvc;

namespace Chirp.Razor.Pages;

public class UserTimelineModel(ICheepService service) : CheepTimeline(service)
{
    public async Task<ActionResult> OnGetAsync([FromRoute] string author, [FromQuery] int page = 1)
    {
        if (string.IsNullOrEmpty(author))
        {
            Cheeps = [];
            return Page();
        }

        BaseUrl = $"/{author}";
        
        var cheepsResult = await _service.GetCheepsFromAuthor(author, page, PageSize);
        return PopulateTimeline(cheepsResult);
    }
}
