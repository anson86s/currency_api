using currency_api.Models;
using Microsoft.EntityFrameworkCore;

namespace currency_api.Data;

public class CurrencyContext : DbContext
{
    public DbSet<Currency> Currencies { get; set; }

    public CurrencyContext(DbContextOptions<CurrencyContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>().ToTable("Currency");

        // 設定 Code 為主鍵
        modelBuilder.Entity<Currency>().HasKey(c => c.Code);

        // 配置初始資料
        modelBuilder.Entity<Currency>().HasData(
            new Currency { Code = "USD", ChineseName = "美元" },
            new Currency { Code = "GBP", ChineseName = "英鎊" },
            new Currency { Code = "EUR", ChineseName = "歐元" }
        );
    }
}

