// See https://aka.ms/new-console-template for more information

using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MyJetWallet.SimplePay;
using MyJetWallet.SimplePay.Client;

var serviceGrpcUrl = "https://simple-pay-api-uat.simple-spot.biz";
//var serviceGrpcUrl = "http://localhost:80";
var apiToken = "test_token";
var workspace = "pay-001";
var workspace2 = "pay-002";
var client = SimplePayFactory.CreateClient(serviceGrpcUrl, apiToken);
//var client = SimplePayFactory.CreateClientForWindows(serviceGrpcUrl, apiToken, true); //for local Grpc service
//var client = SimplePayFactory.CreateClientForWindows(serviceGrpcUrl, apiToken, false); //for remote Grpc service

//await SayHello(client);
//await GetBalance(client);
//await ContactsDemo(client);
//await PaymentDemo(client, TransactionAmountType.Settlement); //amount that will be received
//await PaymentDemo(client, TransactionAmountType.Total); //amount that will be send (including fee)
//TestResponses();
//await GetDepositHistory(client);
//await GetDepositAddress(client);

//--Webhooks--//
//await AddWebhook(client);
await GetWebhooks(client);
//await DisableAllWebhooks(client);


async Task DisableAllWebhooks(SimplePayClientGrpc.SimplePayClientGrpcClient simplePayClientGrpcClient)
{

    foreach (var w in new List<string>(){workspace, workspace2})
    {
        var resp = await simplePayClientGrpcClient.ListEnabledCallbacksAsync(new GetCallbacksRequest()
        {
            Workspace = w
        });

        foreach (var callbackModel in resp.Callbacks)
        {
            var r = await simplePayClientGrpcClient.DisableCallbackAsync(new DisableCallbackRequest()
            {
                Id = callbackModel.Id,
                Workspace = callbackModel.Workspace
            });
            Console.WriteLine($"DisableCallback: {r}");
        }
    }
}

async Task GetDepositAddress(SimplePayClientGrpc.SimplePayClientGrpcClient simplePayClientGrpcClient)
{
    var resp = await simplePayClientGrpcClient.AccountGetDepositAddressAsync(new SimplePayAssetAndNetworkDto()
    {
        AssetSymbol = "ETH",
        NetworkId = "fireblocks-eth-goerli",
        Workspace = workspace
    });
    Console.WriteLine($"Deposit Address: {resp}");
}

async Task GetWebhooks(SimplePayClientGrpc.SimplePayClientGrpcClient simplePayClientGrpcClient)
{
    var resp = await simplePayClientGrpcClient.ListEnabledCallbacksAsync(new () { Workspace = workspace });
    Console.WriteLine($"GetCallback: {resp}");
    
    var resp2 = await simplePayClientGrpcClient.ListEnabledCallbacksAsync(new () { Workspace = workspace2 });
    Console.WriteLine($"GetCallback: {resp2}");
}

async Task AddWebhook(SimplePayClientGrpc.SimplePayClientGrpcClient simplePayClientGrpcClient)
{
    var resp = await simplePayClientGrpcClient.AddCallbackAsync(new AddCallbackRequest()
    {
        Url = "https://webhook.site/0e90de65-dced-4301-8eec-15fdf08815d9",
        Workspace = workspace
    });
    Console.WriteLine($"AddCallback: {resp}");
    
    var resp2 = await simplePayClientGrpcClient.AddCallbackAsync(new AddCallbackRequest()
    {
        Url = "https://webhook.site/0e90de65-dced-4301-8eec-15fdf08815d9",
        Workspace = workspace2
    });
    Console.WriteLine($"AddCallback: {resp2}");
}

void TestResponses()
{
    var resp1 = new SimplePayContactResponse() { Result = ResultCodes.InternalError, Message = "Test" };

}

Console.ReadLine();
Console.WriteLine("Good bye!");

async Task SayHello(SimplePayClientGrpc.SimplePayClientGrpcClient simplePayClientGrpcClient)
{
    var resp = await simplePayClientGrpcClient.HelloAsync(new HelloMessage() { Message = "Hi, how are you #1" });
    Console.WriteLine($"Resp: {resp.Message}");

    var whoResp = await simplePayClientGrpcClient.WhoIAmAsync(new Empty());

    if (whoResp.Result == ResultCodes.Unauthorized)
        Console.WriteLine("Wrong token");
    else
        Console.WriteLine($"Who I am: {whoResp}");
}

async Task GetBalance(SimplePayClientGrpc.SimplePayClientGrpcClient client1)
{
    var resp = await client1.AccountGetAllBalancesAsync(new SimplePayWorkspaceDto() { Workspace = workspace });
    Console.WriteLine($"Balance pay-001: {resp}");
    resp = await client1.AccountGetAllBalancesAsync(new SimplePayWorkspaceDto() { Workspace = workspace2 });
    Console.WriteLine($"Balance pay-002: {resp}");
}

