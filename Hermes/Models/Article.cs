namespace Hermes.Models;

public class Article
{
    public string Title { get; set; }
    public string Link { get; set; }
    public string Channel { get; set; }
    public string Content { get; set; }
    public DateTime PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}