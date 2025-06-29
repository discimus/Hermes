using System.Globalization;
using System.Text;
using CommandLine;
using Hermes.Models;
using Hermes.Services;

namespace Hermes;

class Program
{
    static void Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(t =>
            {
                try
                {
                    OptionsService.Handle(t);
                }
                catch (FileNotFoundException)
                {
                    Console.Error.WriteLine($"File {t.Json} not found.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    throw;
                }
            });
    }
}