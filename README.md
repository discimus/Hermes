Example of use:
```
dotnet run -- -j "/path/to/json/with/rss/feeds/array" -e "windows-1252" -d "/path/to/sqlite/file"
```

Params:

-j or --json to set the path to a json file with an array of rss feeds addres

```
[
  "www.example-one/rss",
  "www.example-two/rss"
]
```

-e or --encode to set the encode like "utf-8" or "windows-1252"

-d or --db to set the path for the sqlitefile to store the articles


Without -e the encode is utf-8 by default

Without -d the articles will be listed at the console

The table is
tb_article

And has the structure bellow:

```
create table if not exists tb_article(
	article_id integer PRIMARY key,
	article_title varchar(500),
	article_link varchar(500),
	article_channel varchar(255),
	article_content text,
	article_published_at text,
	article_created_at text);
```
