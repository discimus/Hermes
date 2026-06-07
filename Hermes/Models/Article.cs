using System.Text.Json.Serialization;

namespace Hermes.Models;

public class Article
{
    [JsonPropertyName("article_title")]
    public string Title { get; set; }

    [JsonPropertyName("article_link")]
    public string Link { get; set; }

    [JsonIgnore]
    public string Channel { get; set; }

    [JsonPropertyName("article_content")]
    public string Content { get; set; }

    [JsonPropertyName("article_published_at")]
    public DateTime PublishedAt { get; set; }

    [JsonPropertyName("article_created_at")]
    public DateTime CreatedAt { get; set; }
}