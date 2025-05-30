using System.Text.Json.Serialization;

namespace Startup.Api.Models.Ui.InfoPage;

/// <summary>
/// 
/// </summary>
public class Info
{
    [JsonPropertyName("@name")]
    public string? Name { get; set; }

    [JsonPropertyName("#text")]
    public string? Text { get; set; }
}