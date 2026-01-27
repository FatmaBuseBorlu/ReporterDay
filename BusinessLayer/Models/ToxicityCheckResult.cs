namespace ReporterDay.BusinessLayer.Models
{
    public class ToxicityCheckResult
    {
        public bool IsAvailable { get; set; }
        public bool IsToxic { get; set; }
        public double Score { get; set; }
        public string Label { get; set; } = "";
        public string? ErrorMessage { get; set; }

        public static ToxicityCheckResult Ok(bool isToxic, double score, string label)
            => new ToxicityCheckResult
            {
                IsAvailable = true,
                IsToxic = isToxic,
                Score = score,
                Label = label ?? ""
            };

        public static ToxicityCheckResult NotAvailable(string message)
            => new ToxicityCheckResult
            {
                IsAvailable = false,
                IsToxic = false,
                Score = 0,
                Label = "",
                ErrorMessage = message
            };
    }
}
