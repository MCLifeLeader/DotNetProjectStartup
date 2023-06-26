using System.Text.Json.Serialization;

namespace Api.Startup.Example.Model.Ui.CanaryPage;

public class ProjectInformation
{
    [JsonPropertyName("info")]
    public List<Info> Info { get; set; }
}