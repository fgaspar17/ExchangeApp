# ExchangeApp

**ExchangeApp** is a currency exchange application built with .NET 9 and Blazor Server. It also includes a reusable .NET library that fetches real-time currency exchange data. The project leverages modern .NET features like `HttpClientFactory`, `Polly` for resilience, `MemoryCache` for caching, and `Serilog` for structured logging.

This project demonstrates clean architecture, robust error handling, and testable, resilient service integration patterns.

## Tech Stack

- **.NET 9 Blazor Server**
- **MemoryCache** – in-memory caching strategy
- **Polly** – transient fault handling and resilience policies
- **Serilog** – structured and enriched logging

## Goals

  - [x] Learn effective usage of HttpClient in modern .NET applications
  - [x] Integrate Polly for retries, timeouts, and circuir breaks
  - [x] Apply `MemoryCache` for efficient data reuse and evaluate trade-offs
  - [x] Explore robust error handling and fault-tolerant communication with external APIs
  - [x] Get introduced to Blazor Server as a stepping stone for future UI projects

## Features

- Currency Exchange

  - Retrieve the current exchange rate between the currencies selected from the dropdown.
  - Sends a request to the external API Alpha Vantage to fetch live data.
  - ![image](https://github.com/user-attachments/assets/c7f71dfd-5e03-415f-9d37-46a89cdea113)


- Caching

  - Responses are cached in-memory to minimize calls to the external API and stay within the free usage limit (25 requests).
  - Improves performance by serving repeated requests from cache.

- Resilience

  - Uses Polly to implement robust retry policies with a 5-second timeout on transient errors.
  - Includes a Circuit Breaker policy to prevent overloading the external API when repeated failures occur.
  - Ensures high availability and graceful degradation of the service.

## Testing

- Comprehensive unit testing implemented with **xUnit**.
- Custom `HttpMessageHandler` mocks enable precise simulation of HTTP responses for `HttpClient`-based services.

## Challenges

- Building responsive Blazor forms for real-time data display.
- Configuring Dependency Injection to support decorators and properly integrate `IHttpClientFactory`.
- Setting up and tuning Polly's ResiliencePipeline for retries and circuit breaking.
- Implementing efficient caching to reduce redundant external API calls.
- Designing the system to keep cache, resilience policies, and service layers loosely coupled and maintainable.

## Lessons Learned

- Understanding how **HttpClient** processes requests through the **HttpMessageHandler** pipeline.
- Setting up and configuring a **Blazor Server** project from scratch.
- When and how to effectively use **MemoryCache** for caching data in .NET.
- Creating custom **DelegatingHandlers** to implement caching and other cross-cutting concerns.
- Building custom **HttpMessageHandlers** to mock `HttpClient` for unit testing.
- Configuring and using **Polly’s ResiliencePipeline** for retries, circuit breaking, and timeout policies.
- Applying the **Decorator Pattern** with Dependency Injection to extend service behavior.
- Leveraging **HttpClientFactory** for managing `HttpClient` lifetimes and handler chains.

## Areas to Improve

- Deepen understanding of networking concepts in .NET and general HTTP client-server communication.
- Expand test coverage (unit tests and edge case validations).
- Explore distributed caching solutions such as Redis to support scalability and sticky sessions across multiple servers.
- Investigate and experiment with alternative resilience policies and strategies beyond Polly’s defaults.
- Improve skills in Blazor, HTML5, and CSS3 to build richer, more responsive user interfaces.

## Resources used

- StackOverflow posts
- ChatGPT
- [DelegatingHandler Video](https://youtu.be/goxI3rOMnmY)
- [HttpClientFactory Typed Clients Video](https://youtu.be/g-JGay_lnWI)
- [Blazor HttpClient Implementation Video](https://youtu.be/cwgck1k0YKU)
- [Microsoft HttpClient .NET Documentation](https://learn.microsoft.com/es-es/dotnet/api/system.net.http.httpclient?view=net-8.0)
- [Implementing IMemoryCache Video](https://youtu.be/KSRlVOgVxyI)
- [Polly Documentation](https://www.pollydocs.org/index.html)
