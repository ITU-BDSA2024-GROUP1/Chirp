using Chirp.Infrastructure.CheepService;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel(ICheepService service) : PageModel
{
    public List<CheepViewModel> Cheeps { get; set; }
    public string Text { get; set; }

    public async Task<ActionResult> OnGetAsync([FromRoute] string author, [FromQuery] int page = 1)
    {
        if (string.IsNullOrEmpty(author))
        {
            Cheeps = [];
            return Page();
        }

        const int pageSize = 32; // Define your page size
        var cheepsResult = await service.GetCheepsFromAuthor(author, page, pageSize);
        Cheeps = cheepsResult.Items;
        CurrentPage = cheepsResult.CurrentPage;
        TotalPages = cheepsResult.TotalPages;
        return Page();
    }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

}
