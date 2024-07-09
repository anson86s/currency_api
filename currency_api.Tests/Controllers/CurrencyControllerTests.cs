using currency_api.Controllers;
using currency_api.Models;
using currency_api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace currency_api.Tests.Controllers;

public class CurrencyControllerTests
{
    private readonly Mock<ICurrencyService> _mockCurrencyService;
    private readonly CurrencyController _controller;

    public CurrencyControllerTests()
    {
        _mockCurrencyService = new Mock<ICurrencyService>();
        _controller = new CurrencyController(_mockCurrencyService.Object);
    }

    [Fact]
    public async Task GetCurrencies_ReturnsSuccessResult_WithListOfCurrencies()
    {
        // Arrange
        var currencies = new List<Currency>
        {
            new Currency { Code = "USD", ChineseName = "美元" },
            new Currency { Code = "EUR", ChineseName = "歐元" }
        };
        _mockCurrencyService.Setup(service => service.GetAllCurrenciesAsync())
            .ReturnsAsync(currencies);

        // Act
        var result = await _controller.GetCurrencies();

        // Assert
        var actionResult = Assert.IsType<ActionResult<Result<IEnumerable<Currency>>>>(result);
        var objectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var successResult = Assert.IsType<Result<IEnumerable<Currency>>>(objectResult.Value);
        Assert.True(successResult.IsSuccess);
        Assert.Equal(currencies.Count, successResult.Data.Count());
    }

    [Fact]
    public async Task GetCurrency_ReturnsSuccessResult_WithCurrency()
    {
        // Arrange
        var currency = new Currency { Code = "USD", ChineseName = "美元" };
        _mockCurrencyService.Setup(service => service.GetCurrencyAsync("USD"))
            .ReturnsAsync(currency);

        // Act
        var result = await _controller.GetCurrency("USD");

        // Assert
        var actionResult = Assert.IsType<ActionResult<Result<Currency>>>(result);
        var objectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var successResult = Assert.IsType<Result<Currency>>(objectResult.Value);
        Assert.True(successResult.IsSuccess);
        Assert.Equal(currency.Code, successResult.Data.Code);
        Assert.Equal(currency.ChineseName, successResult.Data.ChineseName);
    }

    [Fact]
    public async Task GetCurrency_ReturnsFailResult_WhenCurrencyNotFound()
    {
        // Arrange
        _mockCurrencyService.Setup(service => service.GetCurrencyAsync("XYZ"))
            .ReturnsAsync((Currency)null);

        // Act
        var result = await _controller.GetCurrency("XYZ");

        // Assert
        var actionResult = Assert.IsType<ActionResult<Result<Currency>>>(result);
        var objectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var failResult = Assert.IsType<Result>(objectResult.Value);
        Assert.False(failResult.IsSuccess);
        Assert.Equal("1001", failResult.ReturnCode);
        Assert.Equal("查無資料", failResult.ReturnMessage);
    }

    [Fact]
    public async Task PostCurrency_ReturnsSuccessResult_WhenCurrencyAdded()
    {
        // Arrange
        var currency = new Currency { Code = "USD", ChineseName = "美元" };

        // 初次檢查資料庫，確認該貨幣不存在
        _mockCurrencyService.SetupSequence(service => service.GetCurrencyAsync(currency.Code))
            .ReturnsAsync((Currency)null) // 初次調用，貨幣不存在
            .ReturnsAsync(currency);      // 第二次調用，貨幣存在

        // 模擬添加貨幣
        _mockCurrencyService.Setup(service => service.AddCurrencyAsync(currency))
            .ReturnsAsync(1); // Mock that currency is successfully added

        // Act
        var result = await _controller.PostCurrency(currency);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Result<Currency>>>(result);
        var objectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var successResult = Assert.IsType<Result<Currency>>(objectResult.Value);
        Assert.True(successResult.IsSuccess);
        Assert.Equal(currency.Code, successResult.Data.Code);
        Assert.Equal(currency.ChineseName, successResult.Data.ChineseName);
    }

    [Fact]
    public async Task PostCurrency_ReturnsFailResult_WhenCurrencyExists()
    {
        // Arrange
        var currency = new Currency { Code = "USD", ChineseName = "美元" };
        _mockCurrencyService.Setup(service => service.GetCurrencyAsync(currency.Code))
            .ReturnsAsync(currency);

        // Act
        var result = await _controller.PostCurrency(currency);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Result<Currency>>>(result);
        var objectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var failResult = Assert.IsType<Result>(objectResult.Value);
        Assert.False(failResult.IsSuccess);
        Assert.Equal("1002", failResult.ReturnCode);
        Assert.Equal("新增失敗: 資料已存在", failResult.ReturnMessage);
    }

    [Fact]
    public async Task PutCurrency_ReturnsSuccessResult_WhenCurrencyUpdated()
    {
        // Arrange
        var currency = new Currency { Code = "USD", ChineseName = "美元" };
        _mockCurrencyService.Setup(service => service.UpdateCurrencyAsync(currency))
            .ReturnsAsync(1); // Mock that currency is successfully updated
        _mockCurrencyService.Setup(service => service.GetCurrencyAsync(currency.Code))
            .ReturnsAsync(currency); // Mock that currency can be retrieved after update

        // Act
        var result = await _controller.PutCurrency(currency);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Result<Currency>>>(result);
        var objectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var successResult = Assert.IsType<Result<Currency>>(objectResult.Value);
        Assert.True(successResult.IsSuccess);
        Assert.Equal(currency.Code, successResult.Data.Code);
        Assert.Equal(currency.ChineseName, successResult.Data.ChineseName);
    }

    [Fact]
    public async Task PutCurrency_ReturnsFailResult_WhenUpdateFails()
    {
        // Arrange
        var currency = new Currency { Code = "USD", ChineseName = "美元" };
        _mockCurrencyService.Setup(service => service.UpdateCurrencyAsync(currency))
            .ReturnsAsync(0);

        // Act
        var result = await _controller.PutCurrency(currency);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Result<Currency>>>(result);
        var objectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var failResult = Assert.IsType<Result>(objectResult.Value);
        Assert.False(failResult.IsSuccess);
        Assert.Equal("1003", failResult.ReturnCode);
        Assert.Equal("更新失敗", failResult.ReturnMessage);
    }

    [Fact]
    public async Task DeleteCurrency_ReturnsSuccessResult_WhenCurrencyDeleted()
    {
        // Arrange
        var code = "USD";
        _mockCurrencyService.Setup(service => service.DeleteCurrencyAsync(code))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.DeleteCurrency(code);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        var successResult = Assert.IsType<Result>(objectResult.Value);
        Assert.True(successResult.IsSuccess);
    }

    [Fact]
    public async Task DeleteCurrency_ReturnsFailResult_WhenDeleteFails()
    {
        // Arrange
        var code = "USD";
        _mockCurrencyService.Setup(service => service.DeleteCurrencyAsync(code))
            .ReturnsAsync(0);

        // Act
        var result = await _controller.DeleteCurrency(code);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        var failResult = Assert.IsType<Result>(objectResult.Value);
        Assert.False(failResult.IsSuccess);
        Assert.Equal("1004", failResult.ReturnCode);
        Assert.Equal("刪除失敗", failResult.ReturnMessage);
    }
}

