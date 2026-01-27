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
        private readonly IArticleIdProtector _idProtector;

        public _ArticleDetailRecentArticlesComponentPartial(
            IArticleService articleService,
            IArticleIdProtector idProtector)
        {
            _articleService = articleService;
            _idProtector = idProtector;
        }

        public IViewComponentResult Invoke(int currentArticleId)
        {
            var values = _articleService.TGetListAll()
                .Where(x => x.ArticleId != currentArticleId)       
                .OrderByDescending(x => x.CreatedDate)
                .Take(5)
                .ToList();

            var vm = values.Select(x => new RecentArticleListItemViewModel
            {
                Title = x.Title ?? "",
                CreatedDate = x.CreatedDate,
                CoverImageUrl = string.IsNullOrWhiteSpace(x.CoverImageUrl)
                    ? Url.Content("~/ZenBlog-1.0.0/assets/img/post-landscape-1.jpg")
                    : x.CoverImageUrl,
                ProtectedId = _idProtector.Protect(x.ArticleId)     
            }).ToList();

            return View(vm);
        }
    }
}
