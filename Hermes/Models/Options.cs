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

    [Option('e', "encode", Required = false, HelpText = "Encode para dsserializar retorno")]
    public string Encode { get; set; }

    [Option("max-threads-count", Required = false, HelpText = "Teto de threads que serão geradas para consultas HTTP assíncronas", Default = 10)]
    public int MaxThreadsCount { get; set; }
}