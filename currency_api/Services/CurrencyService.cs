using currency_api.Models;
using currency_api.Repositories;

namespace currency_api.Services;

public class CurrencyService : ICurrencyService
{
    private readonly ICurrencyRepository _repository;

    public CurrencyService(ICurrencyRepository repository)
	{
        _repository = repository;
	}

    public async Task<IEnumerable<Currency>> GetAllCurrenciesAsync()
    {
        return await _repository.GetAllCurrenciesAsync();
    }

    public async Task<Currency?> GetCurrencyAsync(string code)
    {
        return await _repository.GetCurrencyAsync(code);
    }

    public async Task<int> AddCurrencyAsync(Currency currency)
    {
        return await _repository.AddCurrencyAsync(currency);
    }

    public async Task<int> UpdateCurrencyAsync(Currency currency)
    {
        return await _repository.UpdateCurrencyAsync(currency);
    }

    public async Task<int> DeleteCurrencyAsync(string code)
    {
        return await _repository.DeleteCurrencyAsync(code);
    }
}

