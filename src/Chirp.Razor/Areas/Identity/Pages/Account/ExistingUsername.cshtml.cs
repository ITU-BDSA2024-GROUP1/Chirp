using Chirp.Core.Entities;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExistingUsernameModel : PageModel
    {
        private readonly SignInManager<Author> _signInManager;
        private readonly UserManager<Author> _userManager;
        public ExistingUsernameModel(
            UserManager<Author> userManager,
            SignInManager<Author> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(50, ErrorMessage = "The username must be at least {2} and at most {1} characters long.", MinimumLength = 3)]
            public string UserName { get; set; }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByNameAsync(Input.UserName);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Username " + Input.UserName + " is already taken. Please choose a different one.");
                    return Page();
                }

                // Retrieve data from TempData
                var email = TempData["TemporaryEmail"] as string; 
                var loginProvider = TempData["LoginProvider"] as string; 
                var providerKey = TempData["ProviderKey"] as string;

                if (email != null && loginProvider != null && providerKey != null)
                {
                    var user = new Author { UserName = Input.UserName, Email = email };
                    var createResult = await _userManager.CreateAsync(user);
                    if (createResult.Succeeded)
                    {
                        var info = new UserLoginInfo(loginProvider, providerKey, loginProvider);
                        var addLogin = await _userManager.AddLoginAsync(user, info);
                        if (addLogin.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }

                    foreach (var error in createResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return Page();
        }
    }
}
