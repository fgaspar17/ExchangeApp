using ExchangeLibrary.Models;

namespace ExchangeLibrary.Services;

public interface IExchangeRateRealtimeService
{
    Task<ExchangeRateRealtimeResponse?> GetExchangeRateRealtime(string apiKey, string fromCurrency, string toCurrency);
}