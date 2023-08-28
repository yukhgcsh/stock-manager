using MudBlazor.Services;
using MySqlConnector;
using StockManager.Core.Options;
using StockManager.Core.Repositories;
using StockManager.Core.Services;
using StockManager.Core.Transactions;
using StockManager.Core.Utils;

namespace StockManager.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddMudServices();
            builder.Services.AddScoped<DashboardService>();
            builder.Services.AddScoped<FundsService>();
            builder.Services.AddScoped<StockService>();
            builder.Services.AddScoped<StockTransactionService>();
            builder.Services.AddScoped<InvestmentTrustService>();
            builder.Services.AddScoped(_ => new MySqlConnection(builder.Configuration.GetConnectionString("Database")));
            builder.Services.AddScoped<ITransactionManager, DatabaseTransactionManager>();
            builder.Services.AddScoped<IFundsRepository, DatabaseFundsRepository>();
            builder.Services.AddScoped<IStockHistoryRepository, DatabaseStockHistoryRepository>();
            builder.Services.AddScoped<IStockRepository, DatabaseStockRepository>();
            builder.Services.AddScoped<IInvestmentTrustHistoryRepository, DatabaseInvestmentTrustHistoryRepository>();
            builder.Services.AddAutoMapper(typeof(MapperProfile));

            if (!builder.Environment.IsProduction())
            {
                builder.Services.AddScoped<ITransactionManager, StubTransactionManager>();
                builder.Services.AddSingleton<IFundsRepository, StubFundsRepository>();
                builder.Services.AddSingleton<IStockHistoryRepository, StubStockHistoryRepository>();
                builder.Services.AddSingleton<IStockRepository, StubStockRepository>();
                builder.Services.AddSingleton<IInvestmentTrustHistoryRepository, StubInvestmentTrustHistoryRepository>();
            }

            builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(DatabaseOptions.OptionName));
            builder.Services.Configure<VersionOptions>(builder.Configuration.GetSection(VersionOptions.OptionName));

            WebApplication app = builder.Build();

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