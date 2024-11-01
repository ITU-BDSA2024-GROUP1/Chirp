using Chirp.Infrastructure.CheepService;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel(ICheepService service) : PageModel
{
    public List<CheepViewModel> Cheeps { get; set; }

    public async Task<ActionResult> OnGetAsync([FromQuery] int page = 1)
    {
        const int pageSize = 32; // Define your page size
        var cheepsResult = await service.GetCheeps(page, pageSize);
        Cheeps = cheepsResult.Items;
        CurrentPage = cheepsResult.CurrentPage;
        TotalPages = cheepsResult.TotalPages;
        return Page();
    }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}
