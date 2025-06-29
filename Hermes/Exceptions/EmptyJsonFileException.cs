namespace Hermes.Exceptions;

public class EmptyJsonFileException : System.Exception
{
    public  EmptyJsonFileException() : base() { }
    public  EmptyJsonFileException(string message) : base(message) { }
    public  EmptyJsonFileException(string message, System.Exception inner) : base(message, inner) { }
}