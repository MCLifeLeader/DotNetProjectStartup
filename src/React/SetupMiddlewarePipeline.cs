using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace React.Startup.Example;

public static class SetupMiddlewarePipeline
{
    private static readonly string _swaggerName = "StartupExample React";

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication SetupMiddleware(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        // Configure the HTTP request pipeline.
        if (app.Configuration.GetValue<bool>("SwaggerEnabled"))
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.EnableTryItOutByDefault();
                c.DocExpansion(DocExpansion.None);
                c.EnableFilter();
                c.DisplayRequestDuration();
                c.EnableDeepLinking();
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{_swaggerName} v1");
                c.InjectStylesheet("/css/SwaggerDark.css");
                c.DocumentTitle = $"{_swaggerName} Swagger UI";
            });
        }

        app.MapHealthChecks("/_health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            }).AllowAnonymous();

        app.UseHttpLogging();

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseIdentityServer();
        app.UseAuthorization();

        app.UseRequestDecompression();
        app.UseResponseCompression();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.MapFallbackToFile("index.html");

        return app;
    }
}