using ExchangeLibrary.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ExchangeLibrary.Services;

public class ExchangeRateRealtimeServiceCached : IExchangeRateRealtimeService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IExchangeRateRealtimeService _service;
    private readonly ILogger<ExchangeRateRealtimeServiceCached> _logger;

    public ExchangeRateRealtimeServiceCached(IMemoryCache memoryCache, IExchangeRateRealtimeService service,
        ILogger<ExchangeRateRealtimeServiceCached> logger)
    {
        _memoryCache = memoryCache;
        _service = service;
        _logger = logger;
    }

    public async Task<ExchangeRateRealtimeResponse?> GetExchangeRateRealtime(string apiKey,
        string fromCurrency, string toCurrency, CancellationToken ct)
    {
        string cacheKey = fromCurrency + toCurrency;
        return await _memoryCache.GetOrCreateAsync<ExchangeRateRealtimeResponse>(cacheKey,
            async (entryOptions) =>
            {
                _logger.LogDebug("Cache miss for exchange rate {From}->{To}, fetching from service.", fromCurrency, toCurrency);
                entryOptions.SetSlidingExpiration(TimeSpan.FromMinutes(1));
                entryOptions.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                entryOptions.SetPriority(CacheItemPriority.High);
                entryOptions.SetSize(10);
                return await _service.GetExchangeRateRealtime(apiKey, fromCurrency, toCurrency, ct);
            });
    }
}
