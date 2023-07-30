using StockManager.Core.Entities;

namespace StockManager.Core.Repositories
{
    public class DatabaseInvestmentTrustHistoryRepository : IInvestmentTrustHistoryRepository
    {
        public ValueTask<IEnumerable<InvestmentTrustHistoryEntity>> FetchAsync(TimeSpan? period = null)
        {
            throw new NotImplementedException();
        }

        public ValueTask RegisterAsync(InvestmentTrustHistoryEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
