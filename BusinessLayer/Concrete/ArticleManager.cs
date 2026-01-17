using ReporterDay.BusinessLayer.Abstract;
using ReporterDay.DataAccessLayer.Abstract;
using ReporterDay.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReporterDay.BusinessLayer.Concrete
{
    public class ArticleManager : IArticleService
    {
        private readonly IArticleDal _articleDal;
        public ArticleManager(IArticleDal articleDal)
        {
            _articleDal = articleDal;
        }
        public void TDelete(int id)
        {
            _articleDal.Delete(id);
        }

        public List<Article> TGetArticleByCategoryId()
        {
            return _articleDal.GetArticlesByCategoryId();
        }

        public List<Article> TGetArticlesByAuthor(string id)
        {
            return _articleDal.GetArticlesByAuthor(id);
        }
        public List<Article> TGetArticlesWithAppUser()
        {
            return _articleDal.GetArticlesWithAppUser();
        }

        public Article TGetArticlesWithAuthorAndCategoriesById(int id)
        {
            return _articleDal.GetArticlesWithAuthorAndCategoriesById(id);
        }

        public Article TGetArticlesWithAuthorandCategoriesById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Article> TGetArticlesWithCategories()
        {
            return _articleDal.GetArticlesWithCategories();
        }

        public List<Article> TGetArticlesWithCategoriesAndAppUsers()
        {
            return _articleDal.GetArticlesWithCategoriesAndAppUsers();
        }

        public Article TGetById(int id)
        {
            return _articleDal.GetById(id);
        }
        public List<Article> TGetListAll()
        {
            return _articleDal.GetListAll();
        }
        public void TInsert(Article entity)
        {
            if (entity.Title != null && entity.Title.Length > 10 && entity.CategoryId != 0 && entity.Content.Length <= 1000)
            {
                _articleDal.Insert(entity);
            }
            else
            {
                //hata mesajı
            }
        }

        public void TUpdate(Article entity)
        {
            _articleDal.Update(entity);
        }
    }
}
