﻿using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
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
        logger.LogInformation("[START] Handling {Request} " +
            "- Response={Response} " +
            "- RequestData={RequestData}", 
            typeof(TRequest).Name,
            typeof(TResponse).Name,
            request);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();
        var timeTaken = timer.Elapsed;
        if (timeTaken.Seconds > 3)
            logger.LogWarning("[SLOW] Handling {Request} " +
                "- Response={Response} " +
                "- TimeTaken={TimeTaken}", 
                typeof(TRequest).Name,
                typeof(TResponse).Name,
                timeTaken);

        logger.LogInformation("[END] Handling {Request} " +
            "- Response={Response} " +
            "- TimeTaken={TimeTaken}", 
            typeof(TRequest).Name,
            typeof(TResponse).Name,
            timeTaken);

        return response;
    }
}
