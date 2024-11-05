using System.ComponentModel.DataAnnotations;

using Chirp.Core.Entities;
using Chirp.Core.DataTransferObject;
using Chirp.Infrastructure.CheepService;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel(ICheepService service) : PageModel
{
    public List<CheepViewModel> Cheeps { get; set; } = new List<CheepViewModel>();
    
    [Required]
    [StringLength(maximumLength:160, ErrorMessage = "The cheep must at most be 160 characters long.", MinimumLength = 0)]
    public string Text { get; set; }


    public async Task<ActionResult> OnGetAsync([FromQuery] int page = 1)
    {
        const int pageSize = 32; // Define your page size
        var cheepsResult = await service.GetCheeps(page, pageSize);
        if (cheepsResult.Items != null)
        {
            Cheeps.AddRange(cheepsResult.Items);
        }
       
        CurrentPage = cheepsResult.CurrentPage;
        TotalPages = cheepsResult.TotalPages;
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
        var newCheep = new CheepViewModel(User.Identity.Name, Text, DateTime.Now.ToString("g"));
        //Cheeps.Add(newCheep);

        // Optionally: Save the new message to a database or cache here
        await service.PostCheep(newCheep);
        
        // Redirect to avoid form re-submission
        return RedirectToPage();
    }


    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}
