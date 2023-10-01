using Azure.Core.Extensions;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Microsoft.Extensions.Azure;

namespace Startup.Common.Helpers.Extensions;

internal static class AzureClientFactoryBuilderExtensions
{
    public static IAzureClientBuilder<BlobServiceClient, BlobClientOptions> AddBlobServiceClient(
        this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
    {
        if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out var serviceUri))
        {
            return builder.AddBlobServiceClient(serviceUri);
        }

        return builder.AddBlobServiceClient(serviceUriOrConnectionString);
    }

    public static IAzureClientBuilder<QueueServiceClient, QueueClientOptions> AddQueueServiceClient(
        this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
    {
        if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out var serviceUri))
        {
            return builder.AddQueueServiceClient(serviceUri);
        }

        return builder.AddQueueServiceClient(serviceUriOrConnectionString);
    }
}