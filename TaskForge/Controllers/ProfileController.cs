using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskForge.Data;
using TaskForge.ViewModels;
using TaskForge.Services;

namespace TaskForge.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly TaskForgeDbContext _context;
        private readonly UserService _userService;

        public ProfileController(
            TaskForgeDbContext context,
            UserService userService)
        {
            _context = context;
            _userService = userService;
        }
        public IActionResult Index()
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var user = _context.Users
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            var model = new ProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                DateCreated = user.DateCreated
            };

            return View(model);
        }

        // ==========================
        // GET: Edit Profile
        // ==========================

        [HttpGet]
        public IActionResult Edit()
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var user = _context.Users
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            var model = new ProfileEditViewModel
            {
                FullName = user.FullName,
                Email = user.Email
            };

            return View(model);
        }

        // ==========================
        // POST: Edit Profile
        // ==========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProfileEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var user = _context.Users
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            // Prevent duplicate email addresses
            bool emailExists = _context.Users.Any(u =>
                u.Email == model.Email &&
                u.UserId != userId);

            if (emailExists)
            {
                ModelState.AddModelError("Email", "That email address is already in use.");
                return View(model);
            }

            user.FullName = model.FullName;
            user.Email = model.Email;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // GET: Change Password
        // ==========================

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // ==========================
        // POST: Change Password
        // ==========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            bool success = await _userService.ChangePasswordAsync(
                userId,
                model.CurrentPassword,
                model.NewPassword);

            if (!success)
            {
                ModelState.AddModelError(
                    "CurrentPassword",
                    "Current password is incorrect.");

                return View(model);
            }

            TempData["Success"] = "Password changed successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}