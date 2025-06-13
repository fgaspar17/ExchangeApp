using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeLibrary.Models;
using Polly.Registry;

namespace ExchangeLibrary.Services;

public class ExchangeRateRealtimeServiceResilience : IExchangeRateRealtimeService
{
    private readonly IExchangeRateRealtimeService _service;
    private readonly ResiliencePipelineProvider<string> _pipelineProvider;

    public ExchangeRateRealtimeServiceResilience(IExchangeRateRealtimeService service, 
        ResiliencePipelineProvider<string> pipelineProvider)
    {
        _service = service;
        _pipelineProvider = pipelineProvider;
    }

    public async Task<ExchangeRateRealtimeResponse?> GetExchangeRateRealtime(string apiKey, 
        string fromCurrency, string toCurrency, CancellationToken ct)
    {
        var pipeline = _pipelineProvider.GetPipeline("default");
        var result = await pipeline.ExecuteAsync<ExchangeRateRealtimeResponse?>(async (token) =>
        {
            return await _service.GetExchangeRateRealtime(apiKey, fromCurrency, toCurrency, token);
        }, ct);

        return result;
    }
}
