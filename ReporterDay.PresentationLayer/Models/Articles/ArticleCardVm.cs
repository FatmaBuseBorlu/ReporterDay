using System;

namespace ReporterDay.PresentationLayer.Models.Articles
{
    public sealed class ArticleCardVm
    {
        public string ProtectedId { get; init; } = "";
        public string Title { get; init; } = "";
        public DateTime CreatedDate { get; init; }
        public string CoverImageUrl { get; init; } = "";
        public string AuthorFullName { get; init; } = "";
        public string CategoryName { get; init; } = "";
        public string Preview { get; init; } = "";
    }
}
