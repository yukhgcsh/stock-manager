namespace StockManager.Core.Entities
{
    public class DividendEntity
    {
        public int? Index { get; set; }

        public int Code { get; set; }

        public DateTime Date { get; set; }

        public int Profit { get; set; }
    }
}
