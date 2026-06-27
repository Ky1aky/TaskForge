using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskForge.ViewModels
{
    public class TaskCreateViewModel
    {
        [Required]
        [Display(Name = "Title")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Priority")]
        public int PriorityId { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int StatusId { get; set; }

        [Required]
        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateOnly DueDate { get; set; }
            = DateOnly.FromDateTime(DateTime.Today);

        public List<SelectListItem> Categories { get; set; } = new();

        public List<SelectListItem> Priorities { get; set; } = new();

        public List<SelectListItem> Statuses { get; set; } = new();
    }
}