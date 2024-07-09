namespace currency_api.Models;

public class ExRateResponse
{
    public string? UpdatedTime { get; set; }
    public IEnumerable<CurrencyInfo>? CurrencyInfo { get; set; }
}

public class CurrencyInfo
{
    public string? Code { get; set; }
    public string? ChineseName { get; set; }
    public string? Rate { get; set; }
}

