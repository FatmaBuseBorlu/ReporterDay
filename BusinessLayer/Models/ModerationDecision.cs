namespace ReporterDay.BusinessLayer.Models
{
    public sealed class ModerationDecision
    {
        public bool Allow { get; init; }
        public string Reason { get; init; } = "";         
        public string Message { get; init; } = "";     
        public bool ModelAvailable { get; init; }
        public bool ModelToxic { get; init; }
        public double ModelScore { get; init; }
        public string ModelLabel { get; init; } = "";
        public string MatchedKeyword { get; init; } = "";  
    }
}
