﻿using System.Text.Json.Serialization;

namespace Startup.Api.Models.Ui.CanaryPage;

/// <summary>
/// 
/// </summary>
public class Canary
{
    [JsonPropertyName("project-information")]
    public ProjectInformation ProjectInformation { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("service")]
    public List<Service> Service { get; set; }
}