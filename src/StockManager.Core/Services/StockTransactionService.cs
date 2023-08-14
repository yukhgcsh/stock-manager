using AutoMapper;
using StockManager.Core.Entities;
using StockManager.Core.InputModels;
using StockManager.Core.OutputModels;
using StockManager.Core.Repositories;
using StockManager.Core.Transactions;
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
        private readonly ITransactionManager _tradesTransactionManager;
        private readonly IMapper _mapper;

        /// <summary>
        ///     新しいインスタンスを作成します。
        /// </summary>
        /// <param name="stockRepository">株式リポジトリ。</param>
        /// <param name="stockHistoryRepository">株式取引履歴リポジトリ。</param>
        /// <param name="mapper"><see cref="IMapper"/> 。</param>
        public StockTransactionService(IStockRepository stockRepository, IStockHistoryRepository stockHistoryRepository, ITransactionManager transactionManager, IMapper mapper)
        {
            this._stockRepository = stockRepository;
            this._stockHistoryRepository = stockHistoryRepository;
            this._tradesTransactionManager = transactionManager;
            this._mapper = mapper;
        }

        /// <summary>
        ///     株式の取引履歴を取得します。
        /// </summary>
        /// <returns>非同期処理の状態。値は株式の取引履歴の一覧です。</returns>
        public async ValueTask<IList<StockTransactionHistory>> FetchStockTransactionHistoryAsync()
        {
            await this._tradesTransactionManager.OpenAsync();
            var histories = await this._stockHistoryRepository.FetchHistoryAsync();
            var codes = await this._stockRepository.GetStockCodesAsync();
            var codeDictionary = codes.ToDictionary(x => x.Code, x => x.Name);
            return histories.Select(x =>
            {
                var history = this._mapper.Map<StockTransactionHistoryEntity, StockTransactionHistory>(x);
                history.Name = codeDictionary[history.Code];
                return history;
            }).OrderByDescending(x => x.Date).ToList();
        }

        /// <summary>
        ///     株式の取引履歴を登録します。
        /// </summary>
        /// <param name="stockTransaction">取引履歴。</param>
        /// <returns>非同期処理の状態。</returns>
        public async ValueTask RegisterStockTransactionAsync(StockTransaction stockTransaction)
        {
            await using var transaction = await this._tradesTransactionManager.BeginTransactionAsync();
            var historyEntity = this._mapper.Map<StockTransaction, StockTransactionHistoryEntity>(stockTransaction);
            var stockCode = new StockCodeEntity
            {
                Code = historyEntity.Code,
                Name = stockTransaction.Name
            };

            await this._stockRepository.UpsertStockCodeAsync(stockCode);
            await this._stockHistoryRepository.RegisterTransactionAsync(historyEntity);

            if (stockTransaction.Type == TransactionType.Buy)
            {
                var buyEntity = this._mapper.Map<StockTransaction, HoldingStockEntity>(stockTransaction);
                await this._stockRepository.BuyStockAsync(buyEntity);
            }
            else if (stockTransaction.Type == TransactionType.Sell)
            {
                await this._stockRepository.SellStockAsync(stockTransaction.Code, stockTransaction.Date, stockTransaction.Quantity, stockTransaction.Amount, stockTransaction.IsNisa);
            }

            await transaction.CommitAsync();
        }

        public async ValueTask ImportAsync(Stream stream)
        {
            await using var transaction = await this._tradesTransactionManager.BeginTransactionAsync();
            using var reader = new StreamReader(stream);
            var line = await reader.ReadLineAsync();
            while (stream.CanRead)
            {
                line = await reader.ReadLineAsync();
                if (line == null)
                {
                    break;
                }
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                var elements = line.Split(",").Select(x => x.Trim()).ToArray();
                var historyEntity = new StockTransactionHistoryEntity
                {
                    Code = int.Parse(elements[0]),
                    Type = (TransactionType)int.Parse(elements[2]),
                    Date = DateTime.Parse(elements[3]),
                    Quantity = int.Parse(elements[4]),
                    Amount = double.Parse(elements[5]),
                    IsNisa = bool.Parse(elements[6]),
                    Commission = int.Parse(elements[7]),
                    Memo = elements[8]
                };
                var codeEntity = new StockCodeEntity
                {
                    Code = historyEntity.Code,
                    Name = elements[1]
                };
                await this._stockHistoryRepository.RegisterTransactionAsync(historyEntity);
                await this._stockRepository.UpsertStockCodeAsync(codeEntity);

                if (historyEntity.Type == TransactionType.Buy)
                {
                    var buyEntity = this._mapper.Map<StockTransactionHistoryEntity, HoldingStockEntity>(historyEntity);
                    await this._stockRepository.BuyStockAsync(buyEntity);
                }
                else if (historyEntity.Type == TransactionType.Sell)
                {
                    await this._stockRepository.SellStockAsync(historyEntity.Code, historyEntity.Date, historyEntity.Quantity, historyEntity.Amount, historyEntity.IsNisa);
                }
            }

            await transaction.CommitAsync();
        }
    }
}
