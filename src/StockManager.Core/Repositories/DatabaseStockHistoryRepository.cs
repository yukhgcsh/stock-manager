using StockManager.Core.Entities;

namespace StockManager.Core.Repositories
{
    /// <summary>
    ///     <see cref="IStockHistoryRepository"/> のデータベース実装です。
    /// </summary>
    public class DatabaseStockHistoryRepository : IStockHistoryRepository
    {
        /// <inheritdoc />
        public ValueTask<IEnumerable<DividendEntity>> FetchDividendAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ValueTask<IEnumerable<TransactionHistoryEntity>> FetchHistoryAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ValueTask RegisterDividendAsync(DividendEntity dividend)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ValueTask RegisterTransactionAsync(TransactionHistoryEntity transaction)
        {
            throw new NotImplementedException();
        }
    }
}
