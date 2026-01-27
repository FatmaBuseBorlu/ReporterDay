using Microsoft.Extensions.Options;
using ReporterDay.BusinessLayer.Abstract;
using ReporterDay.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ReporterDay.BusinessLayer.Concrete
{
    public class CommentModerationManager : ICommentModerationService
    {
        private readonly IToxicityService _toxicityService;
        private readonly CommentModerationOptions _opt;

        private readonly HashSet<string> _blockedSpaced;
        private readonly HashSet<string> _blockedCompact;

        public CommentModerationManager(
            IToxicityService toxicityService,
            IOptions<CommentModerationOptions> options)
        {
            _toxicityService = toxicityService;
            _opt = options.Value;

            var list = _opt.BlockedWords ?? Array.Empty<string>();

            _blockedSpaced = new HashSet<string>(
                list.Select(NormalizeTrSpaced).Where(x => !string.IsNullOrWhiteSpace(x)),
                StringComparer.OrdinalIgnoreCase);

            _blockedCompact = new HashSet<string>(
                list.Select(NormalizeTrCompact).Where(x => !string.IsNullOrWhiteSpace(x)),
                StringComparer.OrdinalIgnoreCase);
        }

        public async Task<ModerationDecision> CheckAsync(string text, CancellationToken ct = default)
        {
            text ??= "";
            var trimmed = text.Trim();

            if (string.IsNullOrWhiteSpace(trimmed))
            {
                return new ModerationDecision
                {
                    Allow = false,
                    Reason = "empty",
                    Message = "Yorum metni boş olamaz."
                };
            }

            var hit = FindBlocked(trimmed);
            if (!string.IsNullOrWhiteSpace(hit))
            {
                return new ModerationDecision
                {
                    Allow = false,
                    Reason = "banned-word",
                    Message = "Yorumunuz uygunsuz içerik içeriyor.",
                    MatchedKeyword = hit
                };
            }

            var model = await _toxicityService.CheckAsync(trimmed, ct);

            if (!model.IsAvailable)
            {
                if (_opt.FailClosed)
                {
                    return new ModerationDecision
                    {
                        Allow = false,
                        Reason = "model-unavailable",
                        Message = "Toksik kontrol şu an yapılamadı. Lütfen tekrar deneyin.",
                        ModelAvailable = false
                    };
                }

                return new ModerationDecision
                {
                    Allow = true,
                    Reason = "model-unavailable",
                    Message = "",
                    ModelAvailable = false
                };
            }

            if (model.IsToxic)
            {
                return new ModerationDecision
                {
                    Allow = false,
                    Reason = "model-toxic",
                    Message = $"Yorumunuz uygunsuz içerik içeriyor. (Skor: {model.Score:0.000})",
                    ModelAvailable = true,
                    ModelToxic = true,
                    ModelScore = model.Score,
                    ModelLabel = model.Label ?? ""
                };
            }

            return new ModerationDecision
            {
                Allow = true,
                Reason = "ok",
                Message = "",
                ModelAvailable = true,
                ModelToxic = false,
                ModelScore = model.Score,
                ModelLabel = model.Label ?? ""
            };
        }

        private string FindBlocked(string input)
        {
            if (_blockedSpaced.Count == 0 && _blockedCompact.Count == 0)
                return "";

            var spaced = " " + NormalizeTrSpaced(input) + " ";

            foreach (var w in _blockedSpaced)
            {
                if (spaced.Contains(" " + w + " ", StringComparison.OrdinalIgnoreCase))
                    return w;
            }

            var compact = NormalizeTrCompact(input);

            foreach (var w in _blockedCompact)
            {
                if (!string.IsNullOrWhiteSpace(w) && compact.Contains(w, StringComparison.OrdinalIgnoreCase))
                    return w;
            }

            return "";
        }

        private static string NormalizeTrSpaced(string input)
        {
            var s = ToLowerTr(input);

            s = ReplaceTrChars(s);

            s = Regex.Replace(s, @"[^\p{L}\p{N}]+", " ");
            s = Regex.Replace(s, @"\s+", " ").Trim();


            s = ReduceRepeats(s);

            return s;
        }

    
        private static string NormalizeTrCompact(string input)
        {
            var spaced = NormalizeTrSpaced(input);
            return spaced.Replace(" ", "");
        }

        private static string ToLowerTr(string input)
        {
          
            return input.ToLower(new CultureInfo("tr-TR"));
        }

        private static string ReplaceTrChars(string s)
        {
            return s.Replace('ç', 'c')
                    .Replace('ğ', 'g')
                    .Replace('ı', 'i')
                    .Replace('ö', 'o')
                    .Replace('ş', 's')
                    .Replace('ü', 'u');
        }

        private static string ReduceRepeats(string s)
        {
            return Regex.Replace(s, @"(\p{L})\1{2,}", "$1$1");
        }
    }
}
