using System.Data.SqlClient;
using System.Web;
using Dapper;
using Hermes.Models;
using Microsoft.Data.SqlClient;

namespace Hermes.Repository;

public class MssqlRepository : IArticleRepository
{
    private readonly string _connectionString;

    public MssqlRepository(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        _connectionString = connectionString;
    }

    public void Insert(IEnumerable<Article> articles)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string query = @"
				INSERT INTO tb_article (
				    article_title,
				    article_link,
				    article_channel,
				    article_content,
				    article_published_at,
				    article_created_at)
				VALUES (
					@article_title,
					@article_link,
					@article_channel,
					@article_content,
					@article_published_at,
					@article_created_at)";
            
            connection.Execute(query, articles.Select(t => new
            {
	            article_title = HttpUtility.HtmlEncode(t.Title),
	            article_link = t.Link,
	            article_channel = t.Channel,
	            article_content = HttpUtility.HtmlEncode(t.Content),
	            article_published_at = t.PublishedAt,
	            article_created_at = t.CreatedAt,
            }));
        }
    }
}