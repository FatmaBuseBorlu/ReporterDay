namespace ReporterDay.BusinessLayer.Models
{
    public sealed record ToxicityCheckResult(
        bool IsToxic,
        double Score,
        string? Label,
        bool IsAvailable,
        string? Error);
}
