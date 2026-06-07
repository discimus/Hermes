using CommandLine;

namespace Hermes.Models;

public class Options
{
    [Option('j', "json", Required = true, HelpText = "Path of json array with RSS links")]
    public string Json { get; init; }

    [Option('d', "db-sqlite", Required = false, HelpText = "Path of SQLite DB")]
    public string SqlitePath { get; init; }

    [Option("output-json", Required = false, HelpText = "Deve imprimir output em json")]
    public bool OutputJson { get; init; }

    [Option("hide-logs", Required = false, HelpText = "Deve imprimir logs", Default = false)]
    public bool HideLogs { get; init; }

    [Option("use-parallelism", Required = false, HelpText = "Executar em paralelo", Default = false)]
    public bool UseParallelism { get; init; }

    [Option("mariadb-conn", Required = false, HelpText = "MariaDB connection string")]
    public string MariaDbConnection { get; init; }

    [Option("mssql-conn", Required = false, HelpText = "SQL Server connection string")]
    public string MssqlConnection { get; init; }

    [Option('e', "encode", Required = false, HelpText = "Encode para dsserializar retorno")]
    public string Encode { get; init; }

    [Option("max-threads-count", Required = false, HelpText = "Teto de threads que ser�o geradas para consultas HTTP assincronas", Default = 10)]
    public int MaxThreadsCount { get; init; }

    [Option('l', "limit", Required = false, HelpText = "Limite de artigos", Default = null)]
    public int? Limit { get; init; }
}