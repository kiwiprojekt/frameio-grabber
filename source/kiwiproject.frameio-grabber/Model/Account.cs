using System.Text.Json.Serialization;

namespace kiwiproject.frameiograbber.Model
{
    public class Account
    {
        public string Id { get; set; }

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        public long Storage { get; set; }

        public Subscription Subscription { get; set; }
    }
}