namespace MyJetWallet.SimplePay.Client;

public interface ISimplePayResponse
{
    ResultCodes Result { get; }
    string Message { get; }
}