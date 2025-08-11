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

        IEnumerable<string> links = JsonSerializer.Deserialize<IEnumerable<string>>(content)
            ?? new List<string>();

        IArticleRepository? repository = null;

        bool shouldPersistArticles = false;

        if (!string.IsNullOrEmpty(options.SqlitePath))
        {
            repository = new SqliteRepository(options.SqlitePath);
            shouldPersistArticles = true;
        }
        else if (!string.IsNullOrEmpty(options.MariaDbConnection))
        {
            repository = new MariaDbRepository(options.MariaDbConnection);
            shouldPersistArticles = true;
        }

        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = options.MaxThreadsCount
        };

        Parallel.ForEach(links, parallelOptions, item =>
        {
            var channel = new Channel(item);

            channel.Validate(
                isValid: out bool isValid,
                errorMessage: out string errorMessage);

            if (!isValid)
            {
                Console.WriteLine($"Invalid url: {item}");
                return;
            }

            try
            {
                string encode = string.IsNullOrEmpty(options.Encode)
                    ? "utf-8"
                    : options.Encode;

                IEnumerable<Article> articles = ChannelService.ExtractArticles(
                    channel: channel,
                    encode: encode);

                if (shouldPersistArticles)
                {
                    repository.Insert(articles);
                }
                else
                {
                    Console.WriteLine("===");
                    Console.WriteLine(item);

                    foreach (var article in articles)
                    {
                        Console.WriteLine(article.Title);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        });
    }
}