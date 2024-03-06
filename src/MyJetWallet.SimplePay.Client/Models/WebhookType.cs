using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace MyJetWallet.SimplePay.Client.Models;

[Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WebhookType
{
    Unknown,
    Statement,
    Deposit,
    Balance,
}