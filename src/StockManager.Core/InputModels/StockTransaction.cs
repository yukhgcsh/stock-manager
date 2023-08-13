using StockManager.Core.Utils;

namespace StockManager.Core.InputModels
{
    /// <summary>
    ///     株式の取引情報(購入、売却、配当)を定義します。
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
        public int Quantity { get; set; }

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
        ///     株式の売買の場合は単価、配当の場合は合計額を取得または設定します。
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        ///     NISAかどうかを取得または設定します。
        /// </summary>
        public bool IsNisa { get; set; }

        /// <summary>
        ///     手数料を取得または設定します。
        /// </summary>
        public int Commission { get; set; }
    }
}
