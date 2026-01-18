using Microsoft.AspNetCore.Mvc;
using ReporterDay.BusinessLayer.Abstract;
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

        public IActionResult ArticleDetail(int id)
        {
            ViewBag.i = id;
            return View();
        }
    }
}
