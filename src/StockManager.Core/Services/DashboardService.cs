﻿using AutoMapper;
using StockManager.Core.Entities;
using StockManager.Core.OutputModels;
using StockManager.Core.Repositories;

namespace StockManager.Core.Services
{
    /// <summary>
    ///     ダッシュボード表示に関するロジックを提供します。
    /// </summary>
    public class DashboardService
    {
        private readonly IFundsRepository _fundsRepository;
        private readonly IStockHistoryRepository _stockHistoryRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IMapper _mapper;

        /// <summary>
        ///     新しいインスタンスを作成します。
        /// </summary>
        /// <param name="fundsRepository">元手に関するリポジトリ。</param>
        /// <param name="stockHistoryRepository">株式取引履歴に関するリポジトリ。</param>
        /// <param name="stockRepository">株式に関するリポジトリ。</param>
        /// <param name="mapper"></param>
        public DashboardService(IFundsRepository fundsRepository, IStockHistoryRepository stockHistoryRepository, IStockRepository stockRepository, IMapper mapper)
        {
            this._fundsRepository = fundsRepository;
            this._stockHistoryRepository = stockHistoryRepository;
            this._stockRepository = stockRepository;
            this._mapper = mapper;
        }

        /// <summary>
        ///     現在の元手の額を取得します。
        /// </summary>
        /// <returns>非同期処理の状態。値は現在の元手額です。</returns>
        public async ValueTask<int> GetCapitalAsync()
        {
            return await this._fundsRepository.GetCapitalAsync();
        }

        /// <summary>
        ///     現在の損益を取得します。
        /// </summary>
        /// <returns>非同期処理の状態。値あh現在の損益です。</returns>
        public async ValueTask<ProfitAndLoss> GetProfitAndLossAsync()
        {
            ProfitAndLoss fixledProfit = await this.GetFixedProfitAsync();
            ProfitAndLoss unrealizedProfit = await this.GetUnrealizedProfitAsync();

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
            ProfitAndLoss result = new ProfitAndLoss();
            IEnumerable<SoldStockEntity> soldStocks = await this._stockRepository.GetSoldStocksAsync();
            DateTimeOffset today = DateTimeOffset.Now;
            foreach (SoldStockEntity stock in soldStocks)
            {
                TimeSpan diff = today - stock.SoldDate;
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
            ProfitAndLoss value = new ProfitAndLoss
            {
                HalfYear = 0,
                OneYear = 0,
                OneMonth = 0,
                Total = 0,
            };
            return new ValueTask<ProfitAndLoss>(value);
        }

        /// <summary>
        ///     現在の保有株式の一覧を取得します。
        /// </summary>
        /// <returns>非同期処理の状態。値は保有株式の一覧です。</returns>
        public async ValueTask<IEnumerable<HoldingStock>> GetHoldingStockAsync()
        {
            IEnumerable<HoldingStockEntity> data = await this._stockRepository.GetHoldingStocksAsync();
            IEnumerable<StockCodeEntity> stockCodes = await this._stockRepository.GetStockCodesAsync();
            Dictionary<int, string> stockCodeDictionary = stockCodes.ToDictionary(x => x.Code, x => x.Name);
            return data.Select(x =>
                {
                    HoldingStock stock = this._mapper.Map<HoldingStockEntity, HoldingStock>(x);
                    if (stockCodeDictionary.TryGetValue(stock.Code, out string? name))
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

        /// <summary>
        ///     指定した期間の取引履歴の一覧を取得します。
        /// </summary>
        /// <param name="fetchPeriod">取得する取引履歴の期間。現在より指定した日時以内の情報を返します。未指定の場合すべての取引履歴を返します。</param>
        /// <returns>非同期処理の状態。値は期限内の取引の一覧。</returns>
        public async ValueTask<IEnumerable<StockTransactionHistory>> FetchTransactionHistoryAsync(TimeSpan? fetchPeriod)
        {
            IEnumerable<StockTransactionHistoryEntity> transactions = await this._stockHistoryRepository.FetchHistoryAsync(fetchPeriod);
            IEnumerable<StockCodeEntity> stockCodes = await this._stockRepository.GetStockCodesAsync();
            Dictionary<int, string> stockCodeDictionary = stockCodes.ToDictionary(x => x.Code, x => x.Name);
            return transactions.Select(x =>
                {
                    StockTransactionHistory stock = this._mapper.Map<StockTransactionHistoryEntity, StockTransactionHistory>(x);
                    if (stockCodeDictionary.TryGetValue(stock.Code, out string? name))
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
