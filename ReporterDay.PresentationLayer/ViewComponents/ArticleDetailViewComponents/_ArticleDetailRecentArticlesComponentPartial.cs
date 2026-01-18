using Microsoft.AspNetCore.Mvc;
using ReporterDay.BusinessLayer.Abstract;

namespace ReporterDay.PresentationLayer.ViewComponents.ArticleDetailViewComponents
{
    [ViewComponent(Name = "_ArticleDetailRecentArticlesComponentPartial")]
    public class _ArticleDetailRecentArticlesComponentPartial : ViewComponent
    {
        private readonly IArticleService _articleService;

        public _ArticleDetailRecentArticlesComponentPartial(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public IViewComponentResult Invoke()
        {
            var values = _articleService.TGetLastArticles();
            return View(values);
        }
    }
}
