using ReporterDay.BusinessLayer.Abstract;
using ReporterDay.DataAccessLayer.Abstract;
using ReporterDay.DataAccessLayer.EntityFramework;
using ReporterDay.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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

        public int TGetArticleCount()
        {
            return _articleDal.GetArticleCount();
        }

        public List<Article> TGetArticlesByAuthor(string id)
        {
            return _articleDal.GetArticlesByAuthor(id);
        }
        public List<Article> TGetArticlesWithAppUser()
        {
            return _articleDal.GetArticlesWithAppUser();
        }

        public Article TGetArticlesWithAuthorandCategoriesById(int id)
        {
            return _articleDal.GetArticlesWithAuthorAndCategoriesById(id);
        }

        public List<Article> TGetArticlesWithCategories()
        {
            return _articleDal.GetArticlesWithCategories();
        }

        public List<Article> TGetArticlesWithCategoriesAndAppUsers()
        {
            return _articleDal.GetArticlesWithCategoriesAndAppUsers();
        }

        public Article TGetArticleWithAuthorAndCategoryBySlug(string slug)
        {
            return _articleDal.GetArticleWithAuthorAndCategoryBySlug(slug);
        }

        public Article TGetById(int id)
        {
            return _articleDal.GetById(id);
        }
        public List<Article> TGetListAll()
        {
            return _articleDal.GetListAll();
        }

        public List<Article> TGetPagedArticlesWithCategoriesAndAppUsers(int page, int pageSize)
        {
            return _articleDal.GetPagedArticlesWithCategoriesAndAppUsers(page, pageSize);
        }


private static string Slugify(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return "";

        text = text.Trim().ToLowerInvariant();

        text = text
            .Replace("ç", "c")
            .Replace("ğ", "g")
            .Replace("ı", "i")
            .Replace("ö", "o")
            .Replace("ş", "s")
            .Replace("ü", "u");

        text = Regex.Replace(text, @"[^a-z0-9\s-]", "");
        text = Regex.Replace(text, @"\s+", " ").Trim();
        text = text.Replace(" ", "-");
        text = Regex.Replace(text, @"-+", "-");

        return text;
    }

    private string GenerateUniqueSlug(string title)
    {
        var baseSlug = Slugify(title);
        if (string.IsNullOrWhiteSpace(baseSlug)) baseSlug = Guid.NewGuid().ToString("n");

        var slug = baseSlug;
        var i = 2;

        while (_articleDal.SlugExists(slug))
        {
            slug = $"{baseSlug}-{i}";
            i++;
        }

        return slug;
    }

    public void TInsert(Article entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Slug))
                entity.Slug = GenerateUniqueSlug(entity.Title);
            if (entity.Title != null && entity.Title.Length > 10 && entity.CategoryId != 0 && entity.Content.Length <= 1000)
            {
                _articleDal.Insert(entity);
            }
            else
            {
                //hata mesajı
            }
        }

        public bool TSlugExists(string slug)
        {
            return _articleDal.SlugExists(slug);
        }

        public void TUpdate(Article entity)
        {
            _articleDal.Update(entity);
        }

        public List<Article> TGetLastArticles()
        {
            return _articleDal.GetLastArticles();
        }
    }
}
