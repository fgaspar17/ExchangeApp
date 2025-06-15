using ExchangeLibrary.Models;
using ExchangeLibrary.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeLibrary.Tests;

public class ExchangeRateRealtimeServiceCachedTests
{
    [Fact]
    public async Task GetExchangeRateRealtimeAsync_ShouldCallService_WhenCacheMiss()
    {
        // Arrange
        var expectedResponse = new ExchangeRateRealtimeResponse
        {
            RealtimeCurrencyExchangeRate = new RealtimeCurrencyExchangeRate
            {
                FromCurrencyCode = "USD",
                ToCurrencyCode = "EUR",
                ExchangeRate = "0.9253"
            }
        };

        var cache = new MemoryCache(new MemoryCacheOptions());
        var mockInnerService = new Mock<IExchangeRateRealtimeService>();
        var logger = new Mock<ILogger<ExchangeRateRealtimeServiceCached>>();

        mockInnerService
            .Setup(s => s.GetExchangeRateRealtimeAsync("apikey", "USD", "EUR", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var cachingService = new ExchangeRateRealtimeServiceCached(cache, mockInnerService.Object, logger.Object);

        // Act
        var result = await cachingService.GetExchangeRateRealtimeAsync("apikey", "USD", "EUR", CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("0.9253", result.RealtimeCurrencyExchangeRate.ExchangeRate);
        mockInnerService.Verify(s => s.GetExchangeRateRealtimeAsync("apikey", "USD", "EUR", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetExchangeRateRealtimeAsync_ShouldReturnCachedValue_WhenCacheHit()
    {
        // Arrange
        var expectedResponse = new ExchangeRateRealtimeResponse
        {
            RealtimeCurrencyExchangeRate = new RealtimeCurrencyExchangeRate
            {
                FromCurrencyCode = "USD",
                ToCurrencyCode = "EUR",
                ExchangeRate = "0.9253"
            }
        };

        var cache = new MemoryCache(new MemoryCacheOptions());
        var mockInnerService = new Mock<IExchangeRateRealtimeService>();
        var logger = new Mock<ILogger<ExchangeRateRealtimeServiceCached>>();

        var cachingService = new ExchangeRateRealtimeServiceCached(cache, mockInnerService.Object, logger.Object);

        // Prime the cache
        await cache.GetOrCreateAsync("USDEUR", entry =>
        {
            entry.SetSlidingExpiration(TimeSpan.FromMinutes(1));
            return Task.FromResult(expectedResponse);
        });

        // Act
        var result = await cachingService.GetExchangeRateRealtimeAsync("apikey", "USD", "EUR", CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("0.9253", result.RealtimeCurrencyExchangeRate.ExchangeRate);
        mockInnerService.Verify(s => s.GetExchangeRateRealtimeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

}