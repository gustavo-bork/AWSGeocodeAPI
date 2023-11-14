using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using AWSGeocodeAPI.Services;

namespace AWSGeocodeAPI.Controllers;

[Route("[controller]")]
public class GeocodeController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IDistributedCache _cache;
    private readonly SecretsManagerService _secretsManagerService;

    public GeocodeController(
        IHttpClientFactory httpClientFactory,
        IDistributedCache cache,
        SecretsManagerService secretsManagerService
    )
    {
        _httpClient = httpClientFactory.CreateClient("GoogleMaps");
        _cache = cache;
        _secretsManagerService = secretsManagerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetInfoAsync(string address)
    {
        try
        {
            var addressData = await _cache.GetStringAsync(address);
            if (!string.IsNullOrEmpty(addressData))
            {
                return Ok(addressData);
            }

            string apiKey = await _secretsManagerService.GetSecret("GoogleMapsAPIKey");
            var response = await _httpClient.GetStringAsync($"?address={address}&key={apiKey}");

            const int secondsInMinute = 60, minutesInHour = 60, hoursInDay = 24, daysToExpire = 30;
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(daysToExpire * hoursInDay * minutesInHour * secondsInMinute)
            };
            await _cache.SetStringAsync(address, response, options);

            return Ok(response);
        }
        catch (Exception)
        {
            throw;
        }
    }
}