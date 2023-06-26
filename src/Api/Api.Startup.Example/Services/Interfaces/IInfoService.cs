namespace Api.Startup.Example.Services.Interfaces;

public interface IInfoService
{
    string SerializeToResponseXml();
    string SerializeToResponseJson();
}