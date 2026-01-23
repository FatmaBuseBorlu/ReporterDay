using ReporterDay.BusinessLayer.Models;
using System.Threading;
using System.Threading.Tasks;

namespace ReporterDay.BusinessLayer.Abstract
{
    public interface IToxicityService
    {
        Task<ToxicityCheckResult> CheckAsync(string text, CancellationToken ct = default);
    }
}
