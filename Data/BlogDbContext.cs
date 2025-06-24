using Microsoft.EntityFrameworkCore;
using Blog.Models;

namespace Blog.Data
{
    public class BlogDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public BlogDbContext(Microsoft.EntityFrameworkCore.DbContextOptions<BlogDbContext> options)
            : base(options)
        {
           
            Database.EnsureCreated();

        }
        public Microsoft.EntityFrameworkCore.DbSet<BlogModel> Blogs { get; set; }
        protected override void OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogModel>().ToTable("Blogs");
            modelBuilder.Entity<BlogModel>().HasData(
                new BlogModel
                {
                    Id = 1,
                    Title = "Первый пост",
                    Content = "Это содержимое первого поста.",
                    Author = "Иван Иванов",
                    PublishedDate = new DateTime(2024, 6, 1)
                },
                new BlogModel
                {
                    Id = 2,
                    Title = "Второй пост",
                    Content = "Это содержимое второго поста.",
                    Author = "Мария Петрова",
                    PublishedDate = new DateTime(2024, 6, 2)
                }
            );
           
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=blog.db");
            }
        }
    }
}
