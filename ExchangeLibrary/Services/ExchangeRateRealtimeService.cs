using System.Net.Http.Json;
using System.Text.Json;
using ExchangeLibrary.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeLibrary.Services;

public class ExchangeRateRealtimeService : IExchangeRateRealtimeService
{
    private readonly HttpClient _client;
    private readonly ILogger<ExchangeRateRealtimeService> _logger;

    public ExchangeRateRealtimeService(HttpClient client, ILogger<ExchangeRateRealtimeService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<ExchangeRateRealtimeResponse?> GetExchangeRateRealtimeAsync(string apiKey, 
        string fromCurrency, string toCurrency, CancellationToken ct)
    {
        try
        {
            var path = $"query?function=CURRENCY_EXCHANGE_RATE&from_currency={fromCurrency}&to_currency={toCurrency}&apikey={apiKey}";
            var request = new HttpRequestMessage(HttpMethod.Get, path);

            _logger.LogDebug("Sending request to Exchange Rate API: {Url}", path);

            var response = await _client.SendAsync(request, ct);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Exchange Rate API responded with status code {StatusCode} for {From}->{To}", response.StatusCode, fromCurrency, toCurrency);
            }
            else
            {
                _logger.LogInformation("Exchange rate retrieved successfully: {From}->{To}", fromCurrency, toCurrency);
            }

            var result = await response.Content.ReadFromJsonAsync<ExchangeRateRealtimeResponse>(cancellationToken: ct);

            if (result == null)
            {
                _logger.LogWarning("Exchange Rate API returned null response for {From}->{To}", fromCurrency, toCurrency);
            }

            return result;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Exchange rate request cancelled for {From}->{To}", fromCurrency, toCurrency);
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "Invalid JSON response received from Exchange Rate API for {From}->{To}", fromCurrency, toCurrency);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get exchange rate from {From}->{To}", fromCurrency, toCurrency);
            throw;
        }
    }
}