using currency_api.Models;
using currency_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace currency_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CurrencyController : BaseController<CurrencyController>
{
    private readonly ICurrencyService _currencyService;

    public CurrencyController(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    [HttpGet("GetCurrencies")]
    public async Task<ActionResult<Result<IEnumerable<Currency>>>> GetCurrencies()
    {
        var currencies = await _currencyService.GetAllCurrenciesAsync();
        return Success(currencies);
    }

    [HttpGet("GetCurrency/{code}")]
    public async Task<ActionResult<Result<Currency>>> GetCurrency(string code)
    {
        var currency = await _currencyService.GetCurrencyAsync(code);

        if (currency == null)
        {
            return Fail("1001", "查無資料");
        }

        return Success(currency);
    }

    [HttpPost("AddCurrency")]
    public async Task<ActionResult<Result<Currency>>> PostCurrency(Currency currency)
    {
        var data = await _currencyService.GetCurrencyAsync(currency.Code);

        if (data != null)
        {
            return Fail("1002", "新增失敗: 資料已存在");
        }

        int count = await _currencyService.AddCurrencyAsync(currency);
        if (count == 0)
        {
            return Fail("1002", "新增失敗");
        }
        
        return await GetCurrency(currency.Code);
    }

    [HttpPut("UpdateCurrency")]
    public async Task<ActionResult<Result<Currency>>> PutCurrency(Currency currency)
    {
        int count = await _currencyService.UpdateCurrencyAsync(currency);
        if (count == 0)
        {
            return Fail("1003", "更新失敗");
        }
        
        return await GetCurrency(currency.Code);
    }

    [HttpDelete("DeleteCurrency/{code}")]
    public async Task<IActionResult> DeleteCurrency(string code)
    {
        int count = await _currencyService.DeleteCurrencyAsync(code);
        if (count == 0)
        {
            return Fail("1004", "刪除失敗");
        }
        return Success();
    }
}

