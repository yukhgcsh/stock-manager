using StockManager.Core.Entities;

namespace StockManager.Core.Repositories
{
    public interface IStockHistoryRepository
    {
        ValueTask<IEnumerable<TransactionHistoryEntity>> FetchHistoryAsync();

        ValueTask RegisterTransactionAsync(TransactionHistoryEntity transaction);

        ValueTask RegisterDividendAsync(DividendEntity dividend);

        ValueTask<IEnumerable<DividendEntity>> FetchDividendAsync();
    }
}
