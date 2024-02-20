using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;

namespace MyJetWallet.SimplePay.Client;

public static class SimplePayFactory
{
    public static async Task CreateClient(string serviceGrpcUrl)
    {
        var apiToken = "api::123123123";
        
        var channel = GrpcChannel.ForAddress(serviceGrpcUrl).Intercept(metadata =>
        {
            metadata.Add("Authorization", $"Bearer {apiToken}");
            return metadata;
        });
        
        var client = new SimplePayClientGrpc.SimplePayClientGrpcClient(channel);

        var resp = await client.HelloAsync(new HelloMessage() {Message = "Hi, how are you"});
        Console.WriteLine($"Resp: {resp.Message}");
        await client.NotifyAsync(new HelloMessage()
        {
            Message = "I'm fine"
        });
        
        Console.WriteLine(client.Test());



        // var client = new SimplePayClientGrpc.SimplePayClientGrpcClient(channel);
        //
        // var client = new Greeter.GreeterClient(channel);
        // var reply = await client.SayHelloAsync(
        //     new HelloRequest { Name = "GreeterClient" });
        // Console.WriteLine("Greeting: " + reply.Message);
        // Console.WriteLine("Press any key to exit...");
        // Console.ReadKey();
    }
}