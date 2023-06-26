using System.Text.Json.Serialization;

namespace Api.Startup.Example.Model.Ui.CanaryPage;

/// <summary>
/// 
/// </summary>
public class StatusInformation
{
    [JsonPropertyName("?xml")]
    public Xml Xml { get; set; }

    [JsonPropertyName("canary")]
    public Canary Canary { get; set; }
}