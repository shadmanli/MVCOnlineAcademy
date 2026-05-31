using Academy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        // Silinməsi/dəyişdirilməsi qadağan olan əsas rollar
        private static readonly string[] ProtectedRoles = { "SuperAdmin", "Admin", "Muellim", "User" };

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: /Admin/Role
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            var roleData = new List<(IdentityRole Role, int UserCount)>();
            foreach (var role in roles)
            {
                var users = await _userManager.GetUsersInRoleAsync(role.Name!);
                roleData.Add((role, users.Count));
            }

            ViewBag.ProtectedRoles = ProtectedRoles;
            return View(roleData);
        }

        // POST: /Admin/Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                TempData["Error"] = "Rol adı boş ola bilməz.";
                return RedirectToAction(nameof(Index));
            }

            roleName = roleName.Trim();

            if (await _roleManager.RoleExistsAsync(roleName))
            {
                TempData["Error"] = $"'{roleName}' rolu artıq mövcuddur.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (result.Succeeded)
            {
                TempData["Success"] = $"'{roleName}' rolu uğurla yaradıldı.";
            }
            else
            {
                TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/Role/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                TempData["Error"] = "Rol tapılmadı.";
                return RedirectToAction(nameof(Index));
            }

            if (ProtectedRoles.Contains(role.Name, StringComparer.OrdinalIgnoreCase))
            {
                TempData["Error"] = $"'{role.Name}' əsas rol olduğu üçün silinə bilməz.";
                return RedirectToAction(nameof(Index));
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
            if (usersInRole.Any())
            {
                TempData["Error"] = $"'{role.Name}' rolunda {usersInRole.Count} istifadəçi var. Əvvəlcə istifadəçiləri bu roldan çıxarın.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["Success"] = $"'{role.Name}' rolu uğurla silindi.";
            }
            else
            {
                TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/Role/Users — İstifadəçi siyahısı + rol dəyişmə
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();

            var userRoleData = new List<(AppUser User, IList<string> Roles)>();
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                userRoleData.Add((user, userRoles));
            }

            ViewBag.AllRoles = roles.Select(r => r.Name).ToList();
            return View(userRoleData);
        }

        // POST: /Admin/Role/AssignRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "İstifadəçi tapılmadı.";
                return RedirectToAction(nameof(Users));
            }

            if (!await _roleManager.RoleExistsAsync(newRole))
            {
                TempData["Error"] = "Seçilən rol mövcud deyil.";
                return RedirectToAction(nameof(Users));
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var oldRole = currentRoles.FirstOrDefault() ?? "Yoxdur";

            // Köhnə rolları sil
            if (currentRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
            }

            // Yeni rol əlavə et
            var result = await _userManager.AddToRoleAsync(user, newRole);
            if (result.Succeeded)
            {
                // Security stamp yenilə — köhnə cookie invalidate olur,
                // istifadəçi növbəti requestdə yeni rolunu alır
                await _userManager.UpdateSecurityStampAsync(user);

                TempData["Success"] = $"{user.FullName} istifadəçisinin rolu '{oldRole}' → '{newRole}' olaraq dəyişdirildi.";
            }
            else
            {
                TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return RedirectToAction(nameof(Users));
        }
    }
}
