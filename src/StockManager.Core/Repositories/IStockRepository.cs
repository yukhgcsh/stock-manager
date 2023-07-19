using StockManager.Core.Entities;

namespace StockManager.Core.Repositories
{
    public interface IStockRepository
    {
        public ValueTask<IEnumerable<HoldingStockEntity>> GetHoldingStocksAsync();

        public ValueTask<IEnumerable<SoldStockEntity>> GetSoldStocksAsync();

        public ValueTask<IEnumerable<StockCodeEntity>> GetStockCodesAsync();

        public ValueTask UpsertStockCodeAsync(StockCodeEntity stockCode);

        public ValueTask BuyStockAsync(HoldingStockEntity entity);

        public ValueTask SellStockAsync(int code, DateTime date, int amount, double price);
    }
}
