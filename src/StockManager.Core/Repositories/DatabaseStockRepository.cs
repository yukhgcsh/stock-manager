using StockManager.Core.Entities;

namespace StockManager.Core.Repositories
{
    public class DatabaseStockRepository : IStockRepository
    {
        public ValueTask BuyStockAsync(HoldingStockEntity entity)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<HoldingStockEntity>> GetHoldingStocksAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<SoldStockEntity>> GetSoldStocksAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<StockCodeEntity>> GetStockCodesAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask SellStockAsync(int code, DateTime date, int amount, double price)
        {
            throw new NotImplementedException();
        }

        public ValueTask UpsertStockCodeAsync(StockCodeEntity stockCode)
        {
            throw new NotImplementedException();
        }
    }
}
