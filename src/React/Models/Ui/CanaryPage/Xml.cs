using System.Text.Json.Serialization;

namespace React.Startup.Example.Models.Ui.CanaryPage;

/// <summary>
/// 
/// </summary>
public class Xml
{
    [JsonPropertyName("@version")]
    public string Version { get; set; }

    [JsonPropertyName("@encoding")]
    public string Encoding { get; set; }
}