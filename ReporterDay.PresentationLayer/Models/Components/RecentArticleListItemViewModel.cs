namespace ReporterDay.PresentationLayer.Models.Components
{
    public class RecentArticleListItemViewModel
    {
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }

        public string CoverImageUrl { get; set; } = string.Empty;

        public string DetailUrl { get; set; } = string.Empty;
    }
}
