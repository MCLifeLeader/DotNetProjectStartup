using System.Text.Json.Serialization;

namespace Api.Startup.Example.Model.Ui.CanaryPage;

/// <summary>
/// 
/// </summary>
public class Info
{
    [JsonPropertyName("@name")]
    public string Name { get; set; }

    [JsonPropertyName("#text")]
    public string Text { get; set; }
}