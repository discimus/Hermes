using CommandLine;

namespace Hermes.Models;

public class Options
{
    [Option('j', "json", Required = true, HelpText = "Path of json array with RSS links")]
    public string Json { get; set; }
    
    [Option('d', "db", Required = false, HelpText = "Path of SQLite DB")]
    public string Db { get; set; }
    
    [Option('e', "encode", Required = false, HelpText = "Encode para dsserializar retorno")]
    public string Encode { get; set; }
}