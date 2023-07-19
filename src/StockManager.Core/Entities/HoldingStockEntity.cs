
namespace StockManager.Core.Entities
{
    public class HoldingStockEntity
    {
        public int? Index { get; set; }

        public int Code { get; set; }

        public DateTime Date { get; set; }

        public int Amount { get; set; }

        public double Price { get; set; }
    }
}
