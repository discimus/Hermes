using CommandLine;

namespace Hermes.Models;

public class Options
{
    [Option('j', "json", Required = true, HelpText = "Path of json array with RSS links")]
    public string Json { get; set; }

    [Option('d', "db-sqlite", Required = false, HelpText = "Path of SQLite DB")]
    public string SqlitePath { get; set; }

    [Option("mariadb-conn", Required = false, HelpText = "MariaDB connection string")]
    public string MariaDbConnection { get; set; }

    [Option("mssql-conn", Required = false, HelpText = "SQL Server connection string")]
    public string MssqlConnection { get; set; }

    [Option('e', "encode", Required = false, HelpText = "Encode para dsserializar retorno")]
    public string Encode { get; set; }

    [Option("max-threads-count", Required = false, HelpText = "Teto de threads que ser�o geradas para consultas HTTP ass�ncronas", Default = 10)]
    public int MaxThreadsCount { get; set; }
}