using Microsoft.AspNetCore.Mvc;
using ReporterDay.BusinessLayer.Abstract;

namespace ReporterDay.PresentationLayer.ViewComponents
{
    public class _ArticleListDefaultComponentPartial : ViewComponent
    {
        private readonly IArticleService _articleService;
        public _ArticleListDefaultComponentPartial(IArticleService articleService)
        {
            _articleService = articleService;
        }
        public IViewComponentResult Invoke()
        {
            var values = _articleService.TGetArticlesWithAppUser();
            return View(values);
        }
    }
}

