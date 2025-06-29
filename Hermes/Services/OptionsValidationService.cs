using Hermes.Models;

namespace Hermes.Services;

public static class OptionsValidationService
{
    public static void Validate(Options options)
    {
        if (string.IsNullOrWhiteSpace(options.Json))
            throw new Exception("Json path is required");
        if (!File.Exists(options.Json))
            throw new FileNotFoundException(options.Json);

        if (!string.IsNullOrWhiteSpace(options.Db)
            && !File.Exists(options.Db))
            throw new Exception("Database path is required");
    }
}