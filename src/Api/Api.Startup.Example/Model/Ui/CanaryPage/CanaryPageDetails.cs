using System.Xml.Serialization;

namespace Api.Startup.Example.Model.Ui.CanaryPage;

/// <summary>
/// </summary>
[XmlRoot("canary")]
public class CanaryPageDetails
{
    /// <summary>
    ///     Gets or sets the project information collection.
    /// </summary>
    /// <value>
    ///     The project information collection.
    /// </value>
    [XmlArray("project-information")]
    [XmlArrayItem("info")]
    public List<ProjectInfoDetails> ProjectInfoCollection { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the title.
    /// </summary>
    /// <value>
    ///     The title.
    /// </value>
    [XmlElement("title")]
    public string Title { get; set; } = null!;

    /// <summary>
    ///     Gets the services.
    /// </summary>
    /// <value>
    ///     The services.
    /// </value>
    [XmlElement("service")]
    public List<ServiceDetails> Services { get; } = new();
}