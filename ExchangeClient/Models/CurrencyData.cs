using ExchangeClient.Models;

namespace ExchangeClient.Data;

public static class CurrencyData
{
    public static readonly Currency[] Currencies =
    {
        new() { Code = "CNY", Name = "Chinese Yuan" },
        new() { Code = "DKK", Name = "Danish Krone" },
        new() { Code = "EUR", Name = "Euro" },
        new() { Code = "GBP", Name = "British Pound Sterling" },
        new() { Code = "JPY", Name = "Japanese Yen" },
        new() { Code = "KRW", Name = "South Korean Won" },
        new() { Code = "MAD", Name = "Moroccan Dirham" },
        new() { Code = "USD", Name = "United States Dollar" },
        new() { Code = "ADA", Name = "Cardano" },
        new() { Code = "BTC", Name = "Bitcoin" },
        new() { Code = "ETH", Name = "Ethereum" },
        new() { Code = "BCH", Name = "Bitcoin-Cash" },
        new() { Code = "XRP", Name = "Ripple" },
        new() { Code = "SOL", Name = "Solana" }
    };
}
