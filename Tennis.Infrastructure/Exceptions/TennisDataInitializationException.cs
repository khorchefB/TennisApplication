namespace Tennis.Infrastructure.Exceptions;

public sealed class TennisDataInitializationException(string message, Exception innerException)
    : Exception(message, innerException);
