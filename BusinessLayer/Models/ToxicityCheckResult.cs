

namespace ReporterDay.BusinessLayer.Models
{
    public class ToxicityCheckResult
    {
        public bool IsAvailable { get; set; }
        public bool IsToxic { get; set; }

        public double Score { get; set; }
        public string Label { get; set; } = "unknown";

        public string? ErrorMessage { get; set; }

        public static ToxicityCheckResult Ok(bool isToxic, double score, string label)
        {
            return new ToxicityCheckResult
            {
                IsAvailable = true,
                IsToxic = isToxic,
                Score = score,
                Label = string.IsNullOrWhiteSpace(label) ? "unknown" : label,
                ErrorMessage = null
            };
        }

        public static ToxicityCheckResult NotAvailable(string? errorMessage = null)
        {
            return new ToxicityCheckResult
            {
                IsAvailable = false,
                IsToxic = false,
                Score = 0,
                Label = "not_available",
                ErrorMessage = errorMessage
            };
        }
    }
}