using System.Text.Json.Serialization;

namespace kiwiproject.frameiograbber.Model
{
    public class Asset
    {
        public string Id { get; set; }
        public string Name { get; set; }

        [JsonPropertyName("item_count")]
        public int ItemCount { get; set; }


        [JsonPropertyName("inserted_at")]
        public DateTime InsertedAt { get; set; }

        [JsonPropertyName("uploaded_at")]
        public DateTime UploadedAt { get; set; }

        [JsonPropertyName("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        public long FileSize { get; set; }

        public string FileType { get; set; }

        public string Status { get; set; }

        public string Original { get; set; }
        public string _path { get; set; }
    }
}