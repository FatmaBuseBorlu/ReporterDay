using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ReporterDay.BusinessLayer.Abstract;
using ReporterDay.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ReporterDay.BusinessLayer.Concrete
{
    public sealed class ToxicityManager : IToxicityService
    {
        private readonly HttpClient _http;
        private readonly IMemoryCache _cache;
        private readonly HuggingFaceOptions _opt;

        private static readonly HashSet<string> ToxicLabels = new(StringComparer.OrdinalIgnoreCase)
        {
            "toxic", "severe_toxic", "obscene", "threat", "insult", "identity_hate"
        };

        public ToxicityManager(HttpClient http, IMemoryCache cache, IOptions<HuggingFaceOptions> options)
        {
            _http = http;
            _cache = cache;
            _opt = options.Value ?? new HuggingFaceOptions();

            if (_opt.TimeoutSeconds > 0)
                _http.Timeout = TimeSpan.FromSeconds(_opt.TimeoutSeconds);
        }

        public async Task<ToxicityCheckResult> CheckAsync(string text, CancellationToken ct = default)
        {
            text ??= "";
            var normalized = text.Trim();

            if (string.IsNullOrWhiteSpace(normalized))
                return new ToxicityCheckResult { IsToxic = false, Score = 0, Label = "empty", IsAvailable = true, Error = null };

            if (string.IsNullOrWhiteSpace(_opt.ApiToken))
                return new ToxicityCheckResult { IsToxic = false, Score = 0, Label = "unavailable", IsAvailable = false, Error = "HuggingFace ApiToken boş." };

            var cacheKey = "tox:" + Sha256(normalized);
            if (_cache.TryGetValue(cacheKey, out ToxicityCheckResult cached))
                return cached;

            try
            {
                var url = BuildModelUrl(_opt.BaseUrl, _opt.ModelId);

                using var req = new HttpRequestMessage(HttpMethod.Post, url);
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _opt.ApiToken);

                var payload = JsonSerializer.Serialize(new
                {
                    inputs = normalized,
                    options = new { wait_for_model = true }
                });

                req.Content = new StringContent(payload, Encoding.UTF8, "application/json");

                using var resp = await _http.SendAsync(req, ct);
                var json = await resp.Content.ReadAsStringAsync(ct);

                if (!resp.IsSuccessStatusCode)
                {
                    var fail = new ToxicityCheckResult
                    {
                        IsToxic = false,
                        Score = 0,
                        Label = "error",
                        IsAvailable = false,
                        Error = $"HF API hata: {(int)resp.StatusCode} - {Truncate(json, 200)}"
                    };

                    CacheSet(cacheKey, fail);
                    return fail;
                }

                var outputs = ParseLabelScores(json);
                if (outputs.Count == 0)
                {
                    var empty = new ToxicityCheckResult
                    {
                        IsToxic = false,
                        Score = 0,
                        Label = "unreadable",
                        IsAvailable = false,
                        Error = "HF yanıtı okunamadı/boş geldi."
                    };

                    CacheSet(cacheKey, empty);
                    return empty;
                }

                var bestToxic = outputs
                    .Where(x => ToxicLabels.Contains(x.Label))
                    .OrderByDescending(x => x.Score)
                    .FirstOrDefault();

                var top1 = outputs.OrderByDescending(x => x.Score).First();

                var isToxic = bestToxic != null && bestToxic.Score >= _opt.ToxicThreshold;

                var result = new ToxicityCheckResult
                {
                    IsToxic = isToxic,
                    Score = isToxic ? bestToxic!.Score : top1.Score,
                    Label = isToxic ? bestToxic!.Label : top1.Label,
                    IsAvailable = true,
                    Error = null
                };

                CacheSet(cacheKey, result);
                return result;
            }
            catch (OperationCanceledException)
            {
                var fail = new ToxicityCheckResult { IsToxic = false, Score = 0, Label = "timeout", IsAvailable = false, Error = "HF isteği zaman aşımı." };
                CacheSet(cacheKey, fail);
                return fail;
            }
            catch (Exception ex)
            {
                var fail = new ToxicityCheckResult { IsToxic = false, Score = 0, Label = "exception", IsAvailable = false, Error = ex.Message };
                CacheSet(cacheKey, fail);
                return fail;
            }
        }

        private sealed record LabelScore(string Label, double Score);

        private static List<LabelScore> ParseLabelScores(string json)
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.ValueKind != JsonValueKind.Array || root.GetArrayLength() == 0)
                return new List<LabelScore>();

            var first = root[0];

            if (first.ValueKind == JsonValueKind.Array)
                return first.EnumerateArray().Select(ParseOne).ToList();

            if (first.ValueKind == JsonValueKind.Object)
                return root.EnumerateArray().Select(ParseOne).ToList();

            return new List<LabelScore>();
        }

        private static LabelScore ParseOne(JsonElement el)
        {
            var label = el.TryGetProperty("label", out var l) ? (l.GetString() ?? "") : "";
            var score = el.TryGetProperty("score", out var s) ? s.GetDouble() : 0;
            return new LabelScore(label, score);
        }

        private void CacheSet(string key, ToxicityCheckResult value)
        {
            var minutes = _opt.CacheMinutes <= 0 ? 60 : _opt.CacheMinutes;
            _cache.Set(key, value, TimeSpan.FromMinutes(minutes));
        }

        private static string BuildModelUrl(string baseUrl, string modelId)
        {
            baseUrl = (baseUrl ?? "").TrimEnd('/');
            modelId ??= "";
            return $"{baseUrl}/{modelId}";
        }

        private static string Sha256(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = SHA256.HashData(bytes);
            return Convert.ToHexString(hash);
        }

        private static string Truncate(string s, int max)
        {
            s ??= "";
            return s.Length <= max ? s : s.Substring(0, max) + "...";
        }
    }
}
