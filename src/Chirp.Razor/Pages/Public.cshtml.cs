using Chirp.Core.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel>? Cheeps { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGetAsync([FromQuery] int page = 1)
    {
        var pageSize = 32; // Define your page size
        var cheepsResult = await _service.GetCheeps(page, pageSize);
        Cheeps = cheepsResult.Items;
        CurrentPage = cheepsResult.CurrentPage;
        TotalPages = cheepsResult.TotalPages;
        return Page();
    }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}
