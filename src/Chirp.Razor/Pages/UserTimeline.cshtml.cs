using System.ComponentModel.DataAnnotations;

using Chirp.Core.Entities;
using Chirp.Infrastructure.CheepService;
using Chirp.Infrastructure.FollowService;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel(ICheepService service, IFollowService followService) : PageModel
{
    public List<CheepViewModel> Cheeps { get; set; } = new List<CheepViewModel>();

    [BindProperty]
    [Required]
    [StringLength(maximumLength: 160, ErrorMessage = "The cheep must at most be 160 characters long.", MinimumLength = 0)]

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
    public IActionResult OnPostChangeFollowStatus()
    {
        var cheepAuthor = Request.Form["cheepAuthor"];
        if (AFollowsBAsync(User.Identity.Name, cheepAuthor).Result)
        {
            followService.RemoveFollow(new FollowViewModel(User.Identity!.Name, cheepAuthor));
            return RedirectToPage();
        }
        followService.AddFollow(new FollowViewModel(User.Identity!.Name, cheepAuthor));
        return RedirectToPage();
    }

    public async Task<bool> AFollowsBAsync(string A, string B)
    {
        FollowViewModel FVM = await followService.GetFollow(A, B);
        if (FVM != null) return true;
        return false;
    }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

}
