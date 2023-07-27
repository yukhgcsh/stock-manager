using StockManager.Core.Entities;
using StockManager.Core.Utils;

namespace StockManager.Core.Repositories
{
    /// <summary>
    ///     <see cref="IStockHistoryRepository"/> のスタブ実装です。
    /// </summary>
    public class StubStockHistoryRepository : IStockHistoryRepository
    {
        private IList<StockTransactionHistoryEntity> _transactions = new List<StockTransactionHistoryEntity>
        {
            new StockTransactionHistoryEntity{
                Index = 0,
                Code = 1234,
                Quantity = 200,
                Date = new DateTime(2022, 1, 2),
                Memo = "購入",
                Amount = 1000,
                Type = TransactionType.Buy
            },
            new StockTransactionHistoryEntity
            {
                Index = 1,
                Code = 1234,
                Quantity = 100,
                Date = new DateTime(2022, 1, 4),
                Amount = 1100,
                Type = TransactionType.Sell,
                Memo = "利確のため半分売却"
            },
            new StockTransactionHistoryEntity
            {
                Index = 2,
                Code = 5678,
                Quantity = 300,
                Date = new DateTime(2022, 10 ,1),
                Memo = "購入",
                Type = TransactionType.Buy,
                Amount = 600
            },
            new StockTransactionHistoryEntity
            {
                Index = 3,
                Code = 9876,
                Quantity = 400,
                Date = new DateTime(2022, 10, 1),
                Memo = "購入",
                Type = TransactionType.Buy,
                Amount = 400
            },
            new StockTransactionHistoryEntity
            {
                Index = 4,
                Code = 5432,
                Quantity = 200,
                Date = new DateTime(2022, 10, 1),
                Memo = "購入",
                Amount= 1200,
                Type = TransactionType.Buy,
            },
            new StockTransactionHistoryEntity
            {
                Index = 5,
                Code = 5678,
                Quantity = 300,
                Date = new DateTime(2022, 12, 3),
                Memo = "利確のため全部売却",
                Amount = 630,
                Type = TransactionType.Sell,
            },
            new StockTransactionHistoryEntity
            {
                Index = 6,
                Code = 9876,
                Quantity = 400,
                Date = new DateTime(2023, 3, 3),
                Memo = "損切のため売却",
                Amount = 395,
                Type = TransactionType.Sell
            },
            new StockTransactionHistoryEntity
            {
                Index = 7,
                Code = 5432,
                Date = new DateTime(2023, 3, 28),
                Amount = 3000,
                Memo = "-",
                Quantity = 200,
                Type = TransactionType.Dividend
            },
            new StockTransactionHistoryEntity
            {
                Index = 8,
                Code = 5432,
                Quantity = 200,
                Date = new DateTime(2023, 6, 27),
                Memo = "利確のため売却",
                Type= TransactionType.Sell,
                Amount = 1260
            }
        };

        /// <inheritdoc />
        public ValueTask<IEnumerable<StockTransactionHistoryEntity>> FetchHistoryAsync(TimeSpan? fetchPeriod)
        {
            var now = DateTime.Now;
            if (fetchPeriod == null)
            {
                return new ValueTask<IEnumerable<StockTransactionHistoryEntity>>(this._transactions);
            }
            else
            {
                return new ValueTask<IEnumerable<StockTransactionHistoryEntity>>(this._transactions.Where(x => now - x.Date <= fetchPeriod));
            }
        }

        /// <inheritdoc />
        public ValueTask RegisterTransactionAsync(StockTransactionHistoryEntity transaction)
        {
            this._transactions.Add(transaction);
            return new ValueTask(Task.CompletedTask);
        }
    }
}
