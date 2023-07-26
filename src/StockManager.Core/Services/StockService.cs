using AutoMapper;
using StockManager.Core.Entities;
using StockManager.Core.OutputModels;
using StockManager.Core.Repositories;
using StockManager.Core.Utils;

namespace StockManager.Core.Services
{
    /// <summary>
    ///     株式に関する操作を提供します。
    /// </summary>
    public class StockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IStockHistoryRepository _stockHistoryRepository;
        private readonly IMapper _mapper;

        /// <summary>
        ///     新しいインスタンスを作成します。
        /// </summary>
        /// <param name="stockRepository">株式リポジトリ。</param>
        /// <param name="stockHistoryRepository">株式取引履歴リポジトリ。</param>
        /// <param name="mapper"><see cref="IMapper"/> 。</param>
        public StockService(IStockRepository stockRepository, IStockHistoryRepository stockHistoryRepository, IMapper mapper)
        {
            this._stockRepository = stockRepository;
            this._stockHistoryRepository = stockHistoryRepository;
            this._mapper = mapper;
        }

        /// <summary>
        ///     現在の保有株式の一覧を取得します。
        /// </summary>
        /// <returns>非同期処理の状態。値は保有株式の一覧です。</returns>
        public async ValueTask<IList<StockInfo>> GetStocksAsync()
        {
            var stockCodes = await this._stockRepository.GetStockCodesAsync();
            var stockCodeDictionary = stockCodes.ToDictionary(x => x.Code, x => x.Name);
            var transactionHistory = await this._stockHistoryRepository.FetchHistoryAsync(period: null);
            var dividendHistory = await this._stockHistoryRepository.FetchDividendAsync();
            var holdingStock = await this._stockRepository.GetHoldingStocksAsync();
            var holdingStockCodes = holdingStock.Select(x => x.Code).ToList();

            var stockDictionary = new Dictionary<int, StockInfo>();
            foreach (var transaction in transactionHistory)
            {
                if (!stockDictionary.TryGetValue(transaction.Code, out var stock))
                {
                    stock = new StockInfo
                    {
                        Code = transaction.Code,
                        Name = stockCodeDictionary[transaction.Code]
                    };
                    stockDictionary[transaction.Code] = stock;
                }

                stock.Histories.Add(this._mapper.Map<StockTransactionHistoryEntity, StockInfo.History>(transaction));
            }

            foreach (var dividend in dividendHistory)
            {
                if (stockDictionary.TryGetValue(dividend.Code, out var stock))
                {
                    stock.Histories.Add(
                        new StockInfo.History
                        {
                            Date = dividend.Date,
                            Amount = dividend.Amount,
                            Price = dividend.Profit,
                            Memo = "-",
                            Commission = 0,
                            Type = StockActionType.Dividend
                        }
                    );
                }
            }

            var stocks = stockDictionary.Values.OrderBy(x => !holdingStockCodes.Contains(x.Code)).ThenByDescending(x => x.Histories.Max(x => x.Date)).ToList();
            foreach (var stock in stocks)
            {
                var stockProfit = 0d;
                var dividendProfit = 0d;
                var restStocks = new List<BuyStockInfo>();
                foreach (var history in stock.Histories.OrderBy(x => x.Date))
                {
                    if (history.Type == StockActionType.Buy)
                    {
                        restStocks.Add(
                            new BuyStockInfo
                            {
                                Amount = history.Amount,
                                Price = history.Price
                            }
                       );
                    }
                    else if (history.Type == StockActionType.Sell)
                    {
                        for (var i = 0; i < restStocks.Count; i++)
                        {
                            if (restStocks[i].Amount == 0)
                            {
                                continue;
                            }

                            if (restStocks[i].Amount >= history.Amount)
                            {
                                restStocks[i].Amount -= history.Amount;
                                stockProfit += (history.Price - restStocks[i].Price) * history.Amount;
                                break;
                            }
                            else
                            {
                                stockProfit += (history.Price - restStocks[i].Price) * restStocks[i].Amount;
                                history.Amount -= restStocks[i].Amount;
                            }
                        }
                    }
                    else
                    {
                        dividendProfit += history.Price;
                    }
                }
                stock.StockProfit = (int)stockProfit;
                stock.DividendProfit = (int)dividendProfit;
            }
            return stocks;
        }

        private class BuyStockInfo
        {
            public int Amount { get; set; }

            public double Price { get; set; }
        }
    }
}
