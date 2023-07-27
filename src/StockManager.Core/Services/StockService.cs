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

            var stocks = stockDictionary.Values.OrderBy(x => !holdingStockCodes.Contains(x.Code)).ThenByDescending(x => x.Histories.Max(x => x.Date)).ToList();
            foreach (var stock in stocks)
            {
                var stockProfit = 0d;
                var dividendProfit = 0d;
                var restStocks = new List<BuyStockInfo>();
                foreach (var history in stock.Histories.OrderBy(x => x.Date))
                {
                    if (history.Type == TransactionType.Buy)
                    {
                        restStocks.Add(
                            new BuyStockInfo
                            {
                                Quantity = history.Quantity,
                                Amount = history.Amount
                            }
                       );
                    }
                    else if (history.Type == TransactionType.Sell)
                    {
                        for (var i = 0; i < restStocks.Count; i++)
                        {
                            if (restStocks[i].Quantity == 0)
                            {
                                continue;
                            }

                            if (restStocks[i].Quantity >= history.Quantity)
                            {
                                restStocks[i].Quantity -= history.Quantity;
                                stockProfit += (history.Amount - restStocks[i].Amount) * history.Quantity;
                                break;
                            }
                            else
                            {
                                stockProfit += (history.Amount - restStocks[i].Amount) * restStocks[i].Quantity;
                                history.Quantity -= restStocks[i].Quantity;
                            }
                        }
                    }
                    else
                    {
                        dividendProfit += history.Amount;
                    }
                }
                stock.StockProfit = (int)stockProfit;
                stock.DividendProfit = (int)dividendProfit;
            }
            return stocks;
        }

        private class BuyStockInfo
        {
            public int Quantity { get; set; }

            public double Amount { get; set; }
        }
    }
}
