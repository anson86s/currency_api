using currency_api.Data;
using currency_api.Filters;
using currency_api.Handler;
using currency_api.Middlewares;
using currency_api.Repositories;
using currency_api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddDebug();
//builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddDbContext<CurrencyContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add HttpClient with LoggingHandler
//builder.Services.AddTransient<LoggingHandler>();
//builder.Services.AddHttpClient<ICoinDeskService, CoinDeskService>()
//    .AddHttpMessageHandler<LoggingHandler>();

// Add HttpClient with LoggingHandler
builder.Services.AddTransient<LoggingHandler>();
builder.Services.AddHttpClient<CoinDeskService>()
    .AddHttpMessageHandler<LoggingHandler>();

//builder.Services.AddTransient<LoggingHandler>();
//builder.Services.AddHttpClient("HttpMessageHandler")
//    .AddHttpMessageHandler<LoggingHandler>();
//builder.Services.AddHttpClient();

builder.Services.AddHttpLogging(o => { });

// Add Repository/Service
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<ICoinDeskService, CoinDeskService>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new GlobalExceptionFilter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpLogging();

// Middleware configurations
app.UseMiddleware<RequestResponseLoggingMiddleware>();
//app.UseMiddleware<ErrorHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

