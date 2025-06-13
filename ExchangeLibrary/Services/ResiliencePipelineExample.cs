using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

var cancellationToken = new CancellationTokenSource().Token;

// Create an instance of builder that exposes various extensions for adding resilience strategies
ResiliencePipeline pipeline = new ResiliencePipelineBuilder()
    .AddCircuitBreaker(new CircuitBreakerStrategyOptions
    {
        FailureRatio = 0.5, // 50% failure threshold
        SamplingDuration = TimeSpan.FromSeconds(60), // Sample for 30 seconds
        MinimumThroughput = 2, // Minimum of 10 calls to consider the circuit state
        BreakDuration = TimeSpan.FromSeconds(15), // Break for 15 seconds after threshold is reached
        ShouldHandle = args =>
        {
            var ex = args.Outcome.Exception;
            return new ValueTask<bool>(
                ex is TimeoutRejectedException || ex is InvalidOperationException
            );
        }
    })
    .AddRetry(new RetryStrategyOptions()
    {
        MaxRetryAttempts = 1, // Retry up to 3 times
    }) // Add retry using the default options
    .AddTimeout(new TimeoutStrategyOptions
    {
        Timeout = TimeSpan.FromSeconds(5)
    })
    .Build(); // Builds the resilience pipeline

// Execute the pipeline asynchronously
for (int i = 0; i < 10; i++)
{
    try
    {
        await pipeline.ExecuteAsync(async (ct) => { await MethodToRun(ct); }, cancellationToken);
        Console.WriteLine($"Finished {i}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Operation failed at {i}, please try again.");
    }
    catch (BrokenCircuitException)
    {
        Console.WriteLine($"Operation failed too many times at {i}, please try again later.");
    }
    catch (TimeoutRejectedException ex)
    {
        Console.WriteLine($"Operation timed out at {i}, please try again later.");
    }
}

static async Task MethodToRun(CancellationToken ct)
{
    await Task.Delay(TimeSpan.FromSeconds(10), ct);
    Console.WriteLine("Method executed");
    throw new InvalidOperationException("test");
}
