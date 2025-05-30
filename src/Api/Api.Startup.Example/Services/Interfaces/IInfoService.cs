namespace Startup.Api.Services.Interfaces;

public interface IInfoService
{
    string SerializeToResponseXml();
    string SerializeToResponseJson();
}