using currency_api.Data;
using currency_api.Models;
using Microsoft.EntityFrameworkCore;

namespace currency_api.Repositories;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly CurrencyContext _context;

    public CurrencyRepository(CurrencyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Currency>> GetAllCurrenciesAsync()
    {
        return await _context.Currencies.ToListAsync();
    }

    public async Task<Currency?> GetCurrencyAsync(string code)
    {
        return await _context.Currencies.FindAsync(code);
    }

    public async Task<int> AddCurrencyAsync(Currency currency)
    {
        _context.Currencies.Add(currency);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateCurrencyAsync(Currency currency)
    {
        _context.Entry(currency).State = EntityState.Modified;
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteCurrencyAsync(string code)
    {
        var currency = await _context.Currencies.FindAsync(code);
        if (currency != null)
        {
            _context.Currencies.Remove(currency);
            return await _context.SaveChangesAsync();
        }

        return 0;
    }
}

