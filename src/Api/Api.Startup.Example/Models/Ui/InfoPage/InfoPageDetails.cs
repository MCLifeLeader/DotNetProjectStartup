﻿using System.Xml.Serialization;

namespace Startup.Api.Models.Ui.InfoPage;

/// <summary>
/// </summary>
[XmlRoot("information")]
public class InfoPageDetails
{
    /// <summary>
    ///     Gets or sets the project information collection.
    /// </summary>
    /// <value>
    ///     The project information collection.
    /// </value>
    [XmlArray("project-information")]
    [XmlArrayItem("info")]
    public List<ProjectInfoDetails> ProjectInfoCollection { get; set; } = new List<ProjectInfoDetails>();

    /// <summary>
    ///     Gets or sets the title.
    /// </summary>
    /// <value>
    ///     The title.
    /// </value>
    [XmlElement("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    ///     Gets the services.
    /// </summary>
    /// <value>
    ///     The services.
    /// </value>
    [XmlElement("service")]
    public List<ServiceDetails> Services { get; } = new();
}