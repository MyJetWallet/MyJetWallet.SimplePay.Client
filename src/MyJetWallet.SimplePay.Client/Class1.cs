using Grpc.Net.Client;

namespace MyJetWallet.SimplePay.Client;

public static class SimplePayFactory
{
    public static async Task CreateClient(string serviceGrpcUrl)
    {
        using var channel = GrpcChannel.ForAddress(serviceGrpcUrl);
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