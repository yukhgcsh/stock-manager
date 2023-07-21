using StockManager.Core.Utils;

namespace StockManager.Core.InputModels
{
    /// <summary>
    ///     株式の取引情報を定義します。
    /// </summary>
    public class StockTransaction
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
        ///     取引株数を取得または設定します。
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        ///     メモを取得または設定します。
        /// </summary>
        public string? Memo { get; set; }

        /// <summary>
        ///     取引日付を取得または設定します。
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     取引タイプを取得または設定します。
        /// </summary>
        public TransactionType Type { get; set; }

        /// <summary>
        ///     取引価格を取得または設定します。
        /// </summary>
        public double Price { get; set; }
    }
}
