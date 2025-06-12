using ExchangeLibrary.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ExchangeLibrary.Services;

public class ExchangeRateRealtimeServiceCached : IExchangeRateRealtimeService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ExchangeRateRealtimeService _service;

    public ExchangeRateRealtimeServiceCached(IMemoryCache memoryCache, ExchangeRateRealtimeService service)
    {
        _memoryCache = memoryCache;
        _service = service;
    }

    public async Task<ExchangeRateRealtimeResponse?> GetExchangeRateRealtime(string apiKey, string fromCurrency, string toCurrency)
    {
        string cacheKey = fromCurrency + toCurrency;
        return await _memoryCache.GetOrCreateAsync<ExchangeRateRealtimeResponse>(cacheKey, 
            async (entry) =>  await _service.GetExchangeRateRealtime(apiKey, fromCurrency, toCurrency));
    }
}
