namespace currency_api.Models;

public class CoinDeskResponse
{
    public UpdatedTime Time { get; set; }
    public Dictionary<string, CurrencyData> Bpi { get; set; }
}

public class UpdatedTime
{
    public DateTime UpdatedISO { get; set; }
}

public class CurrencyData
{
    public string Code { get; set; }
    public string Rate { get; set; }
}

