using System.Text.Json.Serialization;

namespace Api.Startup.Example.Models.Ui.CanaryPage;

/// <summary>
/// 
/// </summary>
public class Messages
{
    [JsonPropertyName("message")]
    public string Message { get; set; }
}