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
                merge tb_article as t
                using (select 
                    @article_title as article_title,
                    @article_link as article_link,
                    @article_channel as article_channel,
                    @article_content as article_content,
                    @article_published_at as article_published_at,
                    @article_created_at as article_created_at) as s
                on t.article_link = s.article_link or t.article_title = s.article_title
                when not matched then
                insert (article_title, article_link, article_channel, article_content, article_published_at, article_created_at)
                values (s.article_title, s.article_link, s.article_channel, s.article_content, s.article_published_at, s.article_created_at);";
            
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