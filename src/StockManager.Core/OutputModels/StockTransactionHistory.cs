using StockManager.Core.Utils;

namespace StockManager.Core.OutputModels
{
    public class StockTransactionHistory
    {
        public int Code { get; set; }

        public string Name { get; set; } = null!;

        public string? Memo { get; set; }

        public TransactionType Type { get; set; }

        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public int Quantity { get; set; }

        public int Commission { get; set; } 
    }
}
