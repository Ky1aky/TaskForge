using Microsoft.EntityFrameworkCore;
using TaskForge.Models;

namespace TaskForge.Data
{
    public class TaskForgeDbContext : DbContext
    {
        public TaskForgeDbContext(DbContextOptions<TaskForgeDbContext> options)
            : base(options)
        {
        }

        // Database Tables
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -------------------------
            // Seed Data
            // -------------------------

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "School" },
                new Category { CategoryId = 2, CategoryName = "Work" },
                new Category { CategoryId = 3, CategoryName = "Personal" },
                new Category { CategoryId = 4, CategoryName = "Project" }
            );

            modelBuilder.Entity<Priority>().HasData(
                new Priority { PriorityId = 1, PriorityName = "Low" },
                new Priority { PriorityId = 2, PriorityName = "Medium" },
                new Priority { PriorityId = 3, PriorityName = "High" }
            );

            modelBuilder.Entity<Status>().HasData(
                new Status { StatusId = 1, StatusName = "Pending" },
                new Status { StatusId = 2, StatusName = "In Progress" },
                new Status { StatusId = 3, StatusName = "Completed" }
            );

            // -------------------------
            // Relationships
            // -------------------------

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Priority)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.PriorityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Status)
                .WithMany(s => s.Tasks)
                .HasForeignKey(t => t.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------------
            // Table Names
            // -------------------------

            modelBuilder.Entity<TaskItem>().ToTable("Tasks");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Priority>().ToTable("Priorities");
            modelBuilder.Entity<Status>().ToTable("Statuses");
        }
    }
}