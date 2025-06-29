using Hermes.Exceptions;
using Hermes.Models;
using Hermes.Repository;
using System.Text.Json;

namespace Hermes.Services;

public static class OptionsService
{
    public static void Handle(Options options)
    {
        OptionsValidationService.Validate(options);

        string content = File.ReadAllText(options.Json);

        if (string.IsNullOrEmpty(content))
            throw new EmptyJsonFileException("Empty json file.");

        IEnumerable<string> links = JsonSerializer.Deserialize<IEnumerable<string>>(content);

        var repository = new ArticleRepository(options.Db);

        foreach (var item in links)
        {
            var channel = new Channel(item);

            channel.Validate(
                isValid: out bool isValid,
                errorMessage: out string errorMessage);

            if (!isValid)
            {
                Console.WriteLine($"Invalid url: {item}");
                continue;
            }

            try
            {
                string encode = string.IsNullOrEmpty(options.Encode)
                    ? "utf-8"
                    : options.Encode;

                IEnumerable<Article> articles = ChannelService.ExtractArticles(
                    channel: channel,
                    encode: encode);

                if (String.IsNullOrEmpty(options.Db))
                {
                    Console.WriteLine("===");
                    Console.WriteLine(item);

                    foreach (var article in articles)
                    {
                        Console.WriteLine(article.Title);
                    }
                }
                else
                {
                    repository.Insert(articles);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}