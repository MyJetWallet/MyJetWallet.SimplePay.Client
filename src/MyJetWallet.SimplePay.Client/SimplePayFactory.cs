﻿using System.Globalization;
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

    public static decimal AsDecimal(this string value)
    {
        if (decimal.TryParse(value, out var result))
            return result;

        throw new Exception($"Cannot convert '{value}' to decimal");
    }
    
    public static string AsString(this decimal value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }
}
