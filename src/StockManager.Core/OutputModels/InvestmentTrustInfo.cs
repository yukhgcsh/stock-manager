using StockManager.Core.Utils;

namespace StockManager.Core.OutputModels
{
    /// <summary>
    ///     投資信託情報を定義します。
    /// </summary>
    public class InvestmentTrustInfo
    {
        /// <summary>
        ///     投資信託境界のコード(8桁)を取得または設定します。
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///     投資信託銘柄名を取得または設定します。
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        ///     投資信託の損益を取得または設定します。
        /// </summary>
        public int InvestmentTrustProfit { get; set; }

        /// <summary>
        ///     分配金での利益を取得または設定します。
        /// </summary>
        public int DividendProfit { get; set; }

        /// <summary>
        ///     取引履歴を取得または設定します。
        /// </summary>
        public IList<History> Histories { get; set; } = new List<History>();

        /// <summary>
        ///     取引履歴を定義します。
        /// </summary>
        public class History
        {
            /// <summary>
            ///     取引日を取得または設定します。
            /// </summary>
            public DateTime Date { get; set; }

            /// <summary>
            ///     取引種別を取得または設定します。
            /// </summary>
            public TransactionType Type { get; set; }

            /// <summary>
            ///     取引口数を取得または設定します。
            /// </summary>
            public int Quantity { get; set; }

            /// <summary>
            ///     取引単位を取得または設定します。
            /// </summary>
            public int Unit { get; set; }

            /// <summary>
            ///     取引単価を取得または設定します。
            /// </summary>
            public int Amount { get; set; }

            /// <summary>
            ///     メモを取得または設定します。
            /// </summary>
            public string? Memo { get; set; }

            /// <summary>
            ///     手数料を取得または設定します。
            /// </summary>
            public int Commission { get; set; }

            /// <summary>
            ///     NISAかどうかを取得または設定します。
            /// </summary>
            public bool IsNisa { get; set; }
        }
    }
}
