using Startup.Common.Models.Authorization;
using DeviceLoginModel = Api.Startup.Example.Models.Authorization.DeviceLoginModel;

namespace Api.Startup.Example.Helpers.Extensions;

public static class LoginHelper
{
    public static UserLoginModel ToUserLogin(this DeviceLoginModel entity)
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