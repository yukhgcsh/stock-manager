using StockManager.Core.Utils;

namespace StockManager.Core.OutputModels
{
    public class InvestmentTrustInfo
    {
        public int Code { get; set; }

        public string Name { get; set; }

        public int InvestmentTrustProfit { get; set; }

        public int DividendProfit { get; set; }

        public IList<History> Histories { get; set; } = new List<History>();

        public class History
        {
            public DateTime Date { get; set; }

            public TransactionType Type { get; set; }

            public int Quantity { get; set; }

            public int Unit { get; set; }

            public int Amount { get; set; }

            public string? Memo { get; set; }

            public int Commission { get; set; }
        }
    }
}