async Task ContactsDemo(SimplePayClientGrpc.SimplePayClientGrpcClient simplePayClientGrpcClient)
{
    var assets = await simplePayClientGrpcClient.DictionaryGetAllAssetsAsync(new Empty());
    var networks = await simplePayClientGrpcClient.DictionaryGetAllBlockchainsAsync(new Empty());
    
    Console.WriteLine($"Assets: {assets}");
    Console.WriteLine();
    Console.WriteLine($"Networks: {networks}");
    
    var validateResp = await simplePayClientGrpcClient.ValidateAddressAsync(new SimplePayAddress()
    {
        Workspace = workspace,
        AssetSymbol = "ETH",
        Address = "0x8D0aa0483728cF3404CB8fc867540435BEB4AeDaaa",
        NetworkId = "fireblocks-eth-goerli"
    });
    
    Console.WriteLine($"Validate WRONG address: {validateResp}");
    
    validateResp = await simplePayClientGrpcClient.ValidateAddressAsync(new SimplePayAddress()
    {
        Workspace = workspace,
        AssetSymbol = "ETH",
        Address = "0xe079dcaeb3549b719ff2C5c87ec119dc7a2789b3",
        NetworkId = "fireblocks-eth-goerli"
    });
    Console.WriteLine($"Validate address: {validateResp}");
    
    var createResp = await simplePayClientGrpcClient.ContactCreateAsync(new()
    {
        Workspace = workspace,
        AssetSymbol = "ETH",
        BlockchainAddress = "0x8D0aa0483728cF3404CB8fc867540435BEB4AeDf",
        NetworkId = "fireblocks-eth-goerli",
        GroupId = 0,
        Name = $"My test contact ({DateTime.Now.Date:yyyy-MM-dd})",
        Description = "test"
    });
    
    Console.WriteLine($"CreateResp: {createResp}");
    
    var id = createResp.Contact.Id;
    
    var updateResp = await simplePayClientGrpcClient.ContactUpdateAsync(new()
    {
        Id = id,
        Workspace = workspace,
        AssetSymbol = createResp.Contact.AssetSymbol,
        BlockchainAddress = "0xe079dcaeb3549b719ff2C5c87ec119dc7a2789b3",
        NetworkId = createResp.Contact.NetworkId,
        GroupId = createResp.Contact.GroupId,
        Name = "My test contact updated",
        Description = $"test updated {DateTime.Now.Date:yyyy-MM-dd}"
    });
    
    Console.WriteLine($"UpdateResp: {updateResp}");
    
    Console.WriteLine("Contact list:");

    var allContacts = await simplePayClientGrpcClient.ContactGetAllContactListAsync(workspace);
    foreach (var contact in allContacts)
    {
        Console.WriteLine($"Contact: {contact}");
    }

    var myContact = await simplePayClientGrpcClient.ContactGetByIdAsync(new SimplePayIdDto()
    {
        Id = id,
        Workspace = workspace
    });
    
    Console.WriteLine();
    Console.WriteLine($"My Contact: {myContact}");
    Console.WriteLine();
    
    Console.WriteLine("Press enter to continue, to Delete a contact ...");
    Console.ReadLine();

    var deleteResp = await simplePayClientGrpcClient.ContactDeleteAsync(new() { Id = id, Workspace = workspace });

    Console.WriteLine($"DeleteResp: {deleteResp}");
}

async Task PaymentDemo(SimplePayClientGrpc.SimplePayClientGrpcClient simplePayClientGrpcClient, TransactionAmountType amountType)
{
    var contactId = "428a82ade6464619bdb39ebbacce062a";
    
    var transferPreviewResp = await simplePayClientGrpcClient.SingleTransferPreviewAsync(new()
    {
        Workspace = workspace,
        AssetSymbol = "USDT",
        ContactId = contactId,
        PaymentAmount = amountType == TransactionAmountType.Settlement ? 10m.AsString() : string.Empty,
        TransactionAmount = amountType == TransactionAmountType.Total ? 10m.AsString() : string.Empty,

    });
    
    Console.WriteLine($"TransferPreviewResp: {transferPreviewResp}");
    
    var transferResp = await simplePayClientGrpcClient.SingleTransferExecuteAsync(new()
    {
        Workspace = workspace,
        AssetSymbol = "USDT",
        ContactId = contactId,
        PaymentAmount = amountType == TransactionAmountType.Settlement ? 10m.AsString() : string.Empty,
        TransactionAmount = amountType == TransactionAmountType.Total ? 10m.AsString() : string.Empty,
        StatementName = $"Demo {Guid.NewGuid().ToString()}",
        Comment = $"Test transfer at {DateTime.UtcNow:O}"
    });
    
    Console.WriteLine($"TransferResp: {transferResp}");
    
    var statementResp = await simplePayClientGrpcClient.StatementGetByIdAsync(new()
    {
        Workspace = workspace,
        Id = transferResp.Statement?.Id,
    });
    
    Console.WriteLine($"Statement after execute: {statementResp}");
    
    Console.WriteLine("Press ENTER to continue and read new Statement version ...");
    Console.ReadLine();
    
    statementResp = await simplePayClientGrpcClient.StatementGetByIdAsync(new()
    {
        Workspace = workspace,
        Id = transferResp.Statement?.Id,
    });

    Console.WriteLine($"Statement New Version: {statementResp}");
}

async Task GetDepositHistory(SimplePayClientGrpc.SimplePayClientGrpcClient simplePayClientGrpcClient)
{
    var resp = await simplePayClientGrpcClient.GetDepositHistoryAsync(new ()
    {
        Workspace = workspace2,
        LastSeenId = "1708941261970",
        AssetSymbol = "USDT",
        Take = 10
    });
    
    Console.WriteLine($"Deposit History: {resp}");
}
public enum TransactionAmountType
{
    Settlement,
    Total
}
