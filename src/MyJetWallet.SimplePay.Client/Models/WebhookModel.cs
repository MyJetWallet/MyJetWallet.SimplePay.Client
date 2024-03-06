namespace MyJetWallet.SimplePay.Client.Models;

public class WebhookModel<T> : WebhookBaseModel
{
    public T Payload { get; set; }
}