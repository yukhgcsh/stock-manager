using StockManager.Core.Utils;

namespace StockManager.Core.InputModels
{
    /// <summary>
    ///     元手の履歴情報を定義します。
    /// </summary>
    public class FundsHistory
    {
        /// <summary>
        ///     取引額を取得または設定します。
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        ///     取引日を取得または設定します。
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     メモを取得または設定します。
        /// </summary>
        public string? Memo { get; set; }

        /// <summary>
        ///     取引種別を取得または設定します。
        /// </summary>
        public FundsHistoryType Type { get; set; }
    }
}
