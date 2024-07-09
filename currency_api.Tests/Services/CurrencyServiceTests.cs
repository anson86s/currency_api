using currency_api.Models;
using currency_api.Repositories;
using currency_api.Services;
using Moq;

namespace currency_api.Tests.Services;

public class CurrencyServiceTests
{
    private readonly Mock<ICurrencyRepository> _repositoryMock;
    private readonly CurrencyService _service;

    public CurrencyServiceTests()
    {
        _repositoryMock = new Mock<ICurrencyRepository>();
        _service = new CurrencyService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllCurrenciesAsync_ReturnsAllCurrencies()
    {
        // Arrange
        var currencies = new List<Currency>
        {
            new Currency { Code = "USD", ChineseName = "美元" },
            new Currency { Code = "EUR", ChineseName = "歐元" }
        };
        _repositoryMock.Setup(repo => repo.GetAllCurrenciesAsync()).ReturnsAsync(currencies);

        // Act
        var result = await _service.GetAllCurrenciesAsync();

        // Assert
        Assert.Equal(currencies, result);
    }

    [Fact]
    public async Task GetCurrencyAsync_ReturnsCurrency()
    {
        // Arrange
        var currency = new Currency { Code = "USD", ChineseName = "美元" };
        _repositoryMock.Setup(repo => repo.GetCurrencyAsync("USD")).ReturnsAsync(currency);

        // Act
        var result = await _service.GetCurrencyAsync("USD");

        // Assert
        Assert.Equal(currency, result);
    }

    [Fact]
    public async Task AddCurrencyAsync_AddsCurrency()
    {
        // Arrange
        var currency = new Currency { Code = "USD", ChineseName = "美元" };
        _repositoryMock.Setup(repo => repo.AddCurrencyAsync(currency)).ReturnsAsync(1);

        // Act
        var result = await _service.AddCurrencyAsync(currency);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task UpdateCurrencyAsync_UpdatesCurrency()
    {
        // Arrange
        var currency = new Currency { Code = "USD", ChineseName = "美元" };
        _repositoryMock.Setup(repo => repo.UpdateCurrencyAsync(currency)).ReturnsAsync(1);

        // Act
        var result = await _service.UpdateCurrencyAsync(currency);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task DeleteCurrencyAsync_DeletesCurrency()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.DeleteCurrencyAsync("USD")).ReturnsAsync(1);

        // Act
        var result = await _service.DeleteCurrencyAsync("USD");

        // Assert
        Assert.Equal(1, result);
    }
}

