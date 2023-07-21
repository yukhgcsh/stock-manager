using StockManager.Core.Entities;

namespace StockManager.Core.Repositories
{
    /// <summary>
    ///     <see cref="IFundsRepository"/> のデータベース実装です。
    /// </summary>
    public class DatabaseFundsRepository : IFundsRepository
    {
        /// <inheritdoc />
        public ValueTask<IList<FundsHistoryEntity>> FetchFundsHistoryAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ValueTask<int> GetCapitalAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ValueTask<int> IncleaseCapitalAsync(FundsHistoryEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ValueTask<int> ReduceCapitalAsync(FundsHistoryEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
