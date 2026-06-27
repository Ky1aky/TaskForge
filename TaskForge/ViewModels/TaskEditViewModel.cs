using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskForge.ViewModels
{
    public class TaskEditViewModel
    {
        public int TaskId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int CategoryId { get; set; }

        public int PriorityId { get; set; }

        public int StatusId { get; set; }

        public DateOnly DueDate { get; set; }

        public List<SelectListItem> Categories { get; set; } = new();

        public List<SelectListItem> Priorities { get; set; } = new();

        public List<SelectListItem> Statuses { get; set; } = new();
    }
}