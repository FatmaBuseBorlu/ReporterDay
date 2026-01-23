using System;

namespace ReporterDay.PresentationLayer.Models.Components
{
    public sealed class CommentListItemViewModel
    {
        public int CommentId { get; set; }
        public string CommentDetail { get; set; } = "";
        public DateTime CommentDate { get; set; }
        public string UserFullName { get; set; } = "Anonim";

        public bool IsToxic { get; set; }
        public double ToxicityScore { get; set; }
        public string? ToxicLabel { get; set; }
        public bool IsCheckAvailable { get; set; } = true;
    }
}
