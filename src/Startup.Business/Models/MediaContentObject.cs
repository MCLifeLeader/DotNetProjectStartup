namespace Startup.Business.Models;

public class MediaContentObject
{
    public Guid MediaId { get; set; }
    public Guid AgencyId { get; set; }
    public string FileExtension { get; set; }
    public DateTime FileDate { get; set; }
}