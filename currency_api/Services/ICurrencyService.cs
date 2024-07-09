using currency_api.Models;

namespace currency_api.Services;

public interface ICurrencyService
{
    Task<IEnumerable<Currency>> GetAllCurrenciesAsync();
    Task<Currency?> GetCurrencyAsync(string code);
    Task<int> AddCurrencyAsync(Currency currency);
    Task<int> UpdateCurrencyAsync(Currency currency);
    Task<int> DeleteCurrencyAsync(string code);
}

