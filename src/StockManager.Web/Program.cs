using MudBlazor.Services;
using StockManager.Core.Repositories;
using StockManager.Core.Services;
using StockManager.Core.Utils;

namespace StockManager.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddMudServices();
            builder.Services.AddSingleton<DashboardService>();
            builder.Services.AddSingleton<FundsService>();
            builder.Services.AddSingleton<StockService>();
            builder.Services.AddSingleton<StockTransactionService>();
            builder.Services.AddSingleton<InvestmentTrustService>();
            builder.Services.AddScoped<IFundsRepository, DatabaseFundsRepository>();
            builder.Services.AddScoped<IStockHistoryRepository, DatabaseStockHistoryRepository>();
            builder.Services.AddScoped<IStockRepository, DatabaseStockRepository>();
            builder.Services.AddScoped<IInvestmentTrustHistoryRepository, DatabaseInvestmentTrustHistoryRepository>();
            builder.Services.AddAutoMapper(typeof(MapperProfile));

            if (!builder.Environment.IsProduction())
            {
                builder.Services.AddSingleton<IFundsRepository, StubFundsRepository>();
                builder.Services.AddSingleton<IStockHistoryRepository, StubStockHistoryRepository>();
                builder.Services.AddSingleton<IStockRepository, StubStockRepository>();
                builder.Services.AddSingleton<IInvestmentTrustHistoryRepository, StubInvestmentTrustHistoryRepository>();
            }

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}