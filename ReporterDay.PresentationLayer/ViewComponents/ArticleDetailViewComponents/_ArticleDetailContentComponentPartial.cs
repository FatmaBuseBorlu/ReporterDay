using Microsoft.AspNetCore.Mvc;
using ReporterDay.BusinessLayer.Abstract;

namespace ReporterDay.PresentationLayer.ViewComponents.ArticleDetailViewComponents
{
    public class _ArticleDetailContentComponentPartial : ViewComponent
    {
        private readonly IArticleService _articleService;
        public _ArticleDetailContentComponentPartial(IArticleService articleService)
        {
            _articleService = articleService;
        }
        public IViewComponentResult Invoke(int id)
          {
            var value= _articleService.TGetArticlesWithAuthorandCategoriesById(id);
            return View(value);
        }
    }
}
