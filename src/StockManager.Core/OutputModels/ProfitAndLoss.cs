namespace StockManager.Core.OutputModels
{
    /// <summary>
    ///     損益の情報を定義します。
    /// </summary>
    public class ProfitAndLoss
    {
        /// <summary>
        ///     トータルでの損益を取得または設定します。
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        ///     1カ月(30日)での損益を取得または設定します。
        /// </summary>
        public int OneMonth { get; set; }

        /// <summary>
        ///     半年(180日)での損益を取得または設定します。
        /// </summary>
        public int HalfYear { get; set; }

        /// <summary>
        ///     1年での損益を取得または設定します。
        /// </summary>
        public int OneYear { get; set; }
    }
}
