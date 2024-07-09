using currency_api.Models;
using currency_api.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace currency_api.Tests.Services;

public class CoinDeskServiceTests
{
    [Fact]
    public async Task GetCoinDeskData_ReturnsCoinDeskResponse()
    {
        // Arrange
        var expectedResponse = new CoinDeskResponse
        {
            Time = new UpdatedTime { UpdatedISO = DateTime.UtcNow },
            Bpi = new Dictionary<string, CurrencyData>
            {
                { "USD", new CurrencyData { Code = "USD", Rate = "1000.00" } }
            }
        };

        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = System.Net.HttpStatusCode.OK,
                   Content = new StringContent(JsonConvert.SerializeObject(new
                   {
                       time = new { updatedISO = expectedResponse.Time.UpdatedISO },
                       bpi = new Dictionary<string, object>
                       {
                           { "USD", new { code = expectedResponse.Bpi["USD"].Code, rate = expectedResponse.Bpi["USD"].Rate } }
                       }
                   }))
               });

        var httpClient = new HttpClient(handler.Object);
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(c => c["Endpoint:CoinDesk"]).Returns("https://api.coindesk.com/v1/bpi/currentprice.json");

        var service = new CoinDeskService(httpClient, configMock.Object);

        // Act
        var result = await service.GetCoinDeskData();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Time.UpdatedISO, result.Time.UpdatedISO);
        Assert.Single(result.Bpi);
        Assert.Equal(expectedResponse.Bpi["USD"].Code, result.Bpi["USD"].Code);
        Assert.Equal(expectedResponse.Bpi["USD"].Rate, result.Bpi["USD"].Rate);
    }
}

