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
    ///     元手に関する操作を提供します。
    /// </summary>
    public class FundsService
    {
        private readonly IFundsRepository _fundsRepository;
        private readonly ITransactionManager _transactionManager;
        private readonly IMapper _mapper;

        /// <summary>
        ///     新しいインスタンスを作成します。
        /// </summary>
        /// <param name="fundsRepository">元手に関するリポジトリ。</param>
        public FundsService(IFundsRepository fundsRepository, ITransactionManager transactionManager, IMapper mapper)
        {
            this._fundsRepository = fundsRepository;
            this._transactionManager = transactionManager;
            this._mapper = mapper;
        }

        /// <summary>
        ///     現在の元手の額を取得します。
        /// </summary>
        /// <returns>非同期処理の状態。値は現在の元手の額です。</returns>
        public async ValueTask<int> GetCapitalAsync()
        {
            await this._transactionManager.OpenAsync();
            return await this._fundsRepository.GetCapitalAsync();
        }

        /// <summary>
        ///     元手の推移履歴を取得します。
        /// </summary>
        /// <returns>非同期処理の状態。値は元手の推移履歴の一覧です。</returns>
        public async ValueTask<IList<FundsTransition>> FetchFundsTransitionsAsync()
        {
            await this._transactionManager.OpenAsync();
            var histories = await this._fundsRepository.FetchFundsHistoryAsync();
            var capital = 0;
            return histories.Select(x =>
            {
                if (x.Type == Utils.FundsHistoryType.Deposit)
                {
                    capital += x.Amount;
                }
                else
                {
                    capital -= x.Amount;
                }
                return new FundsTransition
                {
                    Date = x.Date,
                    Capital = capital,
                };
            }
            ).ToList();
        }

        public async ValueTask RegisterFundsHistoryAsync(FundsHistory history)
        {
            await using var transaction = await this._transactionManager.BeginTransactionAsync();
            var entity = this._mapper.Map<FundsHistory, FundsHistoryEntity>(history);

            await this._fundsRepository.RegisterHistoryAsync(entity);
            if (entity.Type == FundsHistoryType.Deposit)
            {
                await this._fundsRepository.IncreaseCapitalAsync(entity.Amount);
            }
            else
            {
                await this._fundsRepository.ReduceCapitalAsync(entity.Amount);
            }

            await transaction.CommitAsync();
        }
    }
}
