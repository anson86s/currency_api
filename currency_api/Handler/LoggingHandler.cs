﻿using Microsoft.Extensions.Logging;

namespace currency_api.Handler;

public class LoggingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingHandler> _logger;

    public LoggingHandler(ILogger<LoggingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Log the request
        _logger.LogInformation($"Request: {request.Method} {request.RequestUri}");

        if (request.Content != null)
        {
            var requestContent = await request.Content.ReadAsStringAsync();
            _logger.LogInformation($"Request Content: {requestContent}");
        }

        // Send the request to the inner handler
        var response = await base.SendAsync(request, cancellationToken);

        // Log the response
        _logger.LogInformation($"Response: {response.StatusCode}");

        if (response.Content != null)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Response Content: {responseContent}");
        }

        return response;
    }
}

