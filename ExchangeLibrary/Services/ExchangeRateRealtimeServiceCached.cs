using ExchangeLibrary.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ExchangeLibrary.Services;

public class ExchangeRateRealtimeServiceCached : IExchangeRateRealtimeService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IExchangeRateRealtimeService _service;

    public ExchangeRateRealtimeServiceCached(IMemoryCache memoryCache, IExchangeRateRealtimeService service)
    {
        _memoryCache = memoryCache;
        _service = service;
    }

    public async Task<ExchangeRateRealtimeResponse?> GetExchangeRateRealtime(string apiKey, 
        string fromCurrency, string toCurrency, CancellationToken ct)
    {
        string cacheKey = fromCurrency + toCurrency;
        return await _memoryCache.GetOrCreateAsync<ExchangeRateRealtimeResponse>(cacheKey,
            async (entryOptions) =>
            {
                entryOptions.SetSlidingExpiration(TimeSpan.FromMinutes(1));
                entryOptions.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                entryOptions.SetPriority(CacheItemPriority.High);
                entryOptions.SetSize(10);
                return await _service.GetExchangeRateRealtime(apiKey, fromCurrency, toCurrency, ct);
            });
    }
}
