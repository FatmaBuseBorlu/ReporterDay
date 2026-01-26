using System;

namespace ReporterDay.PresentationLayer.Models.Components
{
    public class RecentArticleListItemViewModel
    {
        public string Title { get; set; } = "";
        public DateTime CreatedDate { get; set; }
        public string CoverImageUrl { get; set; } = "";

        public string ProtectedId { get; set; } = "";

        public string DetailUrl => $"/Article/ArticleDetail/{ProtectedId}";
    }
}
