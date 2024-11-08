using System.ComponentModel.DataAnnotations;

using Chirp.Core.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Chirp.Razor.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class FinishGithubLoginModel : PageModel
    {
        private readonly SignInManager<Author> _signInManager;
        private readonly UserManager<Author> _userManager;

        public FinishGithubLoginModel(
            UserManager<Author> userManager,
            SignInManager<Author> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public string ExistingGithubUsername { get; set; }
        [BindProperty]
        public string Email { get; set; }

        public class InputModel
        {
            [Display(Name = "Username")]
            [StringLength(50, ErrorMessage = "The username must be at least {2} and at most {1} characters long.", MinimumLength = 3)]
            public string UserName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string email, string existingGithubUsername)
        {
            Email = email;
            ExistingGithubUsername = existingGithubUsername;

            var user = await _userManager.FindByNameAsync(ExistingGithubUsername);
            if (user == null)
            {
                return NotFound("User not found.");
            } else
            {
                // Initialize Input if it's null
                if (Input == null)
                {
                    Input = new InputModel();
                }

                Input.UserName = ExistingGithubUsername;
            }

            return Page();
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

                // Create a new user with the chosen username
                var user = new Author { UserName = Input.UserName, Email = Email };
                var createResult = await _userManager.CreateAsync(user);

                if (createResult.Succeeded)
                {
                    var passwordResult = await _userManager.AddPasswordAsync(user, Input.Password);
                    if (passwordResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }

                    foreach (var error in passwordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
