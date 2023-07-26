using StockManager.Core.Entities;

namespace StockManager.Core.Repositories
{
    /// <summary>
    ///     株式の取引履歴に関するデータソースを操作するリポジトリのインターフェースを定義します。
    /// </summary>
    public interface IStockHistoryRepository
    {
        /// <summary>
        ///     取引履歴を取得します。
        /// </summary>
        /// <returns>非同期処理の状態。値は取引履歴の一覧。</returns>
        ValueTask<IEnumerable<TransactionHistoryEntity>> FetchHistoryAsync();

        /// <summary>
        ///     取引履歴を登録します。
        /// </summary>
        /// <param name="transaction">取引内容。</param>
        /// <returns>非同期処理の状態。</returns>
        ValueTask RegisterTransactionAsync(TransactionHistoryEntity transaction);

        /// <summary>
        ///     配当金の支給内容を登録します。
        /// </summary>
        /// <param name="dividend">配当金情報。</param>
        /// <returns>非同期処理の状態。</returns>
        ValueTask RegisterDividendAsync(StockDividendEntity dividend);

        /// <summary>
        ///     配当の支給履歴を取得します。
        /// </summary>
        /// <returns>非同期処理の状態。</returns>
        ValueTask<IEnumerable<StockDividendEntity>> FetchDividendAsync();
    }
}
