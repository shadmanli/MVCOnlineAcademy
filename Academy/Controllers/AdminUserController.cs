using Academy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class AdminUserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminUserController(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET /api/admin/users
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = new List<object>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new
                {
                    id             = user.Id,
                    fullName       = user.FullName,
                    email          = user.Email,
                    role           = roles.FirstOrDefault() ?? "Yoxdur",
                    emailConfirmed = user.EmailConfirmed
                });
            }
            return Ok(result);
        }

        // PUT /api/admin/users/{id}/role
        [HttpPut("users/{id}/role")]
        public async Task<IActionResult> ChangeRole(string id, [FromBody] ChangeRoleRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.NewRole))
                return BadRequest(new { message = "Yeni rol boş ola bilməz." });

            if (!await _roleManager.RoleExistsAsync(request.NewRole))
                return BadRequest(new { message = $"'{request.NewRole}' rolu mövcud deyil." });

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "İstifadəçi tapılmadı." });

            var currentRoles = await _userManager.GetRolesAsync(user);

            // SuperAdmin-in rolu dəyişdirilə bilməz
            if (currentRoles.Contains("SuperAdmin"))
                return BadRequest(new { message = "SuperAdmin-in rolu dəyişdirilə bilməz." });

            // Köhnə rolları sil
            if (currentRoles.Any())
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // Yeni rolu əlavə et
            await _userManager.AddToRoleAsync(user, request.NewRole);

            // ── Kritik: Security stamp yenilə ──
            // Bu, istifadəçinin mövcud cookie-sini invalidate edir.
            // Növbəti requestdə yeni rol ilə yenidən authenticate olacaq.
            await _userManager.UpdateSecurityStampAsync(user);

            return Ok(new
            {
                message = $"{user.FullName} istifadəçisinin rolu '{request.NewRole}' olaraq dəyişdirildi. İstifadəçi növbəti girişdə yeni rolunu alacaq.",
                userId  = user.Id,
                newRole = request.NewRole,
                canAccessAdmin = request.NewRole == "Admin" || request.NewRole == "SuperAdmin" || request.NewRole == "Muellim"
            });
        }

        // GET /api/admin/roles
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return Ok(roles);
        }
    }

    public class ChangeRoleRequest
    {
        public string NewRole { get; set; } = string.Empty;
    }
}
