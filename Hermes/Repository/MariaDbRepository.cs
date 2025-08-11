using Dapper;
using Hermes.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System.Web;

namespace Hermes.Repository
{
    internal class MariaDbRepository : IArticleRepository
    {
        private readonly string _connectionString;

        public MariaDbRepository(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            _connectionString = connectionString;
        }

        private void CreateArticlesTableIfNotExists()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
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

            using (var connection = new MySqlConnection(_connectionString))
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
					    where article_link = @article_link 
                            or article_title = @article_title);";

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
}
