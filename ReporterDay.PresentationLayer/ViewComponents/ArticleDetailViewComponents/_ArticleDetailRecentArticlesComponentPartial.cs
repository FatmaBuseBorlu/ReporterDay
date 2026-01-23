using Microsoft.AspNetCore.Mvc;
using ReporterDay.BusinessLayer.Abstract;
using ReporterDay.PresentationLayer.Models.Components;
using ReporterDay.PresentationLayer.Security;
using System.Linq;

namespace ReporterDay.PresentationLayer.ViewComponents.ArticleDetailViewComponents
{
    public class _ArticleDetailRecentArticlesComponentPartial : ViewComponent
    {
        private readonly IArticleService _articleService;
        private readonly IdProtector _idProtector;

        public _ArticleDetailRecentArticlesComponentPartial(
            IArticleService articleService,
            IdProtector idProtector)
        {
            _articleService = articleService;
            _idProtector = idProtector;
        }

        public IViewComponentResult Invoke(int? currentArticleId = null)
        {
            const int take = 5;

            var fallbackImage = Url.Content("~/ZenBlog-1.0.0/assets/img/post-landscape-1.jpg");

            var items = _articleService
                .TGetArticlesWithCategoriesAndAppUsers()
                .Where(x => !currentArticleId.HasValue || x.ArticleId != currentArticleId.Value)
                .OrderByDescending(x => x.CreatedDate)
                .Take(take)
                .Select(x => new RecentArticleListItemViewModel
                {
                    Title = x.Title ?? "",
                    CreatedDate = x.CreatedDate,
                    CoverImageUrl = string.IsNullOrWhiteSpace(x.CoverImageUrl) ? fallbackImage : x.CoverImageUrl,
                    DetailUrl = Url.Action("ArticleDetail", "Article", new { id = _idProtector.Protect(x.ArticleId) }) ?? "#"
                })
                .ToList();

            return View(items);
        }
    }
}
