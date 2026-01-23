using Microsoft.AspNetCore.Mvc;
using ReporterDay.BusinessLayer.Abstract;
using ReporterDay.PresentationLayer.Models;
using ReporterDay.PresentationLayer.Models.Articles;
using ReporterDay.PresentationLayer.Security;
using System;

namespace ReporterDay.PresentationLayer.ViewComponents
{
    public class _ArticleListDefaultComponentPartial : ViewComponent
    {
        private readonly IArticleService _articleService;
        private readonly IdProtector _idProtector;

        public _ArticleListDefaultComponentPartial(IArticleService articleService, IdProtector idProtector)
        {
            _articleService = articleService;
            _idProtector = idProtector;
        }
        public IViewComponentResult Invoke()
        {
            int page = 1;
            var pageQuery = HttpContext.Request.Query["page"].ToString();
            if (!string.IsNullOrWhiteSpace(pageQuery) && int.TryParse(pageQuery, out var parsedPage))
                page = parsedPage;

            const int pageSize = 9;

            int totalCount = _articleService.TGetArticleCount();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            if (totalPages < 1) totalPages = 1;
            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            var articles = _articleService.TGetPagedArticlesWithCategoriesAndAppUsers(page, pageSize);

            var vm = new PagedArticleListVm
            {
                CurrentPage = page,
                TotalPages = totalPages,
                Articles = (articles ?? new())
           .Select(a => new ArticleCardVm
           {
               ProtectedId = _idProtector.Protect(a.ArticleId),
               Title = a.Title ?? "",
               CreatedDate = a.CreatedDate,
               CoverImageUrl = string.IsNullOrWhiteSpace(a.CoverImageUrl)
                   ? Url.Content("~/ZenBlog-1.0.0/assets/img/post-landscape-1.jpg")
                   : a.CoverImageUrl,
               AuthorFullName = $"{a.AppUser?.Name} {a.AppUser?.Surname}".Trim(),
               CategoryName = a.Category?.CategoryName ?? "",
               Preview = BuildPreview(a.Content, 120)
           })
           .ToList()
            };

            return View(vm);
        }
        private static string BuildPreview(string? content, int max)
        {
            if (string.IsNullOrWhiteSpace(content)) return "";
            return content.Length <= max ? content : content.Substring(0, max) + "...";
        }
    }
}
