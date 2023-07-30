using StockManager.Core.Entities;

namespace StockManager.Core.Repositories
{
    /// <summary>
    ///     投資信託のデータを扱うインターフェースを定義します。
    /// </summary>
    public interface IInvestmentTrustHistoryRepository
    {
        /// <summary>
        ///     投資信託の取引履歴を取得します。
        /// </summary>
        /// <param name="period">取得期間。指定しない場合全履歴を取得します。</param>
        /// <returns>非同期処理の状態。値は取引履歴の一覧です。</returns>
        public ValueTask<IEnumerable<InvestmentTrustHistoryEntity>> FetchAsync(TimeSpan? period = null);

        /// <summary>
        ///     投資信託の取引履歴を登録します。
        /// </summary>
        /// <param name="entity">追加する取引履歴。</param>
        /// <returns>非同期処理の状態。</returns>
        public ValueTask RegisterAsync(InvestmentTrustHistoryEntity entity);
    }
}
