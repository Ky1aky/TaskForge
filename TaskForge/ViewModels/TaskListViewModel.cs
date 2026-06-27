namespace TaskForge.ViewModels
{
    public class TaskListViewModel
    {
        public int TaskId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateOnly DueDate { get; set; }
    }
}