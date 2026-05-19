using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handler;

public partial class CustomExceptionHandler
    (ILogger<CustomExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        (int statusCode, string title, string? detail) = exception switch
        {
            ValidationException e => (StatusCodes.Status400BadRequest, nameof(ValidationException), e.Message),
            BadRequestException e => (StatusCodes.Status400BadRequest, nameof(BadRequestException), e.Details ?? e.Message),
            NotFoundException e => (StatusCodes.Status404NotFound, nameof(NotFoundException), e.Message),
            InternalServerException e => (StatusCodes.Status500InternalServerError, nameof(InternalServerException), e.Details ?? e.Message),
            _ => (StatusCodes.Status500InternalServerError, "InternalServerError", exception.Message),
        };

        if (statusCode >= 500)
            LogError(logger, statusCode, exception);
        else
            LogWarning(logger, statusCode, exception);

        context.Response.StatusCode = statusCode;

        ProblemDetails problemDetails = new()
        {
            Title    = title,
            Detail   = detail,
            Status   = statusCode,
            Instance = context.Request.Path
        };

        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

        if (exception is ValidationException validationException)
            problemDetails.Extensions.Add("errors", validationException.Errors);

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);

        return true;
    }

    [LoggerMessage(Level = LogLevel.Warning, Message = "Client error HTTP {StatusCode}")]
    static partial void LogWarning(ILogger logger, int statusCode, Exception exception);

    [LoggerMessage(Level = LogLevel.Error, Message = "Unhandled server error HTTP {StatusCode}")]
    static partial void LogError(ILogger logger, int statusCode, Exception exception);
}
