#nullable enable
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Startup.Api.Models.ApplicationSettings;
using Startup.Api.Models.Ui.InfoPage;
using Startup.Api.Services.Interfaces;
using Startup.Common.Helpers.Extensions;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Xml.Serialization;

namespace Startup.Api.Services;

public class InfoService : IInfoService
{
    private readonly AppSettings _appSettings;
    private readonly InfoPageDetails _canaryPageDetails = new();
    private readonly ILogger<InfoService> _logger;
    private readonly Stopwatch _totalTimer = new();

    // needed for serialization
    /// <summary>
    ///     Initializes a new instance of the <see cref="InfoService" /> class.
    /// </summary>
    public InfoService(
        ILogger<InfoService> logger,
        IOptions<AppSettings> appSettings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _appSettings = appSettings.Value ?? throw new ArgumentNullException(nameof(appSettings));

        _logger.LogDebug("{ControllerName} init.", nameof(InfoService));
    }

    /// <summary>
    ///     Serializes to response.
    /// </summary>
    /// <returns></returns>
    public string SerializeToResponseXml()
    {
        _logger.LogDebug("{ControllerName}.{MethodName} - Executing", nameof(InfoService), nameof(SerializeToResponseXml));

        PopulateProjectInfoCollection();
        AddCanaryPageDatabaseEntry();
        AddCanaryPageServicesEntry();

        XmlSerializerNamespaces serializerNamespaces = new XmlSerializerNamespaces();
        serializerNamespaces.Add("", "");

        XmlSerializer serializer = new XmlSerializer(_canaryPageDetails.GetType());
        using StringWriter textWriter = new StringWriter();

        serializer.Serialize((TextWriter)textWriter, (object?)_canaryPageDetails, serializerNamespaces);
        string response = textWriter.ToString();

        return response;
    }

    public string SerializeToResponseJson()
    {
        _logger.LogDebug("{ControllerName}.{MethodName} - Executing", nameof(InfoService), nameof(SerializeToResponseJson));

        PopulateProjectInfoCollection();
        AddCanaryPageDatabaseEntry();
        AddCanaryPageServicesEntry();

        return _canaryPageDetails.ToJson();
    }

    private void PopulateProjectInfoCollection()
    {
        _totalTimer.Start();

        Assembly assembly = GetType().Assembly;
        _canaryPageDetails.Title = "StartupExample Api";

        Version? assemblyVersion = assembly.GetName().Version;
        _canaryPageDetails.ProjectInfoCollection = new List<ProjectInfoDetails>
        {
            new("Current Time on Server", DateTime.Now.ToString(CultureInfo.CurrentCulture)),
            new("Product Name", assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product!),
            new("Product Version", $"{assemblyVersion?.Major}.{assemblyVersion?.Minor}.{assemblyVersion?.Build}"),
            new("Build Date", new FileInfo(assembly.Location ?? "").LastWriteTime.ToString(CultureInfo.CurrentCulture)),
            new("Build Version", assemblyVersion?.ToString()!),
            new("Runtime .NET Framework Version", Environment.Version.ToString()),
            new("Product .NET Framework Version", assembly.ImageRuntimeVersion),
            new("Server OS Version", Environment.OSVersion.VersionString),
            new("Single Resource", ""),
            new("App Relative Path", "~/canary")
        };
    }

    private void AddCanaryPageServicesEntry()
    {
        _totalTimer.Stop();

        ServiceDetails canaryPageService = new ServiceDetails(
            "Canary Page", Status.Ok, _totalTimer.ElapsedMilliseconds
        );

        _canaryPageDetails.Services.Insert(0, canaryPageService);
    }

    private void AddCanaryPageDatabaseEntry()
    {
        Status databaseStatus;
        string message;

        Stopwatch stopwatch = Stopwatch.StartNew();

        try
        {
            int result;

            DbContextOptionsBuilder<InfoDbContext> optionsBuilder = new DbContextOptionsBuilder<InfoDbContext>();
            optionsBuilder.UseSqlServer(_appSettings.ConnectionStrings.DefaultConnection);

            using (InfoDbContext context = new InfoDbContext(optionsBuilder.Options))
            {
                result = RelationalQueryableExtensions.FromSqlRaw(context.TestEntities, "SELECT 1 as TestInt").Count();
            }

            if (result == 1)
            {
                databaseStatus = Status.Ok;
                message = "Operation completed successfully.";
            }
            else
            {
                databaseStatus = Status.Warning;
                message = $"Unexpected value returned; expected 1, received {result}";
            }
        }
        catch (SqlException ex)
        {
            databaseStatus = Status.Warning;
            if (ex.Message.Contains("Invalid object name"))
            {
                databaseStatus = Status.Critical;
            }

            message = ex.Message;
        }
        catch (Exception ex)
        {
            databaseStatus = Status.Critical;
            message = ex.Message;
        }

        stopwatch.Stop();

        ServiceDetails service = new ServiceDetails("DefaultConnection", databaseStatus, stopwatch.ElapsedMilliseconds);
        service.AddMessage(message);
        _canaryPageDetails.Services.Add(service);
    }
}