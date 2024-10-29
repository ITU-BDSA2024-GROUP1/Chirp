using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGetAsync([FromRoute] string author, [FromQuery] int page = 1)
    {
        if (string.IsNullOrEmpty(author))
        {
            Cheeps = new List<CheepViewModel>();
        }
        else
        {
            var pageSize = 32; // Define your page size
            var cheepsResult = await _service.GetCheepsFromAuthor(author, page, pageSize);
            Cheeps = cheepsResult.Items;
            CurrentPage = cheepsResult.CurrentPage;
            TotalPages = cheepsResult.TotalPages;
        }
        return Page();
    }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

}
