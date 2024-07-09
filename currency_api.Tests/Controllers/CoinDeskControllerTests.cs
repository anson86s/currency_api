using currency_api.Controllers;
using currency_api.Models;
using currency_api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace currency_api.Tests.Controllers;

public class CoinDeskControllerTests
{
    [Fact]
    public async Task GetCurrentExchangeRate_Returns_Success()
    {
        // Arrange
        var coinDeskData = new CoinDeskResponse
        {
            Time = new UpdatedTime { UpdatedISO = DateTime.UtcNow },
            Bpi = new Dictionary<string, CurrencyData>
            {
                { "USD", new CurrencyData { Code = "USD", Rate = "1000.00" } },
                { "EUR", new CurrencyData { Code = "EUR", Rate = "1200.00" } }
            }
        };

        var currencies = new List<Currency>
        {
            new Currency { Code = "USD", ChineseName = "美元" },
            new Currency { Code = "EUR", ChineseName = "歐元" }
        };

        var coinDeskServiceMock = new Mock<ICoinDeskService>();
        coinDeskServiceMock.Setup(s => s.GetCoinDeskData()).ReturnsAsync(coinDeskData);

        var currencyServiceMock = new Mock<ICurrencyService>();
        currencyServiceMock.Setup(s => s.GetAllCurrenciesAsync()).ReturnsAsync(currencies);

        var controller = new CoinDeskController(coinDeskServiceMock.Object, currencyServiceMock.Object);

        // Act
        var actionResult = await controller.GetCurrentExchangeRate();
        var result = actionResult.Result as OkObjectResult;
        var exRateResponse = result.Value as Result<ExRateResponse>;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(exRateResponse);
        Assert.Equal(200, result.StatusCode);

        var updatedTime = coinDeskData.Time.UpdatedISO.ToString("yyyy/MM/dd HH:mm:ss");
        Assert.Equal(updatedTime, exRateResponse.Data.UpdatedTime);

        var expectedCurrencyInfos = coinDeskData.Bpi.Select(bpi => new CurrencyInfo
        {
            Code = bpi.Value.Code,
            ChineseName = currencies.FirstOrDefault(c => c.Code == bpi.Value.Code)?.ChineseName,
            Rate = bpi.Value.Rate
        }).OrderBy(c => c.Code);

        Assert.Equal(expectedCurrencyInfos, exRateResponse.Data.CurrencyInfo, new CurrencyInfoComparer());
    }
}

public class CurrencyInfoComparer : IEqualityComparer<CurrencyInfo>
{
    public bool Equals(CurrencyInfo x, CurrencyInfo y)
    {
        return x.Code == y.Code && x.ChineseName == y.ChineseName && x.Rate == y.Rate;
    }

    public int GetHashCode(CurrencyInfo obj)
    {
        return obj.GetHashCode();
    }
}

