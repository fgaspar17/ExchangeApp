using System.Net.Http.Headers;
using System.Net.Http.Json;
using ExchangeLibrary.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeLibrary.Tests
{
    public class ExchangeRateRealtimeServiceTests
    {
        private readonly ILogger<ExchangeRateRealtimeService> _logger;
        public ExchangeRateRealtimeServiceTests()
        {
            var logger = new Mock<ILogger<ExchangeRateRealtimeService>>().Object;
            _logger = logger;
        }

        [Fact]
        public async Task GetExchangeRateRealtimeAsync_ShouldReturnValidResponse_WhenValidRequest()
        {
            // Arrange
            var jsonString = """
{
  "Realtime Currency Exchange Rate": {
    "1. From_Currency Code": "USD",
    "2. From_Currency Name": "United States Dollar",
    "3. To_Currency Code": "EUR",
    "4. To_Currency Name": "Euro",
    "5. Exchange Rate": "0.9253",
    "6. Last Refreshed": "2025-06-07 12:00:00",
    "7. Time Zone": "UTC"
  }
}
""";

            var mockedHandler = new MockHandler(request =>
            {
                var response = new HttpResponseMessage()
                {
                    Content = new StringContent(jsonString),
                    StatusCode = System.Net.HttpStatusCode.OK,
                };

                return response;
            });
            var client = new HttpClient(mockedHandler);
            client.BaseAddress = new Uri("https://www.example.co/");
            var ct = new CancellationTokenSource().Token;

            // Act
            var service = new ExchangeRateRealtimeService(client, _logger);
            var result = await service.GetExchangeRateRealtimeAsync(apiKey: string.Empty,
                fromCurrency: string.Empty, toCurrency: string.Empty, ct);

            // Asserts
            Assert.Equal(result.RealtimeCurrencyExchangeRate.ExchangeRate, "0.9253");
        }
    }
}
