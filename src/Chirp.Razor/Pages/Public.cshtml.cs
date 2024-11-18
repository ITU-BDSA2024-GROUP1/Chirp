using Chirp.Infrastructure.Services.CheepService;
using Chirp.Razor.Pages.Shared;

using Microsoft.AspNetCore.Mvc;

namespace Chirp.Razor.Pages;

public class PublicModel(ICheepService service) : CheepTimeline(service)
{
    public async Task<ActionResult> OnGetAsync([FromQuery] int page = 1)
    {
        var cheepsResult = await _service.GetCheeps(page, PageSize);
        return PopulateTimeline(cheepsResult);
    }
}
