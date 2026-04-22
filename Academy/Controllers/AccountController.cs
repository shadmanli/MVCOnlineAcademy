using Academy.Helpers;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Academy.Helpers;

namespace Academy.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["RegisterError"] = "Z?hm?t olmasa bütün xanalar? düzgün doldurun.";
                TempData["OpenRegisterModal"] = true;
                return RedirectToAction("Index", "Home");
            }

            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                
                // Encode the token for safe URL transmission
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var confirmationLink = Url.Action("ConfirmEmail", "Account",
                    new { userId = user.Id, token = encodedToken }, Request.Scheme);

                var emailHtmlBody = EmailTemplateHelper.GetConfirmationEmailTemplate(confirmationLink, user.FullName);

                await _emailService.SendEmailAsync(user.Email, "Online Academy - Hesabin Tesdiqlenmesi", emailHtmlBody);

                TempData["RegisterSuccess"] = "Qeydiyyat ugurla tamamlandi! Hesabinizi tesdiqlemek ucun zehmet olmasa email qutunuzu (Inbox) yoxlayin.";
                TempData["OpenRegisterModal"] = true;
                return RedirectToAction("Index", "Home");
            }

            // X?talar?n siyah?s?n? düz?ltm?k
            var errorMessages = string.Join(" | ", result.Errors.Select(e => e.Description));
            
            TempData["RegisterError"] = errorMessages;
            TempData["OpenRegisterModal"] = true;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null) return RedirectToAction("Index", "Home");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            // Decode the token before consuming it
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                TempData["SuccessMessage"] = "Emailiniz ugurla tesdiqlendi ve sisteme avtomatik daxil oldunuz.";
                return RedirectToAction("Index", "Home");
            }

            TempData["ErrorMessage"] = "Email confirmation failed.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["LoginError"] = "Email v? ya ?ifr? daxil edilm?yib.";
                TempData["OpenLoginModal"] = true;
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["LoginError"] = "Email v? ya ?ifr? yanl??d?r.";
                TempData["OpenLoginModal"] = true;
                return RedirectToAction("Index", "Home");
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                TempData["LoginError"] = "Sistem? daxil olmaq üçün ilk önc? emailiniz? g?l?n t?sdiq linkin? klikl?yin.";
                TempData["OpenLoginModal"] = true;
                return RedirectToAction("Index", "Home");
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                TempData["LoginError"] = "Hesab?n?z kilidl?nib. Z?hm?t olmasa bir az sonra t?krar c?hd edin.";
                TempData["OpenLoginModal"] = true;
                return RedirectToAction("Index", "Home");
            }

            TempData["LoginError"] = "Email v? ya ?ifr? yanl??d?r.";
            TempData["OpenLoginModal"] = true;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}