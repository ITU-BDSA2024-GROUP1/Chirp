using Chirp.Core.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure.Services.FollowService;
using Chirp.Infrastructure.Services.AuthorService;
using Chirp.Infrastructure.Services.CheepService;

namespace Chirp.Razor.Pages
{
    public class AboutMeModel : PageModel
    {
        private readonly SignInManager<Author> _signInManager;
        private readonly UserManager<Author> _userManager;
        private readonly ICheepService _cheepService;
        private readonly IFollowService _followService;
        private readonly IAuthorService _authorService;
        public List<CheepViewModel> Cheeps { get; set; } = new List<CheepViewModel>();
        public List<FollowViewModel> Follows { get; set; } = new List<FollowViewModel>();
        public List<FollowViewModel> FollowedBy { get; set; } = new List<FollowViewModel>();


        public AboutMeModel(SignInManager<Author> signInManager, UserManager<Author> userManager, ICheepService cheepService, IFollowService followService, IAuthorService authorService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _cheepService = cheepService;
            _followService = followService;
            _authorService = authorService;
        }

        [BindProperty]
        public DataModel Data { get; set; }

        public bool githubUser { get; set; }

        public class DataModel
        {
            public string Email { get; set; }
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
        }

        public async Task<ActionResult> OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToPage("Login"); // Redirect to login page if not authenticated
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                RedirectToPage("Public");
            }

            Data = new DataModel()
            {
                Email = user.Email
            };

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info != null)
            {
                Data.LoginProvider = info.LoginProvider;
                Data.ProviderKey = info.ProviderKey;
                githubUser = true;
            } else
            {
                githubUser = false;
            }

            Follows = await _followService.GetFollowersByName(User.Identity.Name);
            FollowedBy = await _followService.GetFollowingByName(User.Identity.Name);

            var cheepCount = await _cheepService.GetCheepCount(user.UserName);
            var cheepsResult = await _cheepService.GetCheepsFromAuthor(user.UserName, 1, cheepCount);
            if (cheepsResult.Items != null)
            {
                Cheeps.AddRange(cheepsResult.Items);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostForgetMe()
        {
            await _authorService.DeleteAuthor(await _authorService.GetAuthorByName(User.Identity!.Name));
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}
