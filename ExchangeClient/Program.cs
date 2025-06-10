using ExchangeClient.Components;
using ExchangeLibrary.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient();
builder.Services.AddHttpClient("alpha-vantage", c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ExchangeAPI"));
});
builder.Services.AddHttpClient<IExchangeRateRealtimeService, ExchangeRateRealtimeService>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ExchangeAPI"));
});

builder.Configuration.GetSection("AlphaVantageApiKey");

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
