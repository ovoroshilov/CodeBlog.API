using CodeBlog.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodeBlog.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
