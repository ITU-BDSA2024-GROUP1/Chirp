﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel>? Cheeps { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet([FromRoute] string author, [FromQuery] int page = 1)
    {
        if (string.IsNullOrEmpty(author))
        {
            // Handle the case where author is null or empty
            // For example, you could return an empty list or a default page
            Cheeps = new List<CheepViewModel>();
        }
        else
        {
            Cheeps = _service.GetCheepsFromAuthor(author,page);
        }
        return Page();
    }
}
