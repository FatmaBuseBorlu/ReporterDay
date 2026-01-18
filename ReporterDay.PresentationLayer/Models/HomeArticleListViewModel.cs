using ReporterDay.EntityLayer.Entities;

namespace ReporterDay.PresentationLayer.Models
{
    public class HomeArticleListViewModel
    {
        public List<Article> Articles { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
