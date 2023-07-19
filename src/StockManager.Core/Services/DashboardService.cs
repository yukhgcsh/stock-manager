using StockManager.Core.Repositories;
using StockManager.Core.OutputModels;
using AutoMapper;
using StockManager.Core.Entities;

namespace StockManager.Core.Services
{
    public class DashboardService
    {
        private readonly IFundsRepository _fundsRepository;
        private readonly IStockHistoryRepository _stockHistoryRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IMapper _mapper;

        public DashboardService(IFundsRepository fundsRepository, IStockHistoryRepository stockHistoryRepository, IStockRepository stockRepository, IMapper mapper)
        {
            this._fundsRepository = fundsRepository;
            this._stockHistoryRepository = stockHistoryRepository;
            this._stockRepository = stockRepository;
            this._mapper = mapper;
        }

        public async ValueTask<int> GetCapitalAsync()
        {
            return await this._fundsRepository.GetCapitalAsync();
        }

        public async ValueTask<ProfitAndLoss> GetProfitAndLossAsync()
        {
            var fixledProfit = await this.GetFixedProfitAsync();
            var unrealizedProfit = await this.GetUnrealizedProfitAsync();

            return new ProfitAndLoss
            {
                HalfYear = fixledProfit.HalfYear + unrealizedProfit.HalfYear,
                OneMonth = fixledProfit.OneMonth + unrealizedProfit.OneMonth,
                OneYear = fixledProfit.OneYear + unrealizedProfit.OneYear,
                Total = fixledProfit.Total + unrealizedProfit.Total,
            };
        }

        private async ValueTask<ProfitAndLoss> GetFixedProfitAsync()
        {
            var result = new ProfitAndLoss();
            var soldStocks = await this._stockRepository.GetSoldStocksAsync();
            var today = DateTimeOffset.Now;
            foreach (var stock in soldStocks)
            {
                var diff = today - stock.SoldDate;
                result.Total += stock.Profit;
                if (diff <= TimeSpan.FromDays(365))
                {
                    result.OneYear += stock.Profit;
                }

                if (diff <= TimeSpan.FromDays(180))
                {
                    result.HalfYear += stock.Profit;
                }

                if (diff <= TimeSpan.FromDays(30))
                {
                    result.OneMonth += stock.Profit;
                }
            }

            return result;
        }

        private ValueTask<ProfitAndLoss> GetUnrealizedProfitAsync()
        {
#warning 実装
            var value = new ProfitAndLoss
            {
                HalfYear = 0,
                OneYear = 0,
                OneMonth = 0,
                Total = 0,
            };
            return new ValueTask<ProfitAndLoss>(value);
        }

        public async ValueTask<IEnumerable<HoldingStock>> GetHoldingStockAsync()
        {
            var data = await this._stockRepository.GetHoldingStocksAsync();
            var stockCodes = await this._stockRepository.GetStockCodesAsync();
            var stockCodeDictionary = stockCodes.ToDictionary(x => x.Code, x => x.Name);
            return data.Select(x =>
                {
                    var stock = this._mapper.Map<HoldingStockEntity, HoldingStock>(x);
                    if (stockCodeDictionary.TryGetValue(stock.Code, out var name))
                    {
                        stock.Name = name;
                    }
                    else
                    {
                        stock.Name = "Unknown";
                    }
                    return stock;
                });
        }

        public async ValueTask<IEnumerable<SoldStock>> FetchSoldStockAsync(TimeSpan? fetchPeriod)
        {
            var stocks = await this._stockRepository.GetSoldStocksAsync();
            var stockCodes = await this._stockRepository.GetStockCodesAsync();
            var stockCodeDictionary = stockCodes.ToDictionary(x => x.Code, x => x.Name);
            if (fetchPeriod == null)
            {
                return stocks.Select(x =>
                    {
                        var stock = this._mapper.Map<SoldStockEntity, SoldStock>(x);
                        if (stockCodeDictionary.TryGetValue(stock.Code, out var name))
                        {
                            stock.Name = name;
                        }
                        else
                        {
                            stock.Name = "Unknown";
                        }
                        return stock;
                    }
                );
            }
            else
            {
                var now = DateTime.Now;
                return stocks
                    .Where(x => now - x.SoldDate < fetchPeriod)
                    .Select(x =>
                    {
                        var stock = this._mapper.Map<SoldStockEntity, SoldStock>(x);
                        if (stockCodeDictionary.TryGetValue(stock.Code, out var name))
                        {
                            stock.Name = name;
                        }
                        else
                        {
                            stock.Name = "Unknown";
                        }
                        return stock;
                    }
                );
            }
        }
    }
}
