using ReporterDay.BusinessLayer.Models;
using System.Threading;
using System.Threading.Tasks;

namespace ReporterDay.BusinessLayer.Abstract
{
    public interface ICommentModerationService
    {
        Task<ModerationDecision> CheckAsync(string text, CancellationToken ct = default);
    }
}
