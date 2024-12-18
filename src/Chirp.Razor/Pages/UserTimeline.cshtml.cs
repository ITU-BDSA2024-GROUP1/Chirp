using System.ComponentModel.DataAnnotations;

using Chirp.Infrastructure.Services.CheepService;
using Chirp.Infrastructure.Services.FollowService;
using Chirp.Razor.Pages.Shared;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;

using NuGet.Packaging.Signing;

namespace Chirp.Razor.Pages;

public class UserTimelineModel(ICheepService service, IFollowService followService) : CheepTimeline(service, followService)
{
    public string Text { get; set; }
    public int Incrementer { get; set; } = 0;
    public int Increment(){return ++Incrementer;}
    public async Task<ActionResult> OnGetAsync([FromRoute] string author, [FromQuery] int page = 1)
    {
        if (string.IsNullOrEmpty(author))
        {
            Cheeps = [];
            return Page();
        }
        
        List<string> authors = await GetFollowers(author);
        var cheepsResult = await _service.GetCheepsFromAuthor(authors, page, PageSize);
        return PopulateTimeline(cheepsResult);
    }

    private async Task<List<string>> GetFollowers(string author)
    {
        List<string> authors = [];
        
        List<FollowViewModel> follows = await _followService.GetFollowersByName(author);
        authors.AddRange(follows.Select(f => f.FollowedName));
        authors.Add(author);

        return authors;
    }
}
