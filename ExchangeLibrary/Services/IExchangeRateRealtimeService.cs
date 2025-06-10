using ExchangeLibrary.Models;

namespace ExchangeLibrary.Services;

public interface IExchangeRateRealtimeService
{
    Task<ExchangeRateRealtimeResponse?> GetExchangeRateRealtime(HttpClient client, string apiKey, string fromCurrency, string toCurrency);
}