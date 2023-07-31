using StockManager.Core.Utils;

namespace StockManager.Core.OutputModels
{
    /// <summary>
    ///     株式取引履歴を定義します。
    /// </summary>
    public class StockTransactionHistory
    {
        /// <summary>
        ///     銘柄コードを取得または設定します。
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///     銘柄名を取得または設定します。
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        ///     メモを取得または設定します。
        /// </summary>
        public string? Memo { get; set; }

        /// <summary>
        ///     取引種別を取得または設定します。
        /// </summary>
        public TransactionType Type { get; set; }

        /// <summary>
        ///     取引日を取得または設定します。
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     株式の売買の場合は単価、配当の場合は合計額を取得または設定します。
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        ///     取引株数を取得または設定します。
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        ///     取引手数料を取得または設定します。
        /// </summary>
        public int Commission { get; set; } 
    }
}
