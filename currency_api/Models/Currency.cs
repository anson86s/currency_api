using System.ComponentModel.DataAnnotations;

namespace currency_api.Models;

public class Currency
{
    [Key]
    public required string Code { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "ChineseName 未輸入")]
    public required string ChineseName { get; set; }
}

