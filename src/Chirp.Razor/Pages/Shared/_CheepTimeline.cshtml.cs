using System.ComponentModel.DataAnnotations;

using Chirp.Infrastructure.Models;
using Chirp.Infrastructure.Services.CheepService;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages.Shared;

public abstract class CheepTimeline(ICheepService service) : PageModel
{
    private protected readonly ICheepService _service = service;
    private protected const int PageSize = 32;
    
    public List<CheepViewModel> Cheeps { get; set; } = [];
    
    [BindProperty]
    [Required]
    [StringLength(maximumLength: 160, ErrorMessage = "The cheep must at most be 160 characters long.", MinimumLength = 0)]
    public string? Text { get; set; }

    private protected PageResult PopulateTimeline(PagedResult<CheepViewModel> cheeps)
    {
        if (cheeps.Items != null)
        {
            Cheeps.AddRange(cheeps.Items);
        }
       
        CurrentPage = cheeps.CurrentPage;
        TotalPages = cheeps.TotalPages;
        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            // Return the page with validation errors if any
            return Page();
        }

        // Add the new cheep to Cheeps
        var newCheep = new CheepViewModel(User.Identity?.Name, Text, DateTime.Now.ToString("G"));
        //Cheeps.Add(newCheep);

        // Optionally: Save the new message to a database or cache here
        await _service.PostCheep(newCheep);

        // Redirect to avoid form re-submission
        return RedirectToPage();
    }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string BaseUrl { get; set; } = "/";
}