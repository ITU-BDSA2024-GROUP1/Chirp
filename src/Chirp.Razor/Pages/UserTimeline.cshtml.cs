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

    [HttpGet("{author}")]
    public ActionResult OnGet([FromRoute] string author)
    {
        if (string.IsNullOrEmpty(author))
        {
            // Handle the case where author is null or empty
            // For example, you could return an empty list or a default page
            Cheeps = new List<CheepViewModel>();
        }
        else
        {
            Cheeps = _service.GetCheepsFromAuthor(author);
        }
        return Page();
    }
}
