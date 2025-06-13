using System.Net.Http.Headers;
using ExchangeClient.Components;
using ExchangeLibrary.Services;
using Microsoft.Extensions.Caching.Memory;
using Polly;
using Polly.CircuitBreaker;
using Polly.Registry;
using Polly.Retry;
using Polly.Timeout;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddResiliencePipeline("default", x =>
    x
    .AddCircuitBreaker(new CircuitBreakerStrategyOptions
    {
        FailureRatio = 0.5, // 50% failure threshold
        SamplingDuration = TimeSpan.FromSeconds(60),
        MinimumThroughput = 2, // Minimum of calls
        BreakDuration = TimeSpan.FromSeconds(15), // Break for 15 seconds after threshold is reached
        ShouldHandle = new PredicateBuilder().Handle<Exception>()
    })
    .AddRetry(new RetryStrategyOptions()
    {
        MaxRetryAttempts = 2,
        BackoffType = DelayBackoffType.Exponential,
        UseJitter = true,
        Delay = TimeSpan.FromSeconds(2),
        ShouldHandle = new PredicateBuilder().Handle<Exception>()
    })
    .AddTimeout(new TimeoutStrategyOptions
    {
        Timeout = TimeSpan.FromSeconds(5)
    }));

builder.Services.AddMemoryCache(options =>
options.SizeLimit = 1_000);

builder.Configuration.GetSection("AlphaVantageApiKey");

builder.Services.AddHttpClient();
builder.Services.AddHttpClient("alpha-vantage", c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ExchangeAPI"));
    c.DefaultRequestHeaders
      .Accept
      .Add(new MediaTypeWithQualityHeaderValue("application/json"));
});
builder.Services.AddHttpClient<ExchangeRateRealtimeService>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ExchangeAPI"));
    c.DefaultRequestHeaders
      .Accept
      .Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddTransient<ExchangeRateRealtimeServiceCached>(sp =>
{
    var inner = sp.GetRequiredService<ExchangeRateRealtimeService>();
    var cache = sp.GetRequiredService<IMemoryCache>();
    return new ExchangeRateRealtimeServiceCached(cache, inner);
});
builder.Services.AddTransient<IExchangeRateRealtimeService>(sp =>
{
    var cached = sp.GetRequiredService<ExchangeRateRealtimeServiceCached>();
    var pipelineProvider = sp.GetRequiredService<ResiliencePipelineProvider<string>>();
    return new ExchangeRateRealtimeServiceResilience(cached, pipelineProvider);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
