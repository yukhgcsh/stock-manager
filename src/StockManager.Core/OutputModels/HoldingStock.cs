namespace StockManager.Core.OutputModels
{
    public class HoldingStock
    {
        /// <summary>
        ///     銘柄コードを取得または設定します。
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///     会社名を取得または設定します。
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        ///     購入日付を取得または設定します。
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     所有株数を取得または設定します。
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        ///     取得株の単価を取得または設定します。
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        ///     NISAかどうかを取得または設定します。
        /// </summary>
        public bool IsNisa { get; set; }
    }
}
