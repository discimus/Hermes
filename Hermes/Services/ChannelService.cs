using System.Text;
using CodeHollow.FeedReader;
using Hermes.Models;

namespace Hermes.Services;

public static class ChannelService
{
    public static IEnumerable<Article> ExtractArticles(
        Channel channel,
        string encode = "utf-8")
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
                        var article = new Article()
                        {
                            Title = item.Title,
                            Link = item.Link,
                            Content = item.Description,
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
                        Console.WriteLine(e);
                        continue;
                    }
                }
            }
        }

        return articles;
    }
}