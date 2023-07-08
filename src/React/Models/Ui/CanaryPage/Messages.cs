using System.Text.Json.Serialization;

namespace React.Startup.Example.Models.Ui.CanaryPage;

/// <summary>
/// 
/// </summary>
public class Messages
{
    [JsonPropertyName("message")]
    public string Message { get; set; }
}