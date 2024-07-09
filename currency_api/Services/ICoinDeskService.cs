using currency_api.Models;

namespace currency_api.Services;

public interface ICoinDeskService
{
    Task<CoinDeskResponse?> GetCoinDeskData();
}

