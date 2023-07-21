using StockManager.Core.Utils;

namespace StockManager.Core.InputModels
{
    /// <summary>
    ///     配当取得履歴に関する情報を定義します。
    /// </summary>
    public class DividendHistory
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
        ///     支給日付を取得または設定します。
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     利益を取得または設定します。
        /// </summary>
        public double Profit { get; set; }
    }
}
