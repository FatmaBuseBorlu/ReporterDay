namespace ReporterDay.BusinessLayer.Models
{
    public class HuggingFaceOptions
    {
        public string ApiToken { get; set; } = "";
        public string ModelId { get; set; } = "unitary/toxic-bert";
        public string BaseUrl { get; set; } = "https://api-inference.huggingface.co/models/";

        public double ToxicThreshold { get; set; } = 0.60;
        public int CacheMinutes { get; set; } = 180;
        public int TimeoutSeconds { get; set; } = 10;
    }
}
