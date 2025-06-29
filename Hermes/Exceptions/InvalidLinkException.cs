namespace Hermes.Exceptions;

public class InvalidLinkException : System.Exception
{
    public  InvalidLinkException() : base() { }
    public  InvalidLinkException(string message) : base(message) { }
    public  InvalidLinkException(string message, System.Exception inner) : base(message, inner) { }
}