using System.Text.Json.Serialization;

namespace Startup.Api.Models.Ui.CanaryPage;

/// <summary>
/// 
/// </summary>
public class StatusInformation
{
    [JsonPropertyName("?xml")]
    public required Xml Xml { get; set; }

    [JsonPropertyName("information")]
    public required Information Information { get; set; }
}