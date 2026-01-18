using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using ReporterDay.BusinessLayer.Abstract;
using ReporterDay.PresentationLayer.Helpers;
using ReporterDay.PresentationLayer.Models;

namespace ReporterDay.PresentationLayer.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet("/article/{slug}")]
        public IActionResult ArticleDetail(string slug)
        {
            var article = _articleService.TGetArticleWithAuthorAndCategoryBySlug(slug);
            if (article == null) return NotFound();

            ViewBag.i = article.ArticleId;

            return View("ArticleDetail");
        }

        [HttpGet("/Article/ArticleDetail/{id:int}")]
        public IActionResult ArticleDetailById(int id)
        {
            var article = _articleService.TGetArticlesWithAuthorandCategoriesById(id);
            if (article == null) return NotFound();

            if (string.IsNullOrWhiteSpace(article.Slug))
            {
                article.Slug = SlugHelper.Slugify(article.Title);

                if (string.IsNullOrWhiteSpace(article.Slug))
                    return NotFound();

                _articleService.TUpdate(article);
            }

            return Redirect($"/article/{article.Slug}");
        }

        public IActionResult Index(int page = 1)
        {
            int pageSize = 9;

            int totalCount = _articleService.TGetArticleCount();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            if (totalPages < 1) totalPages = 1;
            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            var articles = _articleService.TGetPagedArticlesWithCategoriesAndAppUsers(page, pageSize);

            var model = new HomeArticleListViewModel
            {
                Articles = articles,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(model);
        }
    }
}
