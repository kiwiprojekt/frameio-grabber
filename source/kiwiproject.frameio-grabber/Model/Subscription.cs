using System.Text.Json.Serialization;

namespace kiwiproject.frameiograbber.Model
{
    public class Subscription
    {
        [JsonPropertyName("total_storage_limit")]
        public long TotalStorageLimit { get; set; }
    }
}