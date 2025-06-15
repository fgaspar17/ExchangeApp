using Microsoft.Extensions.Caching.Memory;

namespace ExchangeClient.HttpClientHandlers;

public class CacheHandler : DelegatingHandler
{
    private readonly IMemoryCache _memoryCache;

    public CacheHandler(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var query = System.Web.HttpUtility.ParseQueryString(request.RequestUri!.Query);
        var fromCurrency = query["from_currency"];
        var toCurrency = query["to_currency"];
        var key = $"{fromCurrency}-{toCurrency}";

        var cached = _memoryCache.Get<string>(key);
        if (cached is not null)
        {
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(cached)
            };
        }

        var response = await base.SendAsync(request, ct);

        var entryOptions = new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(1),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            Priority = CacheItemPriority.High,
            Size = 10,
        };

        _memoryCache.Set(key, await response.Content.ReadAsStringAsync(ct), entryOptions);

        return response;
    }
}