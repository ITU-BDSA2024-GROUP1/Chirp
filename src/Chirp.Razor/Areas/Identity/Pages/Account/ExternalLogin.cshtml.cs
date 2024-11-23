using Chirp.Core.Entities;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace Chirp.Razor.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<Author> _signInManager;
        private readonly UserManager<Author> _userManager;
        private readonly IUserStore<Author> _userStore;
        private readonly IUserEmailStore<Author> _emailStore;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginModel(
            SignInManager<Author> signInManager,
            UserManager<Author> userManager,
            IUserStore<Author> userStore,
            ILogger<ExternalLoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGet() => RedirectToPage("./Login");

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }

            // Get email and username from the external provider
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var username = info.Principal.FindFirstValue(ClaimTypes.Name);

            if (email != null && username != null)
            {
                // Check if the email is already used
                var existingUserByEmail = await _userManager.FindByEmailAsync(email);
                if (existingUserByEmail != null)
                {
                    // Email is already in use
                    ErrorMessage = "The email associated with the github account is already in use. Please log in using your existing account.";
                    return RedirectToPage("./Login");
                }

                // Check if the username is already used
                var existingUserByUsername = await _userManager.FindByNameAsync(username);
                if (existingUserByUsername != null)
                {
                    // Store information in TempData
                    TempData["TemporaryEmail"] = email; 
                    TempData["LoginProvider"] = info.LoginProvider; 
                    TempData["ProviderKey"] = info.ProviderKey; 
                    TempData["ReturnUrl"] = returnUrl;

                    return RedirectToPage("./ExistingUsername");
                }

                // If the username is available, create a new user
                var user = new Author { UserName = username, Email = email };
                var createResult = await _userManager.CreateAsync(user);
                if (createResult.Succeeded)
                {
                    var addLogin = await _userManager.AddLoginAsync(user, info);
                    if (addLogin.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }

                // If user creation failed, show errors
                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        private IUserEmailStore<Author> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<Author>)_userStore;
        }
    }
}
