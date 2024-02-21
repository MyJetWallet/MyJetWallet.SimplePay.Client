// See https://aka.ms/new-console-template for more information

using Google.Protobuf.WellKnownTypes;
using MyJetWallet.SimplePay;
using MyJetWallet.SimplePay.Client;

Console.WriteLine("Hello, World!");

var serviceGrpcUrl = "https://simple-pay-api-uat.simple-spot.biz";
var apiToken = "test_token";
        
var client = SimplePayFactory.CreateClient(serviceGrpcUrl, apiToken);

var resp = await client.HelloAsync(new HelloMessage() {Message = "Hi, how are you"});
Console.WriteLine($"Resp: {resp.Message}");


var whoResp = await client.WhoIAmAsync(new Empty());

if (whoResp.Result == ResultCodes.Unauthorised)
    Console.WriteLine("Wrong token");
else
    Console.WriteLine($"Who I am: {whoResp}");
        


Console.ReadLine();
Console.WriteLine("Good buy!");