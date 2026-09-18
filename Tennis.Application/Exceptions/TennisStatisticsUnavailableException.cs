namespace Tennis.Application.Exceptions;

public sealed class TennisStatisticsUnavailableException(string message) : Exception(message);
