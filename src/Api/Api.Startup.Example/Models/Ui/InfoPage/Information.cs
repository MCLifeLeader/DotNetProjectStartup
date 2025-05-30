using System.Text.Json.Serialization;

namespace Startup.Api.Models.Ui.InfoPage;

/// <summary>
/// 
/// </summary>
public class Information
{
    [JsonPropertyName("project-information")]
    public ProjectInformation? ProjectInformation { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("service")]
    public List<Service>? Service { get; set; }
}