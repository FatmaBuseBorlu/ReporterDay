using ReporterDay.EntityLayer.Entities;

namespace ReporterDay.DataAccessLayer.Abstract
{
    public interface IArticleDal : IGenericDal<Article>
    {
        List<Article> GetArticlesByCategoryId();
        List<Article> GetArticlesWithAppUser();
        List<Article> GetArticlesWithCategories();
        List<Article> GetArticlesWithCategoriesAndAppUsers();
        Article GetArticlesWithAuthorAndCategoriesById(int id);
        List<Article> GetArticlesByAuthor(string id);
        int GetArticleCount();
        List<Article> GetPagedArticlesWithCategoriesAndAppUsers(int page, int pageSize);

    }
}
