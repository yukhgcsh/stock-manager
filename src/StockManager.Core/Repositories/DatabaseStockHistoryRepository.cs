using StockManager.Core.Entities;

namespace StockManager.Core.Repositories
{
    /// <summary>
    ///     <see cref="IStockHistoryRepository"/> のデータベース実装です。
    /// </summary>
    public class DatabaseStockHistoryRepository : IStockHistoryRepository
    {
        /// <inheritdoc />
        public ValueTask<IEnumerable<StockDividendEntity>> FetchDividendAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<StockTransactionHistoryEntity>> FetchHistoryAsync(TimeSpan? period)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ValueTask RegisterDividendAsync(StockDividendEntity dividend)
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
