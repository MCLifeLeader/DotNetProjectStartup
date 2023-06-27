using System.Text.Json.Serialization;

namespace Api.Startup.Example.Models.Ui.CanaryPage;

public class ProjectInformation
{
    [JsonPropertyName("info")]
    public List<Info> Info { get; set; }
}