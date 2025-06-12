using System.Net.Http.Headers;
using ExchangeClient.Components;
using ExchangeLibrary.Services;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMemoryCache();

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

builder.Services.AddTransient<IExchangeRateRealtimeService>(sp =>
{
    var inner = sp.GetRequiredService<ExchangeRateRealtimeService>();
    var cache = sp.GetRequiredService<IMemoryCache>();
    return new ExchangeRateRealtimeServiceCached(cache, inner);
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
