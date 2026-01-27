namespace ReporterDay.PresentationLayer.Models.Components
{
    public class CommentListItemViewModel
    {
        public int CommentId { get; set; }
        public string CommentDetail { get; set; } = "";
        public DateTime CommentDate { get; set; }
        public string UserFullName { get; set; } = "Anonim";

        public bool IsCheckAvailable { get; set; }
        public bool IsToxic { get; set; }
        public double ToxicityScore { get; set; }
        public string ToxicLabel { get; set; } = "";
        public string ErrorMessage { get; set; } = "";
    }
}
