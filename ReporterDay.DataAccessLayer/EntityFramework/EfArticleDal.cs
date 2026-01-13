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
        public List<Article> GetArticlesByCategoryId1()
        {
            var values = _context.Articles.Where(x => x.CategoryId == 1).ToList();
            return values;
        }

        public List<Article> GetArticlesWithAppUser()
        {
            var values = _context.Articles.ToList();
            return values;
        }

        public Article GetArticlesWithAuthorandCategoriesById(int id)
        {
            var values = _context.Articles.Include(x => x.AppUser).Include(x => x.Category).Where(z => z.ArticleId == id).FirstOrDefault();
            return values;
        }

        public List<Article> GetArticlesWithCategories()
        {
           return _context.Articles.Include(x=>x.Category).ToList();
        }

        public List<Article> GetArticlesWithCategoriesAndAppUsers()
        {
            return _context.Articles.Include(x=> x.Category).Include(x=>x.AppUser).ToList();
        }
    }
}
