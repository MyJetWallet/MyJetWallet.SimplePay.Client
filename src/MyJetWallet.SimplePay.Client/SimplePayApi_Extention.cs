using System.Runtime.Serialization;
using Grpc.Core;
using MyJetWallet.SimplePay.Client;

// ReSharper disable once CheckNamespace
namespace MyJetWallet.SimplePay;

public static partial class SimplePayClientGrpc
{
    public partial class SimplePayClientGrpcClient
    {
        public async Task<List<SimplePayContact>> ContactGetAllContactListAsync(string workspace)
        {
            var allResp = ContactGetAllContactStream(new SimplePayWorkspaceDto() {Workspace = workspace});
            var data = new List<SimplePayContact>();
            await foreach (var contact in allResp.ResponseStream.ReadAllAsync())
            {
                data.Add(contact);
            }

            return data;
        }
    }
}


public partial class SimplePayContactResponse : ISimplePayResponse
{
}

public partial class SimplePayResponse : ISimplePayResponse
{
}

public partial class SimplePayAssetListResponse : ISimplePayResponse
{
}

public partial class SimplePayBlockchainListResponse : ISimplePayResponse
{
}

public partial class SimplePayWorkspaceDto : ISimplePayResponse
{
}

public partial class SimplePayBalanceListResponse : ISimplePayResponse
{
}

public partial class SimplePayAddressResponse : ISimplePayResponse
{
}

public partial class SimplePayContactResponse : ISimplePayResponse
{
}

public partial class SimplePayAddressValidationResponse : ISimplePayResponse
{
}

public partial class SimplePayTransferPreviewResponse : ISimplePayResponse
{
}

public partial class SimplePayStatementResponse : ISimplePayResponse
{
}

public partial class SimplePayBlockchainListResponse : ISimplePayResponse
{
}

public partial class InvoiceResponse : ISimplePayResponse
{
}

public partial class InvoiceListResponse : ISimplePayResponse
{
}


