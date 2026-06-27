namespace TaskForge.ViewModels
{
    public class TaskDetailsViewModel
    {
        public int TaskId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string Category { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateOnly DueDate { get; set; }

        public DateTime DateCreated { get; set; }
    }
}