using StockManager.Core.Entities;

namespace StockManager.Core.Repositories
{
    public interface IFundsRepository
    {
        ValueTask<int> GetCapitalAsync();

        ValueTask<IList<FundsHistoryEntity>> FetchFundsHistoryAsync();

        ValueTask<int> IncleaseCapitalAsync(FundsHistoryEntity entity);

        ValueTask<int> ReduceCapitalAsync(FundsHistoryEntity entity);
    }
}
