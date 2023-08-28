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
                Date = DateTime.Now - TimeSpan.FromDays(100),
                Memo = "購入",
                Amount = 1000,
                Type = TransactionType.Buy,
                IsNisa = true
            },
            new StockTransactionHistoryEntity
            {
                Index = 1,
                Code = 1234,
                Quantity = 100,
                Date = DateTime.Now - TimeSpan.FromDays(60),
                Amount = 1100,
                Type = TransactionType.Sell,
                Memo = "利確のため半分売却",
                IsNisa = true
            },
            new StockTransactionHistoryEntity
            {
                Index = 2,
                Code = 5678,
                Quantity = 300,
                Date = DateTime.Now - TimeSpan.FromDays(30),
                Memo = "購入",
                Type = TransactionType.Buy,
                Amount = 600,
                IsNisa = true
            },
            new StockTransactionHistoryEntity
            {
                Index = 3,
                Code = 9876,
                Quantity = 400,
                Date = DateTime.Now - TimeSpan.FromDays(30),
                Memo = "購入",
                Type = TransactionType.Buy,
                Amount = 400,
                IsNisa = false
            },
            new StockTransactionHistoryEntity
            {
                Index = 4,
                Code = 5432,
                Quantity = 200,
                Date = DateTime.Now - TimeSpan.FromDays(30),
                Memo = "購入",
                Amount= 1200,
                Type = TransactionType.Buy,
                IsNisa = false
            },
            new StockTransactionHistoryEntity
            {
                Index = 5,
                Code = 5678,
                Quantity = 300,
                Date = DateTime.Now - TimeSpan.FromDays(3),
                Memo = "利確のため全部売却",
                Amount = 630,
                Type = TransactionType.Sell,
                IsNisa = true
            },
            new StockTransactionHistoryEntity
            {
                Index = 6,
                Code = 9876,
                Quantity = 400,
                Date = DateTime.Now - TimeSpan.FromDays(3),
                Memo = "損切のため売却",
                Amount = 395,
                Type = TransactionType.Sell,
                IsNisa = false
            },
            new StockTransactionHistoryEntity
            {
                Index = 7,
                Code = 5432,
                Date = DateTime.Now - TimeSpan.FromDays(12),
                Amount = 15,
                Memo = "-",
                Quantity = 200,
                Type = TransactionType.Dividend,
                IsNisa = false
            },
            new StockTransactionHistoryEntity
            {
                Index = 8,
                Code = 5432,
                Quantity = 200,
                Date = DateTime.Now - TimeSpan.FromDays(1),
                Memo = "利確のため売却",
                Type= TransactionType.Sell,
                Amount = 1260,
                IsNisa = false
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
                return new ValueTask<IEnumerable<StockTransactionHistoryEntity>>(this._transactions.Where(x => (now - x.Date).Days <= fetchPeriod.Value.Days));

            }
        }

        /// <inheritdoc />
        public ValueTask RegisterTransactionAsync(StockTransactionHistoryEntity transaction)
        {
            this._transactions.Add(transaction);
            return ValueTask.CompletedTask;
        }
    }
}
