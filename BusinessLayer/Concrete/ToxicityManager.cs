using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReporterDay.BusinessLayer.Abstract;
using ReporterDay.BusinessLayer.Models;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ReporterDay.BusinessLayer.Concrete
{
    public class ToxicityManager : IToxicityService
    {
        private readonly HttpClient _http;
        private readonly IMemoryCache _cache;
        private readonly HuggingFaceOptions _opt;
        private readonly ILogger<ToxicityManager> _logger;

        public ToxicityManager(
            HttpClient http,
            IMemoryCache cache,
            IOptions<HuggingFaceOptions> options,
            ILogger<ToxicityManager> logger)
        {
            _http = http;
            _cache = cache;
            _opt = options.Value;
            _logger = logger;
        }

        public async Task<ToxicityCheckResult> CheckAsync(string text, CancellationToken ct = default)
        {
            text ??= "";
            var normalized = text.Trim();

            if (string.IsNullOrWhiteSpace(normalized))
                return ToxicityCheckResult.Ok(isToxic: false, score: 0, label: "empty");

            if (string.IsNullOrWhiteSpace(_opt.ApiToken) || _opt.ApiToken.StartsWith("hf_xxxxx", StringComparison.OrdinalIgnoreCase))
                return ToxicityCheckResult.NotAvailable("HuggingFace ApiToken ayarlı değil.");

            var cacheKey = BuildCacheKey(normalized);
            if (_cache.TryGetValue(cacheKey, out ToxicityCheckResult cached))
                return cached;

            try
            {
                var url = $"{_opt.BaseUrl}{_opt.ModelId}";
                using var req = new HttpRequestMessage(HttpMethod.Post, url);

                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _opt.ApiToken);

                var payload = new
                {
                    inputs = normalized,
                    options = new { wait_for_model = true }
                };

                req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                if (_opt.TimeoutSeconds > 0)
                    cts.CancelAfter(TimeSpan.FromSeconds(_opt.TimeoutSeconds));

                var res = await _http.SendAsync(req, cts.Token);
                var json = await res.Content.ReadAsStringAsync(cts.Token);

                if (!res.IsSuccessStatusCode)
                {
                    _logger.LogWarning("HF toxicity failed: {Status} {Body}", (int)res.StatusCode, json);
                    return CacheAndReturn(cacheKey, ToxicityCheckResult.NotAvailable($"HF hata: {(int)res.StatusCode}"));
                }

                if (json.TrimStart().StartsWith("{"))
                {
                    using var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("error", out var err))
                        return CacheAndReturn(cacheKey, ToxicityCheckResult.NotAvailable(err.GetString()));
                }
                var result = ParseHfClassification(json);
                return CacheAndReturn(cacheKey, result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "HF toxicity exception");
                return CacheAndReturn(cacheKey, ToxicityCheckResult.NotAvailable("İstek sırasında hata oluştu."));
            }
        }

        private ToxicityCheckResult CacheAndReturn(string key, ToxicityCheckResult value)
        {
            var minutes = _opt.CacheMinutes <= 0 ? 60 : _opt.CacheMinutes;
            _cache.Set(key, value, TimeSpan.FromMinutes(minutes));
            return value;
        }

        private string BuildCacheKey(string text)
        {
            var raw = $"{_opt.ModelId}|{_opt.ToxicThreshold}|{text}";
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
            return "tox:" + Convert.ToHexString(bytes);
        }

        private ToxicityCheckResult ParseHfClassification(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                JsonElement arr;

                if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 0 && root[0].ValueKind == JsonValueKind.Array)
                    arr = root[0];
                else
                    arr = root;

                if (arr.ValueKind != JsonValueKind.Array || arr.GetArrayLength() == 0)
                    return ToxicityCheckResult.NotAvailable("Model çıktısı beklenmedik formatta.");

                var items = arr.EnumerateArray()
                    .Select(x => new
                    {
                        Label = x.TryGetProperty("label", out var l) ? (l.GetString() ?? "") : "",
                        Score = x.TryGetProperty("score", out var s) ? s.GetDouble() : 0d
                    })
                    .ToList();

                var toxicItem = items.FirstOrDefault(x =>
                    x.Label.Equals("toxic", StringComparison.OrdinalIgnoreCase) ||
                    x.Label.Equals("LABEL_1", StringComparison.OrdinalIgnoreCase));

                string label;
                double score;

                if (toxicItem != null)
                {
                    label = toxicItem.Label;
                    score = toxicItem.Score;
                }
                else
                {
                    var best = items.OrderByDescending(x => x.Score).First();
                    label = best.Label;
                    score = best.Score;
                }

                var isToxic =
                    (label.Equals("toxic", StringComparison.OrdinalIgnoreCase) || label.Equals("LABEL_1", StringComparison.OrdinalIgnoreCase))
                    && score >= _opt.ToxicThreshold;

                return ToxicityCheckResult.Ok(isToxic, score, label);
            }
            catch
            {
                return ToxicityCheckResult.NotAvailable("Model çıktısı parse edilemedi.");
            }
        }
    }
}
