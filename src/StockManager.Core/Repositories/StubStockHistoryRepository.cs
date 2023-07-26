using StockManager.Core.Entities;
using StockManager.Core.Utils;

namespace StockManager.Core.Repositories
{
    /// <summary>
    ///     <see cref="IStockHistoryRepository"/> のスタブ実装です。
    /// </summary>
    public class StubStockHistoryRepository : IStockHistoryRepository
    {
        private IList<TransactionHistoryEntity> _transactions = new List<TransactionHistoryEntity>
        {
            new TransactionHistoryEntity{
                Index = 0,
                Code = 1234,
                Amount = 200,
                Date = new DateTime(2022, 1, 2),
                Memo = "購入",
                Price = 1000,
                Type = TransactionType.Buy
            },
            new TransactionHistoryEntity
            {
                Index = 1,
                Code = 1234,
                Amount = 100,
                Date = new DateTime(2022, 1, 4),
                Price = 1100,
                Type = TransactionType.Sell,
                Memo = "利確のため半分売却"
            },
            new TransactionHistoryEntity
            {
                Index = 2,
                Code = 5678,
                Amount = 300,
                Date = new DateTime(2022, 10 ,1),
                Memo = "購入",
                Type = TransactionType.Buy,
                Price = 600
            },
            new TransactionHistoryEntity
            {
                Index = 3,
                Code = 9876,
                Amount = 400,
                Date = new DateTime(2022, 10, 1),
                Memo = "購入",
                Type = TransactionType.Buy,
                Price = 400
            },
            new TransactionHistoryEntity
            {
                Index = 4,
                Code = 5432,
                Amount = 200,
                Date = new DateTime(2022, 10, 1),
                Memo = "購入",
                Price= 1200,
                Type = TransactionType.Buy,
            },
            new TransactionHistoryEntity
            {
                Index = 5,
                Code = 5678,
                Amount = 300,
                Date = new DateTime(2022, 12, 3),
                Memo = "利確のため全部売却",
                Price = 630,
                Type = TransactionType.Sell,
            },
            new TransactionHistoryEntity
            {
                Index = 6,
                Code = 9876,
                Amount = 400,
                Date = new DateTime(2023, 3, 3),
                Memo = "損切のため売却",
                Price = 395,
                Type = TransactionType.Sell
            },
            new TransactionHistoryEntity
            {
                Index = 7,
                Code = 5432,
                Amount = 200,
                Date = new DateTime(2023, 6, 27),
                Memo = "利確のため売却",
                Type= TransactionType.Sell,
                Price = 1260
            }
        };

        private IList<StockDividendEntity> _dividends = new List<StockDividendEntity>
        {
            new StockDividendEntity
            {
                Index = 1,
                Code = 5432,
                Date = new DateTime(2023, 3, 28),
                Profit = 3000
            }
        };

        /// <inheritdoc />
        public ValueTask<IEnumerable<StockDividendEntity>> FetchDividendAsync()
        {
            return new ValueTask<IEnumerable<StockDividendEntity>>(this._dividends);
        }

        /// <inheritdoc />
        public ValueTask<IEnumerable<TransactionHistoryEntity>> FetchHistoryAsync(TimeSpan? fetchPeriod)
        {
            var now = DateTime.Now;
            if (fetchPeriod == null)
            {
                return new ValueTask<IEnumerable<TransactionHistoryEntity>>(this._transactions);
            }
            else
            {
                return new ValueTask<IEnumerable<TransactionHistoryEntity>>(this._transactions.Where(x => now - x.Date <= fetchPeriod));
            }
        }

        /// <inheritdoc />
        public ValueTask RegisterDividendAsync(StockDividendEntity dividend)
        {
            this._dividends.Add(dividend);
            return new ValueTask(Task.CompletedTask);
        }

        /// <inheritdoc />
        public ValueTask RegisterTransactionAsync(TransactionHistoryEntity transaction)
        {
            this._transactions.Add(transaction);
            return new ValueTask(Task.CompletedTask);
        }
    }
}
