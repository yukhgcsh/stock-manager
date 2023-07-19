using AutoMapper;
using StockManager.Core.Entities;
using StockManager.Core.InputModels;
using StockManager.Core.Repositories;
using StockManager.Core.Utils;

namespace StockManager.Core.Services
{
    public class StockTransactionService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IStockHistoryRepository _stockHistoryRepository;
        private readonly IMapper _mapper;

        public StockTransactionService(IStockRepository stockRepository, IStockHistoryRepository stockHistoryRepository, IMapper mapper)
        {
            this._stockRepository = stockRepository;
            this._stockHistoryRepository = stockHistoryRepository;
            this._mapper = mapper;
        }

        public async ValueTask RegisterStockTransactionAsync(StockTransaction stockTransaction)
        {
            var historyEntity = this._mapper.Map<StockTransaction, TransactionHistoryEntity>(stockTransaction);
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
                waitTaskList.Add(this._stockRepository.SellStockAsync(stockTransaction.Code, stockTransaction.Date, stockTransaction.Amount, stockTransaction.Price).AsTask());
            }

            await Task.WhenAll(waitTaskList);
        }
    }
}
