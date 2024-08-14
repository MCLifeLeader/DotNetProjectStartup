using System.Text.Json.Serialization;

namespace Startup.Api.Models.Ui.InfoPage;

public class ProjectInformation
{
    [JsonPropertyName("info")]
    public List<Info>? Info { get; set; }
}