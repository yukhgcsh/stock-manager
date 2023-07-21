using StockManager.Core.Utils;

namespace StockManager.Core.Entities
{
    /// <summary>
    ///     元手の推移履歴を定義します。
    /// </summary>
    public class FundsHistoryEntity
    {
        /// <summary>
        ///     主キー用のインデックスを取得または設定します。
        ///     自動採番のため設定不要。
        /// </summary>
        public int? Index { get; set; }

        /// <summary>
        ///     変更を行った日付を取得または設定します。
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     変更額を取得または設定します。
        ///     値がマイナスの場合元手を引き出したことを意味します。
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        ///     メモを取得または設定します。
        /// </summary>
        public string Memo { get; set; } = "";

        /// <summary>
        ///     元手の変更タイプを取得または設定します。
        /// </summary>
        public FundsHistoryType Type { get; set; }
    }
}
