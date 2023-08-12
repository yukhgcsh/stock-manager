namespace StockManager.Core.Entities
{
    /// <summary>
    ///     売却済み株式の情報を定義します。
    /// </summary>
    public class SoldStockEntity
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
        public DateTime BoughtDate { get; set; }

        /// <summary>
        ///     売却日付を取得または設定します。
        /// </summary>
        public DateTime SoldDate { get; set; }

        /// <summary>
        ///     取引株数を取得または設定します。
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        ///     売却による損益を取得または設定します。
        /// </summary>
        public int Profit { get; set; }

        /// <summary>
        ///     NISAかどうかを取得または設定します。
        /// </summary>
        public bool IsNisa { get; set; }
    }
}
