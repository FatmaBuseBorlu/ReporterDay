using Microsoft.AspNetCore.DataProtection;
using System;

namespace ReporterDay.PresentationLayer.Security
{
    public class ArticleIdProtector : IArticleIdProtector
    {
        private readonly IDataProtector _protector;

        public ArticleIdProtector(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("ReporterDay.ArticleId.v1");
        }

        public string Protect(int id)
            => _protector.Protect(id.ToString());

        public int Unprotect(string protectedId)
        {
            if (TryUnprotect(protectedId, out var id))
                return id;

            throw new ArgumentException("Geçersiz makale ID.", nameof(protectedId));
        }

        public bool TryUnprotect(string protectedId, out int id)
        {
            id = 0;

            if (string.IsNullOrWhiteSpace(protectedId))
                return false;

            try
            {
                var raw = _protector.Unprotect(protectedId);
                return int.TryParse(raw, out id);
            }
            catch
            {
                return false;
            }
        }
    }
}
