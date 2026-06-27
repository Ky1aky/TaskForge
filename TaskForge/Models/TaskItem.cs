using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskForge.Models
{
    public class TaskItem
    {
        [Key]
        public int TaskId { get; set; }

        // Foreign Keys
        [Required]
        public int UserId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int PriorityId { get; set; }

        [Required]
        public int StatusId { get; set; }

        // Task Information
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public DateOnly DueDate { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        [ForeignKey("PriorityId")]
        public Priority? Priority { get; set; }

        [ForeignKey("StatusId")]
        public Status? Status { get; set; }
    }
}