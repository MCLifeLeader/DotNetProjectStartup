using Startup.Common.Models.Authorization;
using DeviceLoginModel = Startup.Api.Models.Authorization.DeviceLoginModel;

namespace Startup.Api.Helpers.Extensions;

public static class LoginHelper
{
    public static UserLoginModel? ToUserLogin(this DeviceLoginModel? entity)
    {
        if (entity == null)
        {
            return null;
        }

        return new UserLoginModel
        {
            DisplayName = $"Client {entity.DeviceId}",
            Password = entity.DeviceSecret ?? string.Empty,
            Username = entity.DeviceLogin ?? string.Empty
        };
    }
}