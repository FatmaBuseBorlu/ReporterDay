using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReporterDay.EntityLayer.Entities;

namespace ReporterDay.DataAccessLayer.Context
{
    public class ArticleContext : IdentityDbContext<AppUser>
    {
        public ArticleContext(DbContextOptions<ArticleContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}