namespace StockManager.Core.OutputModels
{
    /// <summary>
    ///     売却株式情報を取得または設定します。
    /// </summary>
    public class SoldStock
    {
        /// <summary>
        ///     銘柄コードを取得または設定します。
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///     会社名を取得または設定します。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     購入日付を取得または設定します。
        /// </summary>
        public DateTime BoughtDate { get; set; }

        /// <summary>
        ///     売却日付を取得または設定します。
        /// </summary>
        public DateTime SoldDate { get; set; }

        /// <summary>
        ///     売却株数を取得または設定します。
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        ///     損益を取得または設定します。
        /// </summary>
        public int Profit { get; set; }
    }
}
