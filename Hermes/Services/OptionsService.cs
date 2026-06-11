using Hermes.Exceptions;
using Hermes.Models;
using Hermes.Repository;
using Spectre.Console;
using System.Collections.Concurrent;
using System.Net.Http.Json;
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
        else if (!string.IsNullOrEmpty(options.MssqlConnection))
        {
            repository = new MssqlRepository(options.MssqlConnection);
            shouldPersistArticles = true;
        }

        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = options.MaxThreadsCount
        };

        ConcurrentBag<Article> threadSafeArticles = new ConcurrentBag<Article>();

        Parallel.ForEach(links, parallelOptions, item =>
        {
            var channel = new Channel(item);

            channel.Validate(
                isValid: out bool isValid,
                errorMessage: out string errorMessage);

            if (!isValid)
            {
                if (!options.HideLogs)
                {
                    Console.WriteLine($"Invalid url: {item}");
                }

                return;
            }

            try
            {
                string encode = string.IsNullOrEmpty(options.Encode)
                    ? "utf-8"
                    : options.Encode;

                IEnumerable<Article> articles = ChannelService.ExtractArticles(
                    channel: channel,
                    encode: encode,
                    truncateText: options.TruncateText,
                    hideLogs: options.HideLogs);

                if (shouldPersistArticles)
                {
                    repository.Insert(articles);
                }
                else if (options.OutputJson)
                {
                    foreach (var article in articles)
                    {
                        threadSafeArticles.Add(article);
                    }
                }
                else
                {
                    Console.WriteLine("===");
                    Console.WriteLine(item);

                    foreach (var article in articles)
                    {
                        AnsiConsole.MarkupLine($"[link={article.Link}]{article.Title}[/]");
                    }
                }
            }
            catch (Exception e)
            {
                if (!options.HideLogs)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        });

        if (options.OutputJson)
        {
            IEnumerable<Article> articles = options.RemoveDuplicates
                ? threadSafeArticles.DistinctBy(t => t.Link)
                : threadSafeArticles;

            IEnumerable<Article> ordered = options.Limit.HasValue
                ? articles.OrderByDescending(t => t.PublishedAt).Take(options.Limit.Value)
                : articles.OrderByDescending(t => t.PublishedAt);

            Console.WriteLine(JsonSerializer.Serialize(ordered));
        }
    }
}