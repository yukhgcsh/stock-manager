using StockManager.Core.Entities;
using StockManager.Core.Utils;

namespace StockManager.Core.Repositories
{
    /// <summary>
    ///     <see cref="IFundsRepository"/> のスタブ実装です。
    /// </summary>
    public class StubFundsRepository : IFundsRepository
    {
        private int _capital = 4_300_000;
        private IList<FundsHistoryEntity> _fundsHistories = new List<FundsHistoryEntity>
        {
            new FundsHistoryEntity
            {
                Date = new DateTime(2017, 5, 10),
                Amount = 2_000_000,
                Memo = "初期投資",
                Type = FundsHistoryType.Deposit
            },
            new FundsHistoryEntity
            {
                Date = new DateTime(2018, 10, 29),
                Amount = 1_500_000,
                Memo = "元手追加",
                Type = FundsHistoryType.Deposit
            },
            new FundsHistoryEntity
            {
                Date = new DateTime(2019, 3, 3),
                Amount = 1_000_000,
                Memo = "出金",
                Type = FundsHistoryType.Withdrawal
            },
            new FundsHistoryEntity
            {
                Date = new DateTime(2022, 12, 24),
                Amount = 1_800_000,
                Memo = "資金追加",
                Type = FundsHistoryType.Deposit
            },
        };

        /// <inheritdoc />
        public ValueTask<IList<FundsHistoryEntity>> FetchFundsHistoryAsync()
        {
            return new ValueTask<IList<FundsHistoryEntity>>(this._fundsHistories);
        }

        /// <inheritdoc />
        public ValueTask<int> GetCapitalAsync()
        {
            return new ValueTask<int>(this._capital);
        }

        /// <inheritdoc />
        public ValueTask<int> IncleaseCapitalAsync(FundsHistoryEntity entity)
        {
            this._capital += entity.Amount;
            this._fundsHistories.Add(entity);
            return new ValueTask<int>(this._capital);
        }

        /// <inheritdoc />
        public ValueTask<int> ReduceCapitalAsync(FundsHistoryEntity entity)
        {
            this._capital -= entity.Amount;
            this._fundsHistories.Add(entity);
            return new ValueTask<int>(this._capital);
        }
    }
}
