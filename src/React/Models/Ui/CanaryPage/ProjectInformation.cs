using System.Text.Json.Serialization;

namespace React.Startup.Example.Models.Ui.CanaryPage;

public class ProjectInformation
{
    [JsonPropertyName("info")]
    public List<Info> Info { get; set; }
}