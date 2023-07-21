using StockManager.Core.Entities;

namespace StockManager.Core.Repositories
{
    /// <summary>
    ///     株式に関するデータソースを操作するリポジトリのインターフェースを定義します。
    /// </summary>
    public interface IStockRepository
    {
        /// <summary>
        ///     所有株式の一覧を取得します。
        /// </summary>
        /// <returns>非同期処理の状態。値は所有株式の一覧です。</returns>
        public ValueTask<IEnumerable<HoldingStockEntity>> GetHoldingStocksAsync();

        /// <summary>
        ///     売却済み株式の一覧を取得します。
        /// </summary>
        /// <returns>非同期処理の状態。値は売却済み株式の一覧です。</returns>
        public ValueTask<IEnumerable<SoldStockEntity>> GetSoldStocksAsync();

        /// <summary>
        ///     銘柄コードと会社名の一覧を取得します。
        /// </summary>
        /// <returns>非同期処理の状態。銘柄コードと会社名の一覧。</returns>
        public ValueTask<IEnumerable<StockCodeEntity>> GetStockCodesAsync();

        /// <summary>
        ///     銘柄コードを更新します。
        /// </summary>
        /// <param name="stockCode">更新する銘柄コード。</param>
        /// <returns>非同期処理の状態。</returns>
        public ValueTask UpsertStockCodeAsync(StockCodeEntity stockCode);

        /// <summary>
        ///     株式の購入処理を行います。
        /// </summary>
        /// <param name="entity">購入株式の情報。</param>
        /// <returns>非同期処理の状態。</returns>
        public ValueTask BuyStockAsync(HoldingStockEntity entity);

        /// <summary>
        ///     株式の売却処理を行います。
        /// </summary>
        /// <param name="code">銘柄コード。</param>
        /// <param name="date">売却日付。</param>
        /// <param name="amount">売却株数。</param>
        /// <param name="price">売却金額。</param>
        /// <returns>非同期処理の状態。</returns>
        public ValueTask SellStockAsync(int code, DateTime date, int amount, double price);
    }
}
