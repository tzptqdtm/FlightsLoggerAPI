﻿using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FlightsLoggerAPI.HealthChecks;

public class HealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var isHealthy = true;

        // Тут должна быть логика проверки состояния сервиса, подключения к кафке. Для упрощения не реализована.

        if (isHealthy)
        {
            return Task.FromResult(
                HealthCheckResult.Healthy("A healthy result."));
        }

        return Task.FromResult(
            new HealthCheckResult(
                context.Registration.FailureStatus, "An unhealthy result."));
    }
}