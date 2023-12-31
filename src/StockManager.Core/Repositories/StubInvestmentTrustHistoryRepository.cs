﻿using StockManager.Core.Entities;

namespace StockManager.Core.Repositories
{
    public class StubInvestmentTrustHistoryRepository : IInvestmentTrustHistoryRepository
    {
        private readonly IList<InvestmentTrustHistoryEntity> _history = new List<InvestmentTrustHistoryEntity>
        {
            new InvestmentTrustHistoryEntity
            {
                Index = 1,
                Code = "12345678",
                Name = "欲張り投資信託",
                Amount = 9384,
                Date = DateTime.Now - TimeSpan.FromDays(25),
                Quantity = 12,
                Unit = 10_000,
                Type = Utils.TransactionType.Buy,
                IsNisa = true,
                Memo = "欲張ってみた"
            },
            new InvestmentTrustHistoryEntity
            {
                Index = 2,
                Code = "45678900",
                Name = "元本割れしないように頑張る",
                Amount = 10284,
                Date = DateTime.Now - TimeSpan.FromDays(25),
                Quantity = 9,
                Unit = 1,
                Type = Utils.TransactionType.Buy,
                IsNisa = false,
                Memo = "安定志向"
            },
            new InvestmentTrustHistoryEntity
            {
                Index = 3,
                Code = "45678900",
                Name = "元本割れしないように頑張る",
                Amount = 15284,
                Date = DateTime.Now - TimeSpan.FromDays(2),
                Quantity = 9,
                Unit = 1,
                Type = Utils.TransactionType.Sell,
                IsNisa = false,
                Memo = "結構上がったので売却"
            },
            new InvestmentTrustHistoryEntity
            {
                Index = 4,
                Code = "12345678",
                Name = "欲張り投資信託",
                Amount = 300,
                Date = DateTime.Now - TimeSpan.FromDays(1),
                Quantity = 12,
                Unit = 10_000,
                Type = Utils.TransactionType.Dividend,
                IsNisa = true,
                Memo = "-"
            },
        };

        /// <inheritdoc />
        public ValueTask<IEnumerable<InvestmentTrustHistoryEntity>> FetchAsync(TimeSpan? period = null)
        {
            var now = DateTime.Now;
            if (period != null)
            {
                return new ValueTask<IEnumerable<InvestmentTrustHistoryEntity>>(this._history.Where(x => (now - x.Date).Days <= period.Value.Days));
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
