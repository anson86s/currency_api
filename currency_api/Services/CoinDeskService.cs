using currency_api.Models;
using Newtonsoft.Json;

namespace currency_api.Services;

public class CoinDeskService : ICoinDeskService
{
    private readonly HttpClient _httpClient;
    private readonly string? coindeskUrl;
    private readonly ILogger<CoinDeskService> _logger;

    public CoinDeskService(IHttpClientFactory httpClientFactory, ILogger<CoinDeskService> logger, IConfiguration config)
    {
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
        coindeskUrl = config["Endpoint:CoinDesk"]?.ToString();
    }

    public async Task<CoinDeskResponse?> GetCoinDeskData()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://api.coindesk.com/v1/bpi/currentprice.json");
        _logger.LogInformation($"External Request: {request.Method} {request.RequestUri}");

        var response = await _httpClient.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        _logger.LogInformation($"External Response: {responseBody}");

        return JsonConvert.DeserializeObject<CoinDeskResponse>(responseBody);

        //var response = await _httpClient.GetStringAsync(coindeskUrl);
        //return JsonConvert.DeserializeObject<CoinDeskResponse>(response);


        //var request = new HttpRequestMessage(HttpMethod.Get, coindeskUrl);      
        //var response = await _httpClient.SendAsync(request);
        //var responseBody = await response.Content.ReadAsStringAsync();
        //return JsonConvert.DeserializeObject<CoinDeskResponse>(responseBody)
    }
}

