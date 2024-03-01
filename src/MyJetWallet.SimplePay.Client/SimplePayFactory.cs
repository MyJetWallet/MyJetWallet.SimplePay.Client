using System.Globalization;
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

    public static SimplePayClientGrpc.SimplePayClientGrpcClient CreateClientForWindows(string serviceGrpcUrl, string apiToken)
    {
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        var channel = GrpcChannel.ForAddress(serviceGrpcUrl, new GrpcChannelOptions
        {
            HttpHandler = new WinHttpHandler()
        }).Intercept(metadata =>
        {
            metadata.Add("Authorization", $"Bearer {apiToken}");
            return metadata;
        });

        var client = new SimplePayClientGrpc.SimplePayClientGrpcClient(channel);

        return client;
    }

    public static decimal AsDecimal(this string value)
    {
        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            return result;

        throw new Exception($"Cannot convert '{value}' to decimal");
    }

    public static string AsString(this decimal value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }
}
