using StockManager.Core.Entities;

namespace StockManager.Core.Repositories
{
    /// <summary>
    ///     <see cref="IInvestmentTrustHistoryRepository"/> のデータベース実装です。
    /// </summary>
    public class DatabaseInvestmentTrustHistoryRepository : IInvestmentTrustHistoryRepository
    {
        /// <inheritdoc />
        public ValueTask<IEnumerable<InvestmentTrustHistoryEntity>> FetchAsync(TimeSpan? period = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ValueTask RegisterAsync(InvestmentTrustHistoryEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
