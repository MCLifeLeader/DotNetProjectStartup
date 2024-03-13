using System.Xml.Serialization;

namespace Startup.Api.Models.Ui.CanaryPage;

/// <summary>
/// </summary>
public class ProjectInfoDetails
{
    // needed for serialization
    /// <summary>
    ///     Initializes a new instance of the <see cref="ProjectInfoDetails" /> class.
    /// </summary>
    protected ProjectInfoDetails()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ProjectInfoDetails" /> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="text">The text.</param>
    public ProjectInfoDetails(string name, string text)
    {
        Name = name;
        Text = text;
    }

    /// <summary>
    ///     Gets or sets the name.
    /// </summary>
    /// <value>
    ///     The name.
    /// </value>
    [XmlAttribute("name")]
    public string Name { get; set; }

    /// <summary>
    ///     Gets or sets the text.
    /// </summary>
    /// <value>
    ///     The text.
    /// </value>
    [XmlText]
    public string Text { get; set; }
}