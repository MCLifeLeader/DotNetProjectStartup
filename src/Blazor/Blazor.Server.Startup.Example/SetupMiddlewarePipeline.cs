using Startup.Blazor.Server.Components;
using Startup.Blazor.Server.Components.Account;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Startup.Blazor.Server.Models.ApplicationSettings;

namespace Startup.Blazor.Server;

public static class SetupMiddlewarePipeline
{
    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="appSettings"></param>
    /// <returns></returns>
    public static WebApplication SetupMiddleware(this WebApplication app, AppSettings? appSettings)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.MapHealthChecks("/_health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            }); //.RequireAuthorization();

        app.UseHttpLogging();

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        //app.UseAuthentication();
        //app.UseAuthorization();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        // Add additional endpoints required by the Identity /Account Razor components.
        app.MapAdditionalIdentityEndpoints();

        return app;
    }
}