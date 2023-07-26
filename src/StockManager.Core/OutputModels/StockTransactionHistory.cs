using StockManager.Core.Utils;

namespace StockManager.Core.OutputModels
{
    public class StockTransactionHistory
    {
        public int Code { get; set; }

        public string Name { get; set; }

        public string Memo { get; set; }

        public TransactionType Type { get; set; }

        public DateTime Date { get; set; }

        public double Price { get; set; }

        public int Amount { get; set; }

        public int Commission { get; set; } 
    }
}
