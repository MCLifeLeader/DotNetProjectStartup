using System.Text.Json.Serialization;

namespace Startup.Api.Models.Ui.InfoPage;

/// <summary>
/// 
/// </summary>
public class Messages
{
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}