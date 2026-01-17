using Microsoft.EntityFrameworkCore;
using ReporterDay.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReporterDay.BusinessLayer.Abstract
{
    public interface IArticleService : IGenericService<Article>
    {
        public List<Article> TGetArticleByCategoryId();
        public List<Article> TGetArticlesWithAppUser();
        public List<Article> TGetArticlesWithCategories();
        public List<Article> TGetArticlesWithCategoriesAndAppUsers();
        public Article TGetArticlesWithAuthorandCategoriesById(int id);
        List<Article> TGetArticlesByAuthor(string id);
    }
}
