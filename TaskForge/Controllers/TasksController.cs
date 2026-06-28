using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskForge.Data;
using TaskForge.Models;
using TaskForge.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace TaskForge.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly TaskForgeDbContext _context;

        public TasksController(TaskForgeDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(
             string? searchString,
             int? categoryId,
             int? priorityId,
             int? statusId,
             string? sortBy)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var query = _context.Tasks
                .Where(t => t.UserId == userId);

            // Search
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(t =>
                    t.Title.Contains(searchString));
            }

            // Category Filter
            if (categoryId.HasValue)
            {
                query = query.Where(t =>
                    t.CategoryId == categoryId.Value);
            }

            // Priority Filter
            if (priorityId.HasValue)
            {
                query = query.Where(t =>
                    t.PriorityId == priorityId.Value);
            }

            // Status Filter
            if (statusId.HasValue)
            {
                query = query.Where(t =>
                    t.StatusId == statusId.Value);
            }

            // Project first
            var taskList = query
                .Select(t => new TaskListViewModel
                {
                    TaskId = t.TaskId,
                    Title = t.Title,
                    Category = t.Category!.CategoryName,
                    Priority = t.Priority!.PriorityName,
                    Status = t.Status!.StatusName,
                    DueDate = t.DueDate
                });

            // Sorting
            taskList = sortBy switch
            {
                "due_desc" => taskList.OrderByDescending(t => t.DueDate),

                "title_asc" => taskList.OrderBy(t => t.Title),

                "title_desc" => taskList.OrderByDescending(t => t.Title),

                "priority" => taskList.OrderByDescending(t =>
                    t.Priority == "High" ? 3 :
                    t.Priority == "Medium" ? 2 : 1),

                _ => taskList.OrderBy(t => t.DueDate)
            };

            var model = new TaskIndexViewModel
            {
                SearchString = searchString,
                SortBy = sortBy,
                CategoryId = categoryId,
                PriorityId = priorityId,
                StatusId = statusId,

                Categories = _context.Categories
                    .Select(c => new SelectListItem
                    {
                        Value = c.CategoryId.ToString(),
                        Text = c.CategoryName
                    })
                    .ToList(),

                Priorities = _context.Priorities
                    .Select(p => new SelectListItem
                    {
                        Value = p.PriorityId.ToString(),
                        Text = p.PriorityName
                    })
                    .ToList(),

                Statuses = _context.Statuses
                    .Select(s => new SelectListItem
                    {
                        Value = s.StatusId.ToString(),
                        Text = s.StatusName
                    })
                    .ToList(),

                Tasks = taskList.ToList()
            };

            return View(model);
        }

        // ==========================
        // GET: Details
        // ==========================

        [HttpGet]
        public IActionResult Details(int id)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var task = _context.Tasks
                .Where(t => t.TaskId == id && t.UserId == userId)
                .Select(t => new TaskDetailsViewModel
                {
                    TaskId = t.TaskId,
                    Title = t.Title,
                    Description = t.Description,
                    Category = t.Category!.CategoryName,
                    Priority = t.Priority!.PriorityName,
                    Status = t.Status!.StatusName,
                    DueDate = t.DueDate,
                    DateCreated = t.DateCreated
                })
                .FirstOrDefault();

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // ==========================
        // GET: Create
        // ==========================

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new TaskCreateViewModel();

            LoadDropdowns(viewModel);

            return View(viewModel);
        }

        // ==========================
        // POST: Create
        // ==========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaskCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns(model);
                return View(model);
            }

            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var task = new TaskItem
            {
                UserId = userId,
                Title = model.Title,
                Description = model.Description,
                CategoryId = model.CategoryId,
                PriorityId = model.PriorityId,
                StatusId = model.StatusId,
                DueDate = model.DueDate,
                DateCreated = DateTime.Now
            };

            _context.Tasks.Add(task);

            _context.SaveChanges();

            TempData["Success"] = "Task created successfully.";

            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // Helper Method
        // ==========================

    

        // ==========================
        // Get Edit
        // ==========================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var task = _context.Tasks
                .FirstOrDefault(t => t.TaskId == id && t.UserId == userId);

            if (task == null)
            {
                return NotFound();
            }

            var model = new TaskEditViewModel
            {
                TaskId = task.TaskId,
                Title = task.Title,
                Description = task.Description,
                CategoryId = task.CategoryId,
                PriorityId = task.PriorityId,
                StatusId = task.StatusId,
                DueDate = task.DueDate
            };

            LoadDropdowns(model);

            return View(model);
        }

        // ==========================
        // Post Edit
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns(model);
                return View(model);
            }

            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.TaskId == id && t.UserId == userId);

            if (task == null)
            {
                return NotFound();
            }

            // Update fields
            task.Title = model.Title;
            task.Description = model.Description;
            task.CategoryId = model.CategoryId;
            task.PriorityId = model.PriorityId;
            task.StatusId = model.StatusId;
            task.DueDate = model.DueDate;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Task updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // Get Delete
        // ==========================
        [HttpGet]
        public IActionResult Delete(int id)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var task = _context.Tasks
                .Where(t => t.TaskId == id && t.UserId == userId)
                .Select(t => new TaskDetailsViewModel
                {
                    TaskId = t.TaskId,
                    Title = t.Title,
                    Description = t.Description,
                    Category = t.Category!.CategoryName,
                    Priority = t.Priority!.PriorityName,
                    Status = t.Status!.StatusName,
                    DueDate = t.DueDate,
                    DateCreated = t.DateCreated
                })
                .FirstOrDefault();

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // ==========================
        // Post Delete
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var task = _context.Tasks
                .FirstOrDefault(t => t.TaskId == id && t.UserId == userId);

            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            TempData["Success"] = "Task deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns(dynamic model)
        {
            model.Categories = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                })
                .ToList();

            model.Priorities = _context.Priorities
                .Select(p => new SelectListItem
                {
                    Value = p.PriorityId.ToString(),
                    Text = p.PriorityName
                })
                .ToList();

            model.Statuses = _context.Statuses
                .Select(s => new SelectListItem
                {
                    Value = s.StatusId.ToString(),
                    Text = s.StatusName
                })
                .ToList();
        }
    }
}