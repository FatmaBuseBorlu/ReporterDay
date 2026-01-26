using System.ComponentModel.DataAnnotations;

namespace ReporterDay.PresentationLayer.Models.Components
{
    public sealed class AddCommentRequestViewModel
    {
        [Required]
        public string ProtectedArticleId { get; set; } = "";

        [Required, StringLength(1000, MinimumLength = 2)]
        public string CommentDetail { get; set; } = "";
    }
}
