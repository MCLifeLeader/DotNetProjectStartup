using System.Web;
using System.Xml.Serialization;

namespace Startup.Api.Models.Ui.InfoPage;

public class ServiceDetails
{
    // needed for serialization
    /// <summary>
    ///     Initializes a new instance of the <see cref="ServiceDetails" /> class.
    /// </summary>
    public ServiceDetails()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ServiceDetails" /> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="status">The status.</param>
    /// <param name="responseTime">The response time.</param>
    public ServiceDetails(string name, Status status, long responseTime)
    {
        Name = name;
        EncodedName = HttpUtility.UrlEncode(name);
        Status = status;
        ResponseTime = responseTime;
    }

    /// <summary>
    ///     Gets or sets the name.
    /// </summary>
    /// <value>
    ///     The name.
    /// </value>
    [XmlElement("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the name of the encoded.
    /// </summary>
    /// <value>
    ///     The name of the encoded.
    /// </value>
    [XmlElement("encoded-name")]
    public string EncodedName { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the messages.
    /// </summary>
    /// <value>
    ///     The messages.
    /// </value>
    [XmlArray("messages")]
    [XmlArrayItem("message")]
    public List<string> Messages { get; set; } = null!;


    /// <summary>
    ///     Gets or sets the status.
    /// </summary>
    /// <value>
    ///     The status.
    /// </value>
    [XmlElement("status")]
    public Status Status { get; set; }

    /// <summary>
    ///     Gets or sets the response time.
    /// </summary>
    /// <value>
    ///     The response time.
    /// </value>
    [XmlElement("response-time")]
    public long ResponseTime { get; set; }


    /// <summary>
    ///     Adds the message.
    /// </summary>
    /// <param name="message">The message.</param>
    public void AddMessage(string message)
    {
        Messages ??= new List<string>();
        Messages.Add(message);
    }
}