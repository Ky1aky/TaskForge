using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskForge.ViewModels
{
    public class TaskIndexViewModel
    {
        public string? SearchString { get; set; }

        public string? SortBy { get; set; }

        public int? CategoryId { get; set; }

        public int? PriorityId { get; set; }

        public int? StatusId { get; set; }

        public List<SelectListItem> Categories { get; set; } = new();

        public List<SelectListItem> Priorities { get; set; } = new();

        public List<SelectListItem> Statuses { get; set; } = new();

        public List<TaskListViewModel> Tasks { get; set; } = new();
    }
}