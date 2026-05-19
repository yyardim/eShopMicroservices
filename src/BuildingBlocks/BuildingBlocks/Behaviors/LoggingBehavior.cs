using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public partial class LoggingBehavior<TRequest, TResponse>
    (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        string responseName = typeof(TResponse).Name;

        LogStart(logger, requestName, responseName, request);

        long startTimestamp = Stopwatch.GetTimestamp();

        TResponse response = await next(cancellationToken);

        TimeSpan elapsed = Stopwatch.GetElapsedTime(startTimestamp);

        if (elapsed.TotalSeconds > 3)
            LogSlow(logger, requestName, responseName, elapsed);

        LogEnd(logger, requestName, responseName, elapsed);

        return response;
    }

    [LoggerMessage(Level = LogLevel.Information,
        Message = "[START] Handling {Request} - Response={Response} - RequestData={RequestData}")]
    static partial void LogStart(ILogger logger, string request, string response, object? requestData);

    [LoggerMessage(Level = LogLevel.Warning,
        Message = "[SLOW] Handling {Request} - Response={Response} - TimeTaken={TimeTaken}")]
    static partial void LogSlow(ILogger logger, string request, string response, TimeSpan timeTaken);

    [LoggerMessage(Level = LogLevel.Information,
        Message = "[END] Handling {Request} - Response={Response} - TimeTaken={TimeTaken}")]
    static partial void LogEnd(ILogger logger, string request, string response, TimeSpan timeTaken);
}
