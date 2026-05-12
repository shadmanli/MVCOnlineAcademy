using Academy.Models;
using Academy.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Academy.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _env;

        public ProfileController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            string[] names = user.FullName?.Split(' ');
            string firstName = names != null && names.Length > 0 ? names[0] : "";
            string lastName = names != null && names.Length > 1 ? string.Join(" ", names.Skip(1)) : "";

            ViewBag.User = user;
            ViewBag.FirstName = firstName;
            ViewBag.LastName = lastName;

            return View();
        }

        [HttpPatch]
        [Route("api/user/profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] ProfileUpdateDto model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            user.FullName = $"{model.FirstName} {model.LastName}".Trim();
            user.PhoneNumber = model.Phone;
            user.Bio = model.Bio;

            if (model.ProfilePicture != null)
            {
                if (model.ProfilePicture.Length > 2 * 1024 * 1024)
                    return BadRequest("Şəkil 2MB-dan böyük olmamalıdır.");

                string ex = Path.GetExtension(model.ProfilePicture.FileName);
                if (ex != ".png" && ex != ".jpg" && ex != ".jpeg")
                    return BadRequest("Yalnız PNG, JPG, JPEG formatları qəbul edilir.");

                string folder = Path.Combine(_env.WebRootPath, "uploads", "profile");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + ex;
                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePicture.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(user.ProfilePicture))
                {
                    string oldPath = Path.Combine(folder, user.ProfilePicture);
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }

                user.ProfilePicture = fileName;
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return Ok(new { message = "Profil uğurla yeniləndi.", profilePicture = user.ProfilePicture });

            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route("api/user/change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return Ok(new { message = "Şifrə uğurla yeniləndi." });
            }

            return BadRequest(result.Errors);
        }

        [HttpPatch]
        [Route("api/user/notifications")]
        public async Task<IActionResult> UpdateNotifications([FromBody] NotificationUpdateDto model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            user.NotifyNewLesson = model.NotifyNewLesson;
            user.NotifyDiscounts = model.NotifyDiscounts;
            user.NotifyCertificate = model.NotifyCertificate;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return Ok();

            return BadRequest();
        }

        [HttpDelete]
        [Route("api/user/account")]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountDto model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            if (user.Email != model.Email) return BadRequest("Email doğru deyil.");

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return Ok(new { message = "Hesabınız silindi." });
            }

            return BadRequest();
        }
    }
}

