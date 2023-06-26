using System.Text.Json.Serialization;

namespace Api.Startup.Example.Model.Ui.CanaryPage;

/// <summary>
/// 
/// </summary>
public class Messages
{
    [JsonPropertyName("message")]
    public string Message { get; set; }
}