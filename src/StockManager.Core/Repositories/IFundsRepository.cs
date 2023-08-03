using StockManager.Core.Entities;

namespace StockManager.Core.Repositories
{
    /// <summary>
    ///     元手のデータソースを操作するリポジトリのインターフェースを定義します。
    /// </summary>
    public interface IFundsRepository
    {
        /// <summary>
        ///     現在の元手の額を取得します。
        /// </summary>
        /// <returns>非同期処理の状態。値は現在の元手の額です。</returns>
        ValueTask<int> GetCapitalAsync();

        /// <summary>
        ///     元手の推移履歴を取得または設定します。
        /// </summary>
        /// <returns>非同期処理の状態、値は元手の推移履歴の一覧。</returns>
        ValueTask<IList<FundsHistoryEntity>> FetchFundsHistoryAsync();

        /// <summary>
        ///     元手を増やします。
        /// </summary>
        /// <param name="amount">元手の増加量。</param>
        /// <returns>非同期処理の状態。</returns>
        ValueTask IncreaseCapitalAsync(int amount);

        /// <summary>
        ///     元手を減らします。
        /// </summary>
        /// <param name="amount">元手の減少量。</param>
        /// <returns>非同期処理の状態。</returns>
        ValueTask ReduceCapitalAsync(int amount);

        /// <summary>
        ///     元手の取引履歴を登録します。
        /// </summary>
        /// <param name="entity">取引履歴。</param>
        /// <returns>非同期処理の状態。</returns>
        ValueTask RegisterHistoryAsync(FundsHistoryEntity entity);
    }
}
