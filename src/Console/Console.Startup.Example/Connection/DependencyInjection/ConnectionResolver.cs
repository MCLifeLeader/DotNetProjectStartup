using Microsoft.Extensions.DependencyInjection;
using Startup.Client.Api;
using Startup.Common.Connection;
using Startup.Common.Connection.Interfaces;
using Startup.Console.Model.ApplicationSettings;

namespace Startup.Console.Connection.DependencyInjection;

public static class ConnectionResolver
{
    public static void RegisterDependencies(IServiceCollection services, AppSettings appSettings)
    {
        services.AddTransient<IHttpClientWrapper, HttpClientWrapper>();
        services.AddTransient<StartupHttp>();

        services.AddSingleton(new UserLoginModel()
        {
            Username = appSettings.WorkerProcesses.StartupApi.Username,
            Password = appSettings.WorkerProcesses.StartupApi.Password,
            DisplayName = appSettings.WorkerProcesses.StartupApi.Username,
        });
    }
}