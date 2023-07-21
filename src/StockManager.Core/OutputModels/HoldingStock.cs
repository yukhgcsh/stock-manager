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
        public string Name { get; set; }

        /// <summary>
        ///     購入日付を取得または設定します。
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     所有株数を取得または設定します。
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        ///     取得株の単価を取得または設定します。
        /// </summary>
        public double Price { get; set; }
    }
}
