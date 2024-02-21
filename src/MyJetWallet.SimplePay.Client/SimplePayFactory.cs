using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;

namespace MyJetWallet.SimplePay.Client;

public static class SimplePayFactory
{
    public static SimplePayClientGrpc.SimplePayClientGrpcClient CreateClient(string serviceGrpcUrl, string apiToken)
    {
        var channel = GrpcChannel.ForAddress(serviceGrpcUrl).Intercept(metadata =>
        {
            metadata.Add("Authorization", $"Bearer {apiToken}");
            return metadata;
        });

        var client = new SimplePayClientGrpc.SimplePayClientGrpcClient(channel);


        return client;
    }
}
