public class MockHandler : HttpMessageHandler
{
    private Func<HttpRequestMessage, HttpResponseMessage> _responseGenerator;
    public MockHandler(Func<HttpRequestMessage, HttpResponseMessage> responseGenerator)
    {
        _responseGenerator = responseGenerator;
    }
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var response = _responseGenerator(request);
        response.RequestMessage = request;

        return Task.FromResult(response);
    }
}
