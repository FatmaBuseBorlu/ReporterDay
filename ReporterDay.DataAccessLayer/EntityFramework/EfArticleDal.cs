using ReporterDay.DataAccessLayer.Repositories;
using ReporterDay.DataAccessLayer.Abstract;
using ReporterDay.DataAccessLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReporterDay.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace ReporterDay.DataAccessLayer.EntityFramework
{
    public class EfArticleDal : GenericRepository<Article>, IArticleDal
    {
        private readonly ArticleContext _context;
        public EfArticleDal(ArticleContext context) : base(context)
        {
            _context = context;
        }

        public List<Article> GetArticleByCategoryId()
        {
            var values = _context.Articles.Where(x => x.CategoryId == 1).ToList();
            return values;
        }
        public List<Article> GetArticlesWithAppUser()
        {
            var values = _context.Articles.ToList();
            return values;
        }
    }
}
