// See https://aka.ms/new-console-template for more information

using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MyJetWallet.SimplePay;
using MyJetWallet.SimplePay.Client;

var serviceGrpcUrl = "https://simple-pay-api-uat.simple-spot.biz";
var apiToken = "test_token";
var workspace = "pay-001";
var client = SimplePayFactory.CreateClient(serviceGrpcUrl, apiToken);
//await SayHello(client);
//await GetBalance(client);
//await ContactsDemo(client);
//await PaymentDemo(client);


Console.ReadLine();
Console.WriteLine("Good bye!");

async Task SayHello(SimplePayClientGrpc.SimplePayClientGrpcClient simplePayClientGrpcClient)
{
    var resp = await simplePayClientGrpcClient.HelloAsync(new HelloMessage() {Message = "Hi, how are you"});
    Console.WriteLine($"Resp: {resp.Message}");


    var whoResp = await simplePayClientGrpcClient.WhoIAmAsync(new Empty());

    if (whoResp.Result == ResultCodes.Unauthorized)
        Console.WriteLine("Wrong token");
    else
        Console.WriteLine($"Who I am: {whoResp}");
}

async Task GetBalance(SimplePayClientGrpc.SimplePayClientGrpcClient client1)
{
    var resp = await client1.AccountGetAllBalancesAsync(new SimplePayWorkspaceDto() {Workspace = workspace});
    Console.WriteLine($"Resp: {resp}");
}

async Task ContactsDemo(SimplePayClientGrpc.SimplePayClientGrpcClient simplePayClientGrpcClient)
{
    var id = Guid.NewGuid().ToString();

    var createResp = await simplePayClientGrpcClient.ContactCreateAsync(new()
    {
        Id = id,
        Workspace = workspace,
        AssetSymbol = "ETH",
        BlockchainAddress = "0x8D0aa0483728cF3404CB8fc867540435BEB4AeDf",
        NetworkId = "fireblocks-eth-goerli",
        GroupId = "1",
        Name = "My test contact",
        Description = "test"
    });
    
    Console.WriteLine($"CreateResp: {createResp}");
    
    var updateResp = await simplePayClientGrpcClient.ContactUpdateAsync(new()
    {
        Id = id,
        Workspace = workspace,
        AssetSymbol = createResp.Contact.AssetSymbol,
        BlockchainAddress = createResp.Contact.BlockchainAddress,
        NetworkId = createResp.Contact.NetworkId,
        GroupId = createResp.Contact.GroupId,
        Name = "My test contact updated",
        Description = "test updated"
    });
    
    Console.WriteLine($"UpdateResp: {updateResp}");
    
    var allResp = simplePayClientGrpcClient.ContactGetAllContactStream(new SimplePayWorkspaceDto() {Workspace = workspace});
    await foreach (var contact in allResp.ResponseStream.ReadAllAsync())
    {
        Console.WriteLine($"Contact: {contact}");
    }
    
    var deleteResp = await simplePayClientGrpcClient.ContactDeleteAsync(new () {Id = id, Workspace = workspace});
    
    Console.WriteLine($"DeleteResp: {deleteResp}");
}

async Task PaymentDemo(SimplePayClientGrpc.SimplePayClientGrpcClient simplePayClientGrpcClient)
{
    var contactId = Guid.NewGuid().ToString();
    var createResp = await simplePayClientGrpcClient.ContactCreateAsync(new()
    {
        Id = contactId,
        Workspace = workspace,
        AssetSymbol = "ETH",
        BlockchainAddress = "0x8D0aa0483728cF3404CB8fc867540435BEB4AeDf",
        NetworkId = "fireblocks-eth-goerli",
        GroupId = "1",
        Name = "My test contact",
        Description = "test",
    });
    
    Console.WriteLine($"CreateResp: {createResp}");
    
    var transferPreviewResp = await simplePayClientGrpcClient.SingleTransferPreviewAsync(new()
    {
        Workspace = workspace,
        AssetSymbol = "ETH",
        ContactId = createResp.Contact.Id,
        PaymentAmount = "0.0001",
    });
    
    Console.WriteLine($"TransferPreviewResp: {transferPreviewResp}");
    
    var transferResp = await simplePayClientGrpcClient.SingleTransferExecuteAsync(new()
    {
        Workspace = workspace,
        AssetSymbol = "ETH",
        ContactId = createResp.Contact.Id,
        PaymentAmount = "0.0001",
        StatementName = $"Demo {Guid.NewGuid().ToString()}"
    });
    
    Console.WriteLine($"TransferResp: {transferResp}");
    
    var statesmentResp = await simplePayClientGrpcClient.StatementGetByIdAsync(new()
    {
        Workspace = workspace,
        Id = transferResp.Statement?.Id,
    });
    
    Console.WriteLine($"StatesmentResp: {statesmentResp}");
    
    var deleteResp = await simplePayClientGrpcClient.ContactDeleteAsync(new () {Id = contactId, Workspace = workspace});
}
