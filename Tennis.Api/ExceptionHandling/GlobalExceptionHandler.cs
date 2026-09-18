using Microsoft.AspNetCore.Diagnostics;
using Tennis.Application.Exceptions;

namespace Tennis.Api.ExceptionHandling;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title, detail) = exception switch
        {
            TennisPlayerNotFoundException => (
                StatusCodes.Status404NotFound,
                "Joueur introuvable",
                exception.Message),
            TennisPlayerAlreadyExistsException => (
                StatusCodes.Status409Conflict,
                "Joueur déjà existant",
                exception.Message),
            TennisStatisticsUnavailableException => (
                StatusCodes.Status503ServiceUnavailable,
                "Statistiques indisponibles",
                exception.Message),
            ArgumentException => (
                StatusCodes.Status400BadRequest,
                "Requête invalide",
                exception.Message),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Erreur interne du serveur",
                environment.IsDevelopment()
                    ? exception.Message
                    : "Une erreur inattendue est survenue.")
        };

        if (statusCode >= StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Erreur non gérée. TraceId: {TraceId}", httpContext.TraceIdentifier);
        }
        else
        {
            logger.LogWarning(exception, "Erreur applicative. TraceId: {TraceId}", httpContext.TraceIdentifier);
        }

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path,
            Type = "about:blank"
        };
        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
