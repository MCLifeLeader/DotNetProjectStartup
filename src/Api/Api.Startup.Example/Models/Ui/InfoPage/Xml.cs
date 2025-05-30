using System.Text.Json.Serialization;

namespace Startup.Api.Models.Ui.InfoPage;

/// <summary>
/// 
/// </summary>
public class Xml
{
    [JsonPropertyName("@version")]
    public string? Version { get; set; }

    [JsonPropertyName("@encoding")]
    public string? Encoding { get; set; }
}