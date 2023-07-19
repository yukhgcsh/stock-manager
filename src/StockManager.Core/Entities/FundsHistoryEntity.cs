using StockManager.Core.Utils;

namespace StockManager.Core.Entities
{
    public class FundsHistoryEntity
    {
        public int? Index { get; set; }

        public DateTime Date { get; set; }

        public int Amount { get; set; }

        public string Memo { get; set; } = "";

        public FundsHistoryType Type { get; set; }
    }
}
