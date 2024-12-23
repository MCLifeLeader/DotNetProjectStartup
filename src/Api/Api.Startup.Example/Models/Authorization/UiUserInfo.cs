namespace Startup.Api.Models.Authorization;

public class UiUserInfo
{
    public string? Name { get; set; }

    public string? Role { get; set; }

    public string? UserAccountID { get; set; }

    public bool IsEmployeeImpersonating { get; set; }

    public bool CanImpersonate { get; set; }

    public int LanguageId { get; set; }


    public DateTime ExpiresAfter { get; set; }

    public long TimeStampTicks { get; set; }

    public void MarkAsImpersonating()
    {
        if (string.IsNullOrEmpty(Name))
        {
            Name = "";
        }

        if (Name.IndexOf("**", StringComparison.Ordinal) < 0)
        {
            Name = "**" + Name;
        }
    }
}