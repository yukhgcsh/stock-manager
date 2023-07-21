
namespace StockManager.Core.Entities
{
    /// <summary>
    ///     保有株式の情報を定義します。
    /// </summary>
    public class HoldingStockEntity
    {
        /// <summary>
        ///     主キー用のインデックスを取得または設定します。
        ///     自動採番のため設定不要。
        /// </summary>
        public int? Index { get; set; }

        /// <summary>
        ///     銘柄コードを取得または設定します。
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///     購入日付を取得または設定します。
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     購入株数を取得または設定します。
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        ///     購入額を取得または設定します。
        /// </summary>
        public double Price { get; set; }
    }
}
