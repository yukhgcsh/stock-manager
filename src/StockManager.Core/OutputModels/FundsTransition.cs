namespace StockManager.Core.OutputModels
{
    /// <summary>
    ///     元手の情報を定義します。
    /// </summary>
    public class FundsTransition
    {
        /// <summary>
        ///     日付を取得または設定します。
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     元手の額を取得または設定します。
        /// </summary>
        public int Capital { get; set; }
    }
}
