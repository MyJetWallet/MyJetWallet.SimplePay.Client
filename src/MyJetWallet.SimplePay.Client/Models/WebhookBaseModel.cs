namespace MyJetWallet.SimplePay.Client.Models;

public class WebhookBaseModel
{
    public string WebhookId { get; set; }
    public string MerchantId { get; set; }
    public string Timestamp { get; set; }
    public WebhookType Type { get; set; }
}