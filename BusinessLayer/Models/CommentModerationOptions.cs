namespace ReporterDay.BusinessLayer.Models
{
    public sealed class CommentModerationOptions
    {

        public bool FailClosed { get; set; } = false;

  
        public string[] BlockedWords { get; set; } = System.Array.Empty<string>();
    }
}
