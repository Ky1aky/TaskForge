using Microsoft.EntityFrameworkCore;

namespace TaskForge.Data
{
    public class TaskForgeDbContext : DbContext
    {
        public TaskForgeDbContext(DbContextOptions<TaskForgeDbContext> options)
            : base(options)
        {
        }
    }
}