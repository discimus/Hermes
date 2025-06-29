using Hermes.Exceptions;

namespace Hermes.Models;

public class Channel
{
    public string Url { get; private set; }

    public Channel(string url)
    {
        this.Url = url;
    }

    public void Validate(
        out bool isValid,
        out string errorMessage)
    {
        isValid = false;
        errorMessage = "";

        if (string.IsNullOrWhiteSpace(Url))
        {
            errorMessage = "Link URL is empty";
        }

        if (!Uri.IsWellFormedUriString(Url, UriKind.Absolute))
        {
            errorMessage = "Link URL is invalid";
        }

        isValid = string.IsNullOrWhiteSpace(errorMessage);
    }
}