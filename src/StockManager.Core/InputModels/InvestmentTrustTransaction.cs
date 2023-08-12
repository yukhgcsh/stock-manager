using StockManager.Core.Utils;

namespace StockManager.Core.InputModels
{
    public class InvestmentTrustTransaction
    {
        /// <summary>
        ///     投資信託コードを取得または設定します。
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///     投資信託名を取得または設定します。
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        ///     取引日付を取得または設定します。
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     取引数を取得または設定します。
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        ///     取引単位を取得または設定します。
        /// </summary>
        public int Unit { get; set; }

        /// <summary>
        ///     取引単価を取得または設定します。
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        ///     取引タイプを取得または設定します。
        /// </summary>
        public TransactionType Type { get; set; }

        /// <summary>
        ///     メモを取得または設定します。
        /// </summary>
        public string? Memo { get; set; }

        /// <summary>
        ///     NISAかどうかを取得または設定します。
        /// </summary>
        public bool IsNisa { get; set; }
    }
}
