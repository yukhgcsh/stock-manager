using StockManager.Core.Entities;

namespace StockManager.Core.Repositories
{
    public class DatabaseStockHistoryRepository : IStockHistoryRepository
    {
        public ValueTask<IEnumerable<DividendEntity>> FetchDividendAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<TransactionHistoryEntity>> FetchHistoryAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask RegisterDividendAsync(DividendEntity dividend)
        {
            throw new NotImplementedException();
        }

        public ValueTask RegisterTransactionAsync(TransactionHistoryEntity transaction)
        {
            throw new NotImplementedException();
        }
    }
}
