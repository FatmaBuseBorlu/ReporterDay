using Microsoft.AspNetCore.DataProtection;
using System.Globalization;

namespace ReporterDay.PresentationLayer.Security
{
    public sealed class ArticleIdProtector : IdProtector
    {
        private const string Purpose = "ReporterDay.ArticleId.v1";
        private readonly IDataProtector _protector;

        public ArticleIdProtector(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector(Purpose);
        }

        public string Protect(int id)
        {
            return _protector.Protect(id.ToString(CultureInfo.InvariantCulture));
        }

        public bool TryUnprotect(string protectedId, out int id)
        {
            id = default;

            if (string.IsNullOrWhiteSpace(protectedId))
                return false;

            try
            {
                var raw = _protector.Unprotect(protectedId);
                return int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out id);
            }
            catch
            {
                return false;
            }
        }
    }
}
