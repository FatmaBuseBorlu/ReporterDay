namespace ReporterDay.BusinessLayer.Models
{
    public class ToxicityCheckResult
    {
        public bool IsToxic { get; set; }
        public double Score { get; set; }
        public string? Label { get; set; }
        public bool IsAvailable { get; set; }
        public string? Error { get; set; }
    }
}
