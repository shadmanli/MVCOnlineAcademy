using Academy.Helpers;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Academy.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<AppUser> userManager,
                                  SignInManager<AppUser> signInManager,
                                  IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity!.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            ViewBag.ReturnUrl = returnUrl;
            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                TempData["LoginError"] = "Email və ya şifrə daxil edilməyib.";
                TempData["OpenLoginModal"] = true;
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["LoginError"] = "Email və ya şifrə yanlışdır.";
                TempData["OpenLoginModal"] = true;
                return RedirectToAction("Index", "Home");
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                TempData["LoginError"] = "Sistemə daxil olmaq üçün əvvəlcə emailinizdəki təsdiq linkini klikləyin.";
                TempData["OpenLoginModal"] = true;
                return RedirectToAction("Index", "Home");
            }

            var result = await _signInManager.PasswordSignInAsync(
                user, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                // ReturnUrl varsa ora yönləndir
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                // Rol əsaslı yönləndirmə
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("SuperAdmin") || roles.Contains("Admin"))
                    return RedirectToAction("Index", "DashBoard", new { area = "Admin" });
                if (roles.Contains("Muellim"))
                    return RedirectToAction("Index", "DashBoard", new { area = "Admin" });

                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                TempData["LoginError"] = "Hesabınız kilidlənib. Zəhmət olmasa bir az sonra təkrar cəhd edin.";
                TempData["OpenLoginModal"] = true;
                return RedirectToAction("Index", "Home");
            }

            TempData["LoginError"] = "Email və ya şifrə yanlışdır.";
            TempData["OpenLoginModal"] = true;
            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();
                TempData["RegisterError"] = errors ?? "Zəhmət olmasa bütün xanaları düzgün doldurun.";
                TempData["OpenRegisterModal"] = true;
                return RedirectToAction("Index", "Home");
            }

            // Email artıq mövcuddursa
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                TempData["RegisterError"] = "Bu email artıq qeydiyyatdadır. Daxil olmağa çalışın.";
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
                // Yeni qeydiyyat → avtomatik "User" rolu
                await _userManager.AddToRoleAsync(user, "User");

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                var confirmationLink = Url.Action("ConfirmEmail", "Account",
                    new { userId = user.Id, token = encodedToken }, Request.Scheme);

                var emailHtmlBody = EmailTemplateHelper.GetConfirmationEmailTemplate(
                    confirmationLink, user.FullName);

                await _emailService.SendEmailAsync(
                    user.Email,
                    "Online Academy - Hesabın Təsdiqlənməsi",
                    emailHtmlBody);

                TempData["RegisterSuccess"] = "Qeydiyyat uğurla tamamlandı! Hesabınızı təsdiqləmək üçün email qutunuzu (Inbox) yoxlayın.";
                TempData["OpenRegisterModal"] = true;
                return RedirectToAction("Index", "Home");
            }

            // Xətaları Azərbaycan dilinə çevir
            var errorMessage = result.Errors.Select(e => e.Code switch
            {
                "PasswordTooShort"       => "Şifrə minimum 6 simvol olmalıdır.",
                "DuplicateUserName"      => "Bu email artıq qeydiyyatdadır.",
                "DuplicateEmail"         => "Bu email artıq qeydiyyatdadır.",
                "InvalidEmail"           => "Email formatı düzgün deyil.",
                _                        => e.Description
            }).FirstOrDefault() ?? "Qeydiyyat zamanı xəta baş verdi.";

            TempData["RegisterError"] = errorMessage;
            TempData["OpenRegisterModal"] = true;
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/ConfirmEmail
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return RedirectToAction("Index", "Home");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                TempData["LoginSuccess"] = "Emailiniz uğurla təsdiqləndi və sistemə avtomatik daxil oldunuz.";
                TempData["OpenLoginModal"] = true;
                return RedirectToAction("Index", "Home");
            }

            TempData["LoginError"] = "Email təsdiqlənməsi uğursuz oldu. Linkin müddəti bitmiş ola bilər.";
            TempData["OpenLoginModal"] = true;
            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            // Bütün session-ları da təmizlə
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            TempData["LoginError"] = "Bu səhifəyə giriş icazəniz yoxdur.";
            TempData["OpenLoginModal"] = true;
            return RedirectToAction("Index", "Home");
        }
    }
}
