using System.Text.Json.Serialization;

namespace kiwiproject.frameiograbber.Model
{
    public class Project
    {
        public string Id { get; set; }
        public string Name { get; set; }

        [JsonPropertyName("root_asset_id")]
        public string RootAssetId { get; set; }
    }
}