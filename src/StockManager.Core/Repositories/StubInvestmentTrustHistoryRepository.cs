using StockManager.Core.Entities;

namespace StockManager.Core.Repositories
{
    public class StubInvestmentTrustHistoryRepository : IInvestmentTrustHistoryRepository
    {
        private readonly IList<InvestmentTrustHistoryEntity> _history = new List<InvestmentTrustHistoryEntity>
        {
            new InvestmentTrustHistoryEntity
            {
                Index = 1,
                Code = 123456,
                Name = "欲張り投資信託",
                Amount = 9384,
                Date = new DateTime(2022, 1, 12),
                Quantity = 12,
                Unit = 10_000,
                Type = Utils.TransactionType.Buy,
                Memo = "欲張ってみた"
            },
            new InvestmentTrustHistoryEntity
            {
                Index = 2,
                Code = 456789,
                Name = "元本割れしないように頑張る",
                Amount = 10284,
                Date = new DateTime(2022, 3, 20),
                Quantity = 9,
                Unit = 1,
                Type = Utils.TransactionType.Buy,
                Memo = "安定志向"
            },
            new InvestmentTrustHistoryEntity
            {
                Index = 3,
                Code = 456789,
                Name = "元本割れしないように頑張る",
                Amount = 15284,
                Date = new DateTime(2023, 3, 30),
                Quantity = 9,
                Unit = 1,
                Type = Utils.TransactionType.Sell,
                Memo = "結構上がったので売却"
            },
            new InvestmentTrustHistoryEntity
            {
                Index = 4,
                Code = 123456,
                Name = "欲張り投資信託",
                Amount = 3600,
                Date = new DateTime(2022, 12, 25),
                Quantity = 12,
                Unit = 10_000,
                Type = Utils.TransactionType.Dividend,
                Memo = "-"
            },
        };

        /// <inheritdoc />
        public ValueTask<IEnumerable<InvestmentTrustHistoryEntity>> FetchAsync(TimeSpan? period = null)
        {
            var now = DateTime.Now;
            if (period != null)
            {
                return new ValueTask<IEnumerable<InvestmentTrustHistoryEntity>>(this._history.Where(x => now - x.Date < period));
            }
            else
            {
                return new ValueTask<IEnumerable<InvestmentTrustHistoryEntity>>(this._history);
            }
        }

        /// <inheritdoc />
        public ValueTask RegisterAsync(InvestmentTrustHistoryEntity entity)
        {
            this._history.Add(entity);
            return ValueTask.CompletedTask;
        }
    }
}
