using ExchangeLibrary.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeLibrary.Services;

public interface IExchangeRateRealtimeService
{
    Task<ExchangeRateRealtimeResponse?> GetExchangeRateRealtimeAsync(string apiKey, 
        string fromCurrency, string toCurrency, CancellationToken ct);
}