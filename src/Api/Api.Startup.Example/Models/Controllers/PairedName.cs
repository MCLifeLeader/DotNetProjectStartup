namespace Startup.Api.Models.Controllers;

/// <summary>
/// Represents a pairing of two people in a gift exchange or other activity.
/// </summary>
public class PairedName
{
    public string Person { get; set; } = null!;
    public string HasName { get; set; } = null!;
}