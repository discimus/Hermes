using System.Text;
using CodeHollow.FeedReader;
using Hermes.Models;

namespace Hermes.Services;

public static class ChannelService
{
    public static IEnumerable<Article> ExtractArticles(
        Channel channel,
        string encode = "utf-8",
        bool truncateText = false,
        bool hideLogs = false)
    {
        var articles = new List<Article>();

        using (var httpClient = new HttpClient() { Timeout = TimeSpan.FromMinutes(5)})
        {
            var request = new HttpRequestMessage(
                method: HttpMethod.Get,
                requestUri: new Uri(channel.Url));
            
            HttpResponseMessage response = httpClient.Send(request);

            if (response.IsSuccessStatusCode)
            {
                byte[] originalBytes = response.Content.ReadAsByteArrayAsync().Result;

                string content = Encoding.GetEncoding(encode).GetString(originalBytes);
                
                Feed feed = FeedReader.ReadFromString(content);

                foreach (var item in feed.Items)
                {
                    try
                    {
                        string description = item.Description ?? "";

                        var article = new Article()
                        {
                            Title = item.Title,
                            Link = item.Link,
                            Content = truncateText
                                ? description.Substring(0, Math.Min(description.Length, 500))
                                : description,
                            Channel = channel.Url,
                            PublishedAt = item.PublishingDate.HasValue
                                ?  item.PublishingDate.Value
                                : DateTime.Now,
                            CreatedAt = DateTime.Now,
                        };
            
                        articles.Add(article);
                    }
                    catch (Exception e)
                    {
                        if (!hideLogs)
                            Console.WriteLine(e);
    
                        continue;
                    }
                }
            }
        }

        return articles;
    }
}