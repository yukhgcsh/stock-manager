using StockManager.Core.Utils;

namespace StockManager.Core.Entities
{
    public class TransactionHistoryEntity
    {
        public int? Index { get; set; }

        public int Code { get; set; }

        public DateTime Date { get; set; }

        public int Amount { get; set; }

        public double Price { get; set; }

        public TransactionType Type { get; set; }

        public string? Memo { get; set; }
    }
}
