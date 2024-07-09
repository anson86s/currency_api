using currency_api.Models;

namespace currency_api.Repositories;

public interface ICurrencyRepository
{
    Task<IEnumerable<Currency>> GetAllCurrenciesAsync();
    Task<Currency?> GetCurrencyAsync(string code);
    Task<int> AddCurrencyAsync(Currency currency);
    Task<int> UpdateCurrencyAsync(Currency currency);
    Task<int> DeleteCurrencyAsync(string code);
}

