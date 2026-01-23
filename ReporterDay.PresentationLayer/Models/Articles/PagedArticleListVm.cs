using System.Collections.Generic;

namespace ReporterDay.PresentationLayer.Models.Articles
{
    public sealed class PagedArticleListVm
    {
        public List<ArticleCardVm> Articles { get; init; } = new();

        public int CurrentPage { get; init; }
        public int TotalPages { get; init; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
