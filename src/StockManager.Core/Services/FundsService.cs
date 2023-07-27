using StockManager.Core.OutputModels;
using StockManager.Core.Repositories;

namespace StockManager.Core.Services
{
    /// <summary>
    ///     元手に関する操作を提供します。
    /// </summary>
    public class FundsService
    {
        private readonly IFundsRepository _fundsRepository;

        /// <summary>
        ///     新しいインスタンスを作成します。
        /// </summary>
        /// <param name="fundsRepository">元手に関するリポジトリ。</param>
        public FundsService(IFundsRepository fundsRepository)
        {
            this._fundsRepository = fundsRepository;
        }

        /// <summary>
        ///     現在の元手の額を取得します。
        /// </summary>
        /// <returns>非同期処理の状態。値は現在の元手の額です。</returns>
        public async ValueTask<int> GetCapitalAsync()
        {
            return await this._fundsRepository.GetCapitalAsync();
        }

        /// <summary>
        ///     元手の推移履歴を取得します。
        /// </summary>
        /// <returns>非同期処理の状態。値は元手の推移履歴の一覧です。</returns>
        public async ValueTask<IList<FundsTransition>> FetchFundsTransitionsAsync()
        {
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
    }
}
