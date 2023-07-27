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
            public string? Memo { get; set; }

            public TransactionType Type { get; set; }

            public DateTime Date { get; set; }

            public double Amount { get; set; }

            public int Quantity { get; set; }

            public int Commission { get; set; }
        }
    }
}
