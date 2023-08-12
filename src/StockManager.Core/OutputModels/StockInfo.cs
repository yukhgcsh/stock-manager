using StockManager.Core.Utils;

namespace StockManager.Core.OutputModels
{
    /// <summary>
    ///     株式の情報を定義します。
    /// </summary>
    public class StockInfo
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
        ///     この銘柄での確定した利益を取得または設定します。
        ///     購入後売却していない場合は含み益・含み損にかかわらず0となります。
        /// </summary>
        public int StockProfit { get; set; }

        /// <summary>
        ///     配当で得た利益を取得または設定します。
        /// </summary>
        public int DividendProfit { get; set; }


        /// <summary>
        ///     この銘柄の取引履歴を取得または設定します。
        /// </summary>
        public IList<History> Histories { get; set; } = new List<History>();

        public class History
        {
            /// <summary>
            ///     メモを取得または設定します。
            /// </summary>
            public string? Memo { get; set; }

            /// <summary>
            ///     取引種別を取得または設定します。
            /// </summary>
            public TransactionType Type { get; set; }

            /// <summary>
            ///     取引日付を取得または設定します。
            /// </summary>
            public DateTime Date { get; set; }

            /// <summary>
            ///     取引額を取得または設定します。
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

            /// <summary>
            ///     NISAかどうかを取得または設定します。
            /// </summary>
            public bool IsNisa { get; set; }
        }
    }
}
