using StockManager.Core.Entities;

namespace StockManager.Core.Repositories
{
    /// <summary>
    ///     <see cref="IStockHistoryRepository"/> のデータベース実装です。
    /// </summary>
    public class DatabaseStockHistoryRepository : IStockHistoryRepository
    {
        public ValueTask<IEnumerable<StockTransactionHistoryEntity>> FetchHistoryAsync(TimeSpan? period)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ValueTask RegisterTransactionAsync(StockTransactionHistoryEntity transaction)
        {
            throw new NotImplementedException();
        }
    }
}
