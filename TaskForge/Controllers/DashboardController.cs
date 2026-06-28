using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskForge.Data;
using TaskForge.ViewModels;

namespace TaskForge.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly TaskForgeDbContext _context;

        public DashboardController(TaskForgeDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var model = new DashboardViewModel
            {
                FullName = User.Identity?.Name ?? "User",

                TotalTasks = _context.Tasks.Count(t => t.UserId == userId),

                PendingTasks = _context.Tasks.Count(t =>
                    t.UserId == userId &&
                    t.Status!.StatusName == "Pending"),

                InProgressTasks = _context.Tasks.Count(t =>
                    t.UserId == userId &&
                    t.Status!.StatusName == "In Progress"),

                CompletedTasks = _context.Tasks.Count(t =>
                    t.UserId == userId &&
                    t.Status!.StatusName == "Completed"),

                RecentTasks = _context.Tasks
                    .Where(t => t.UserId == userId)
                    .OrderByDescending(t => t.DateCreated)
                    .Take(5)
                    .Select(t => new TaskListViewModel
                    {
                        TaskId = t.TaskId,
                        Title = t.Title,
                        Category = t.Category!.CategoryName,
                        Priority = t.Priority!.PriorityName,
                        Status = t.Status!.StatusName,
                        DueDate = t.DueDate
                    })
                    .ToList()
            };

            return View(model);
        }
    }
}