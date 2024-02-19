using System.Runtime.Serialization;

// ReSharper disable once CheckNamespace
namespace MyJetWallet.SimplePay;

public static partial class SimplePayClientGrpc
{
    public partial class SimplePayClientGrpcClient
    {
        public string Test() { return "Test"; }
    }
}

public interface ISimplePayApi
{

}


