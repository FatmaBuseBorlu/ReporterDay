using Microsoft.EntityFrameworkCore;
using ReporterDay.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReporterDay.DataAccessLayer.Context
{
    public class ArticleContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles {  get; set; }
        public DbSet<Comment>Comments { get; set; }
        public DbSet<Slider>Sliders { get; set; }
        public DbSet<Tag> Tags { get; set; }

    }
}
