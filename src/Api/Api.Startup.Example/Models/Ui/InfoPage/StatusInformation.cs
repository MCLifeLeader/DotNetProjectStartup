using System.Text.Json.Serialization;

namespace Startup.Api.Models.Ui.CanaryPage;

/// <summary>
/// 
/// </summary>
public class StatusInformation
{
    [JsonPropertyName("?xml")]
    public Xml Xml { get; set; }

    [JsonPropertyName("information")]
    public Information Information { get; set; }
}