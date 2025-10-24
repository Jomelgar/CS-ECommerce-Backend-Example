namespace Controllers;

using Microsoft.AspNetCore.Mvc;
using Polly.Bulkhead;
using Polly;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly HttpClient _bulkhead;

    public TestController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("PollyHttpClient");

        _bulkhead = httpClientFactory.CreateClient("Bulkhead");
    }

    [HttpGet("CircuitBreaker")]
    public async Task<IActionResult> TestCircuitPolicy()
    {
        try
        {
            var response = await _httpClient.GetAsync("http://localhost:5132/Inventory");
            response.EnsureSuccessStatusCode();
            return Ok("Solicitud exitosa");
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("Bulkhead")]
    public async Task<IActionResult> TestBulkhead()
    {
        var tasks = new List<Task>();

        for (int i = 0; i < 10; i++)
        {
            int CallId = i + 1;
            tasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            var response = await _bulkhead.GetAsync("http://localhost:5132/Inventory");
                            Console.WriteLine($"Call {CallId}: Status {response.StatusCode}, Response: {response}");
                        }
                        catch (HttpRequestException ex)
                        {
                            Console.WriteLine($"Call {CallId}: Exception {ex.Message}");
                        }
                    }
                )
            );
        }

        await Task.WhenAll(tasks);

        return Ok(new { });
    }
}
