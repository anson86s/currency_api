using currency_api.Models;
using currency_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace currency_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoinDeskController : BaseController<CoinDeskController>
{
    private readonly ICoinDeskService _coinDeskService;
    private readonly ICurrencyService _currencyService;

    public CoinDeskController(ICoinDeskService coinDeskService, ICurrencyService currencyService)
    {
        _coinDeskService = coinDeskService;
        _currencyService = currencyService;
    }

    [HttpGet("CurrentExchangeRate")]
    public async Task<ActionResult<Result<ExRateResponse>>> GetCurrentExchangeRate()
    {
        var coinDeskData = await _coinDeskService.GetCoinDeskData();
        var updatedTime = coinDeskData?.Time.UpdatedISO.ToString("yyyy/MM/dd HH:mm:ss");

        var currencies = await _currencyService.GetAllCurrenciesAsync();
        var currencyInfo = coinDeskData?.Bpi.Select(bpi => new CurrencyInfo
        {
            Code = bpi.Value.Code,
            ChineseName = currencies.FirstOrDefault(c => c.Code == bpi.Value.Code)?.ChineseName,
            Rate = bpi.Value.Rate
        }).OrderBy(c => c.Code);

        var response = new ExRateResponse
        {
            UpdatedTime = updatedTime,
            CurrencyInfo = currencyInfo
        };

        return Success(response);
    }
}

