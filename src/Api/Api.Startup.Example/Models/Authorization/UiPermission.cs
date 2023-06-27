namespace Api.Startup.Example.Models.Authorization;

public class UiPermission
{
    public string AccountId { get; set; }
    public Permission Permission { get; set; }
    public string PermissionType { get; set; }
    public string TargetId { get; set; }
}