using StockManager.Core.Utils;

namespace StockManager.Core.Entities
{
    /// <summary>
    ///     取引履歴情報を定義します。
    /// </summary>
    public class TransactionHistoryEntity
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
        ///     取引日付を取得または設定します。
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     取引株数を取得または設定します。
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        ///     取引価格を取得または設定します。
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        ///     取引タイプを取得または設定します。
        /// </summary>
        public TransactionType Type { get; set; }

        /// <summary>
        ///     メモを取得または設定します。
        /// </summary>
        public string? Memo { get; set; }
    }
}
