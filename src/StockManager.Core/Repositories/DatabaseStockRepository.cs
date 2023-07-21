using StockManager.Core.Entities;

namespace StockManager.Core.Repositories
{
    /// <summary>
    ///     <see cref="IStockRepository"/> のデータベース実装です。
    /// </summary>
    public class DatabaseStockRepository : IStockRepository
    {
        /// <inheritdoc />
        public ValueTask BuyStockAsync(HoldingStockEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ValueTask<IEnumerable<HoldingStockEntity>> GetHoldingStocksAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ValueTask<IEnumerable<SoldStockEntity>> GetSoldStocksAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ValueTask<IEnumerable<StockCodeEntity>> GetStockCodesAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ValueTask SellStockAsync(int code, DateTime date, int amount, double price)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ValueTask UpsertStockCodeAsync(StockCodeEntity stockCode)
        {
            throw new NotImplementedException();
        }
    }
}
