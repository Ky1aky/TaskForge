using System.ComponentModel.DataAnnotations;

namespace TaskForge.Models
{
    public class Status
    {
        [Key]
        public int StatusId { get; set; }

        [Required]
        [StringLength(30)]
        public string StatusName { get; set; } = string.Empty;

        // Navigation Property
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}