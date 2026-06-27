using System.ComponentModel.DataAnnotations;

namespace TaskForge.Models
{
    public class Priority
    {
        [Key]
        public int PriorityId { get; set; }

        [Required]
        [StringLength(20)]
        public string PriorityName { get; set; } = string.Empty;

        // Navigation Property
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}