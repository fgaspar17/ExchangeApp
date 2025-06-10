using System.Net.Http.Json;
using ExchangeLibrary.Models;

namespace ExchangeLibrary.Services;

public class ExchangeRateRealtimeService : IExchangeRateRealtimeService
{
    public async Task<ExchangeRateRealtimeResponse?> GetExchangeRateRealtime(HttpClient client, string apiKey, string fromCurrency, string toCurrency)
    {
        var path = $"query?function=CURRENCY_EXCHANGE_RATE&from_currency={fromCurrency}&to_currency={toCurrency}&apikey={apiKey}";
        var request = new HttpRequestMessage(HttpMethod.Get, path);
        var response = await client.SendAsync(request);

        return await response.Content.ReadFromJsonAsync<ExchangeRateRealtimeResponse>();
    }
}