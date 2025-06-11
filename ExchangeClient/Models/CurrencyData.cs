namespace ExchangeClient.Data;

public static class CurrencyData
{
    public static readonly List<Currency> Currencies = new List<Currency>
    {
        new Currency { Code = "CNY", Name = "Chinese Yuan" },
        new Currency { Code = "DKK", Name = "Danish Krone" },
        new Currency { Code = "EUR", Name = "Euro" },
        new Currency { Code = "GBP", Name = "British Pound Sterling" },
        new Currency { Code = "JPY", Name = "Japanese Yen" },
        new Currency { Code = "KRW", Name = "South Korean Won" },
        new Currency { Code = "MAD", Name = "Moroccan Dirham" },
        new Currency { Code = "USD", Name = "United States Dollar" },
        new Currency { Code = "ADA", Name = "Cardano" },
        new Currency { Code = "BTC", Name = "Bitcoin" },
        new Currency { Code = "ETH", Name = "Ethereum" },
        new Currency { Code = "BCH", Name = "Bitcoin-Cash" },
        new Currency { Code = "XRP", Name = "Ripple" },
        new Currency { Code = "SOL", Name = "Solana" }
    };
}
