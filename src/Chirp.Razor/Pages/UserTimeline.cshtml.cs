using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;

using Chirp.Core.Entities;
using Chirp.Infrastructure.CheepService;
using Chirp.Infrastructure.FollowService;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;

using NuGet.Packaging.Signing;

namespace Chirp.Razor.Pages;

public class UserTimelineModel(ICheepService cheepService, IFollowService followService) : PageModel
{
    public List<CheepViewModel> Cheeps { get; set; } = new List<CheepViewModel>();
    public string FollowOrUnfollow { get; set; } = "Follow";
    public int incrementer { get; set; } = 0;

    [BindProperty]
    [Required]
    [StringLength(maximumLength: 160, ErrorMessage = "The cheep must at most be 160 characters long.", MinimumLength = 0)]

    public string Text { get; set; }
    public int increment(){return ++incrementer;}
    public async Task<ActionResult> OnGetAsync([FromRoute] string author, [FromQuery] int page = 1)
    {
        if (string.IsNullOrEmpty(author))
        {
            Cheeps = [];
            return Page();
        }

        const int pageSize = 32; // Define your page size
        List<FollowViewModel> Follows = await followService.GetFollowersByName(author);
        List<string> authors = new List<string>();
        foreach (FollowViewModel f in Follows) authors.Add(f.followedName);
        authors.Add(author);
        var cheepsResult = await cheepService.GetCheepsFromAuthor(authors, page, pageSize);
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
        await cheepService.PostCheep(newCheep);

        // Redirect to avoid form re-submission
        return RedirectToPage();
    }
    public IActionResult OnPostChangeFollowStatus()
    {
        var cheepAuthor = Request.Form["cheepAuthor"];
        if (!User.Identity!.IsAuthenticated || cheepAuthor.IsNullOrEmpty()) return RedirectToPage();
        if (AFollowsBAsync(User.Identity.Name!, cheepAuthor!).Result)
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

    public async Task<IActionResult> OnPostDeleteCheep()
    {
        CheepViewModel cheep = new CheepViewModel(Request.Form["cheepAuthor"], Request.Form["cheepMessage"], Request.Form["cheepTimestamp"]);
        await cheepService.DeleteCheep(cheep);
        return RedirectToPage();
    }
    public async Task<IActionResult> OnPostEditCheep(string cheepAuthor, string cheepMessage, string originalCheepMessage, string timeStamp)
    {
        if (string.IsNullOrEmpty(cheepAuthor) || string.IsNullOrEmpty(cheepMessage) || string.IsNullOrEmpty(timeStamp))
        {
            return RedirectToPage();
        }
        

        await cheepService.UpdateCheep(new CheepViewModel(cheepAuthor, cheepMessage, timeStamp), originalCheepMessage);
        return RedirectToPage();
    }


    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

}
