using System.Globalization;
using System.Runtime.InteropServices;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;

namespace MyJetWallet.SimplePay.Client;

public static class SimplePayFactory
{
    public static SimplePayClientGrpc.SimplePayClientGrpcClient CreateClient(string serviceGrpcUrl, string apiToken)
    {

        bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        CallInvoker channel;

        if (isWindows)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            channel = GrpcChannel.ForAddress(serviceGrpcUrl, new GrpcChannelOptions
            {
                HttpHandler = new WinHttpHandler()
            }).Intercept(metadata =>
            {
                metadata.Add("Authorization", $"Bearer {apiToken}");
                return metadata;
            });
        }
        else
        {
            channel = GrpcChannel.ForAddress(serviceGrpcUrl).Intercept(metadata =>
            {
                metadata.Add("Authorization", $"Bearer {apiToken}");
                return metadata;
            });
        }

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
