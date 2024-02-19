// See https://aka.ms/new-console-template for more information

using MyJetWallet.SimplePay.Client;

Console.WriteLine("Hello, World!");

await SimplePayFactory.CreateClient("http://localhost:80");

Console.ReadLine();
Console.WriteLine("Good buy!");