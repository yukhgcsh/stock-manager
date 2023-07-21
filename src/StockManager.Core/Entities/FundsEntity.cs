namespace StockManager.Core.Entities
{
    /// <summary>
    ///    元手の情報を定義します。
    /// </summary>
    public class FundsEntity
    {
        /// <summary>
        ///     元手を取得または設定します。
        /// </summary>
        public int Capital { get; set; }

        /// <summary>
        ///     元手に対する損益を取得または設定します。
        /// </summary>
        public int Profit { get; set; }
    }
}
