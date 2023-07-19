using StockManager.Core.Entities;

namespace StockManager.Core.Repositories
{
    public class DatabaseFundsRepository : IFundsRepository
    {
        public ValueTask<IList<FundsHistoryEntity>> FetchFundsHistoryAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<int> GetCapitalAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<int> IncleaseCapitalAsync(FundsHistoryEntity entity)
        {
            throw new NotImplementedException();
        }

        public ValueTask<int> ReduceCapitalAsync(FundsHistoryEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
