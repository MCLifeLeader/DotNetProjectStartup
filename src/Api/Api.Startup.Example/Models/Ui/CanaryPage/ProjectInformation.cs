using System.Text.Json.Serialization;

namespace Startup.Api.Models.Ui.CanaryPage;

public class ProjectInformation
{
    [JsonPropertyName("info")]
    public List<Info> Info { get; set; }
}