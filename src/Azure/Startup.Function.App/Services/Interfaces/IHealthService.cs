using Microsoft.AspNetCore.Mvc;

namespace Startup.Function.Api.Services.Interfaces;

public interface IHealthService
{
    public Task<ObjectResult> CheckHealthAsync();
}