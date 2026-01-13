using ReporterDay.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReporterDay.DataAccessLayer.Abstract
{
    public interface IArticleDal : IGenericDal<Article>
    {
        List<Article> GetArticlesByCategoryId1();
        List<Article> GetArticlesWithAppUser();
        List<Article> GetArticlesWithCategories();
        List<Article> GetArticlesWithCategoriesAndAppUsers();
        Article  GetArticlesWithAuthorandCategoriesById(int id);
    }
}
