using StockManager.Core.Utils;
namespace StockManager.Core.InputModels
{
    public class DividendHistory
    {
        public int Code { get; set; }

        public string Name { get; set; } = null!;

        public DateTime Date { get; set; }

        public double Profit { get; set; }
    }
}
