using System.ComponentModel.DataAnnotations;

using Chirp.Infrastructure.Models;
using Chirp.Infrastructure.Services.CheepService;
using Chirp.Infrastructure.Services.FollowService;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace Chirp.Razor.Pages.Shared;

public abstract class CheepTimeline(ICheepService service, IFollowService followService) : PageModel
{
    private protected readonly ICheepService _service = service;
    private protected readonly IFollowService _followService = followService;
    private protected const int PageSize = 32;
    
    public List<CheepViewModel> Cheeps { get; set; } = [];
    public string FollowOrUnfollow { get; set; } = "Follow";
    
    [BindProperty]
    [Required]
    [StringLength(maximumLength: 160, ErrorMessage = "The cheep must at most be 160 characters long.", MinimumLength = 0)]
    public string? Text { get; set; }

    public int Incrementer { get; set; } = 0;
    public int Increment() { return ++Incrementer; }

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
    
    public IActionResult OnPostChangeFollowStatus()
    {
        var cheepAuthor = Request.Form["cheepAuthor"];
        if (!User.Identity!.IsAuthenticated || cheepAuthor.IsNullOrEmpty()) return RedirectToPage();
        
        if (AFollowsBAsync(User.Identity.Name!, cheepAuthor!).Result)
        {
            _followService.RemoveFollow(new(User.Identity!.Name, cheepAuthor));
            return RedirectToPage();
        }
        
        _followService.AddFollow(new(User.Identity!.Name, cheepAuthor));
        return RedirectToPage();
        
        
    }
    
    public async Task<bool> AFollowsBAsync(string a, string b)
    {
        FollowViewModel followViewModel = await _followService.GetFollow(a, b);
        
        return followViewModel != null;
    }
    
    public async Task<IActionResult> OnPostDeleteCheep()
    {
        CheepViewModel cheep = new(Request.Form["cheepAuthor"], Request.Form["cheepMessage"], Request.Form["cheepTimestamp"]);
        await _service.DeleteCheep(cheep);
        
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEditCheep(string cheepAuthor, string newCheepMessage, string cheepMessage, string cheepTimestamp)
    {
        if (string.IsNullOrEmpty(cheepAuthor) || string.IsNullOrEmpty(newCheepMessage) || string.IsNullOrEmpty(cheepTimestamp))
        {
            return RedirectToPage();
        }


        await service.UpdateCheep(new CheepViewModel(cheepAuthor, newCheepMessage, cheepTimestamp), cheepMessage);
        return RedirectToPage();
    }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string BaseUrl { get; set; } = "/";
}