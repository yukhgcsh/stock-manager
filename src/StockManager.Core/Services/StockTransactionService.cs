using AutoMapper;
using StockManager.Core.Entities;
using StockManager.Core.InputModels;
using StockManager.Core.Repositories;
using StockManager.Core.Utils;

namespace StockManager.Core.Services
{
    /// <summary>
    ///     株式の取引に関する操作を提供します。
    /// </summary>
    public class StockTransactionService
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
        public StockTransactionService(IStockRepository stockRepository, IStockHistoryRepository stockHistoryRepository, IMapper mapper)
        {
            this._stockRepository = stockRepository;
            this._stockHistoryRepository = stockHistoryRepository;
            this._mapper = mapper;
        }

        /// <summary>
        ///     株式の取引履歴を登録します。
        /// </summary>
        /// <param name="stockTransaction">取引履歴。</param>
        /// <returns>非同期処理の状態。</returns>
        public async ValueTask RegisterStockTransactionAsync(StockTransaction stockTransaction)
        {
            var historyEntity = this._mapper.Map<StockTransaction, StockTransactionHistoryEntity>(stockTransaction);
            var stockCode = new StockCodeEntity
            {
                Code = historyEntity.Code,
                Name = stockTransaction.Name
            };

            var waitTaskList = new List<Task>()
            {
                this._stockRepository.UpsertStockCodeAsync(stockCode).AsTask(),
                this._stockHistoryRepository.RegisterTransactionAsync(historyEntity).AsTask(),
            };

            if (stockTransaction.Type == TransactionType.Buy)
            {
                var buyEntity = this._mapper.Map<StockTransaction, HoldingStockEntity>(stockTransaction);
                waitTaskList.Add(this._stockRepository.BuyStockAsync(buyEntity).AsTask());
            }
            else if (stockTransaction.Type == TransactionType.Sell)
            {
                waitTaskList.Add(this._stockRepository.SellStockAsync(stockTransaction.Code, stockTransaction.Date, stockTransaction.Quantity, stockTransaction.Amount).AsTask());
            }

            await Task.WhenAll(waitTaskList);
        }
    }
}
