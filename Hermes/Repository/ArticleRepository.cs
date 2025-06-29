using System.Web;
using Dapper;
using Hermes.Models;
using Microsoft.Data.Sqlite;

namespace Hermes.Repository;

public class ArticleRepository
{
    private string _connectionString;

    public ArticleRepository(string dbPath)
    {
	    if (!string.IsNullOrEmpty(dbPath)
	        && !File.Exists(dbPath))
	    {
		    File.Create(dbPath).Close();
	    }
	    
        _connectionString = $"Data Source={dbPath};Mode=ReadWriteCreate;Cache=Shared;";
    }

    private void CreateArticlesTableIfNotExists()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
	        connection.Execute("PRAGMA journal_mode=WAL;");

            string query = @"
                create table if not exists tb_article(
	                article_id integer PRIMARY key,
	                article_title varchar(500),
	                article_link varchar(500),
	                article_channel varchar(255),
	                article_content text,
	                article_published_at text,
	                article_created_at text);";
            
            connection.Execute(query);
        }
    }
    
    public void Insert(IEnumerable<Article> articles)
    {
        CreateArticlesTableIfNotExists();

        using (var connection = new SqliteConnection(_connectionString))
        {
            string query = @"
				insert into tb_article(
					article_title,
					article_link,
					article_channel,
					article_content,
					article_published_at,
					article_created_at)
				select 
					@article_title,
					@article_link,
					@article_channel,
					@article_content,
					@article_published_at,
					@article_created_at
				where not exists (
					select 1
						from tb_article
							where article_link = @article_link);";

            IEnumerable<object> items = articles
	            .Select(t =>
	            {
		            return new
		            {
			            article_title = HttpUtility.HtmlEncode(t.Title),
			            article_link = t.Link,
			            article_channel = t.Channel,
			            article_content = HttpUtility.HtmlEncode(t.Content),
			            article_published_at = t.PublishedAt.ToString("yyyy-MM-dd HH:mm:ss"),
			            article_created_at = t.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
		            };
	            });
            
            connection.Execute(query, items);
        }
    }
}