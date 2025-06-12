using System.Net.Http.Json;
using ExchangeLibrary.Models;

namespace ExchangeLibrary.Services;

public class ExchangeRateRealtimeService : IExchangeRateRealtimeService
{
    private readonly HttpClient _client;

    public ExchangeRateRealtimeService(HttpClient client)
    {
        _client = client;
    }

    public async Task<ExchangeRateRealtimeResponse?> GetExchangeRateRealtime(string apiKey, string fromCurrency, string toCurrency)
    {
        await Task.Delay(TimeSpan.FromSeconds(30));
        var path = $"query?function=CURRENCY_EXCHANGE_RATE&from_currency={fromCurrency}&to_currency={toCurrency}&apikey={apiKey}";
        var request = new HttpRequestMessage(HttpMethod.Get, path);
        var response = await _client.SendAsync(request);

        return await response.Content.ReadFromJsonAsync<ExchangeRateRealtimeResponse>()!;
    }
}