using StockManager.Core.Entities;

namespace StockManager.Core.Repositories
{
    /// <summary>
    ///     <see cref="IStockRepository"/> のスタブ実装です。
    /// </summary>
    public class StubStockRepository : IStockRepository
    {
        private IDictionary<int, IList<HoldingStockEntity>> _holdingStocks = new Dictionary<int, IList<HoldingStockEntity>>
        {
            {
                1234,
                new List<HoldingStockEntity>{
                    new HoldingStockEntity{
                        Amount = 100,
                        Code = 1234,
                        Date = new DateTime(2022, 1, 2),
                        Price = 1000,
                    }
                }
            }
        };

        private IList<SoldStockEntity> _soldStocks = new List<SoldStockEntity> {
            new SoldStockEntity
            {
                Amount = 100,
                Code = 1234,
                BoughtDate = new DateTime(2022, 1, 2),
                SoldDate = new DateTime(2022, 1, 4),
                Profit = 10000
            },
            new SoldStockEntity
            {
                Amount = 300,
                Code = 5678,
                BoughtDate = new DateTime(2022, 10, 1),
                SoldDate = new DateTime(2022, 12, 3),
                Profit = 9000
            },
            new SoldStockEntity
            {
                Amount = 400,
                Code = 9876,
                BoughtDate = new DateTime(2022, 10, 1),
                SoldDate = new DateTime(2023, 3, 3),
                Profit = -2000
            },
            new SoldStockEntity
            {
                Amount = 200,
                Code = 5432,
                BoughtDate = new DateTime(2022, 10, 1),
                SoldDate = new DateTime(2023, 6, 27),
                Profit = 12000
            }
        };

        private IDictionary<int, StockCodeEntity> _stockCodes = new Dictionary<int, StockCodeEntity>
        {
            {
                1234,
                new StockCodeEntity
                {
                    Code = 1234,
                    Name = "テスト株式会社"
                }
            },
            {
                5678,
                new StockCodeEntity
                {
                    Code = 5678,
                    Name = "Hoge株式会社"
                }
            },
            {
                9876,
                new StockCodeEntity
                {
                    Code = 9876,
                    Name = "株式会社Foo"
                }
            },
            {
                5432,
                new StockCodeEntity
                {
                    Code = 5432,
                    Name = "株式会社Bar"
                }
            }
        };

        /// <inheritdoc />
        public ValueTask BuyStockAsync(HoldingStockEntity entity)
        {
            if (this._holdingStocks.ContainsKey(entity.Code))
            {
                this._holdingStocks[entity.Code].Add(entity);
            }
            else
            {
                this._holdingStocks[entity.Code] = new List<HoldingStockEntity>
                {
                    entity
                };
            }
            return new ValueTask(Task.CompletedTask);
        }

        /// <inheritdoc />
        public ValueTask<IEnumerable<HoldingStockEntity>> GetHoldingStocksAsync()
        {
            return new ValueTask<IEnumerable<HoldingStockEntity>>(this._holdingStocks.Values.SelectMany(x => x));
        }

        /// <inheritdoc />
        public ValueTask<IEnumerable<SoldStockEntity>> GetSoldStocksAsync()
        {
            return new ValueTask<IEnumerable<SoldStockEntity>>(this._soldStocks);
        }

        /// <inheritdoc />
        public ValueTask<IEnumerable<StockCodeEntity>> GetStockCodesAsync()
        {
            return new ValueTask<IEnumerable<StockCodeEntity>>(this._stockCodes.Values.ToList());
        }

        /// <inheritdoc />
        public ValueTask SellStockAsync(int code, DateTime date, int amount, double price)
        {
            if (!this._holdingStocks.TryGetValue(code, out var target))
            {
                throw new InvalidOperationException("所有していない株を売却しています。");
            }


            var restAmount = amount;
            foreach (var transaction in target.OrderBy(x => x.Date))
            {
                if (restAmount == 0)
                {
                    break;
                }

                if (transaction.Amount <= restAmount)
                {
                    var entity = new SoldStockEntity
                    {
                        Code = code,
                        Amount = transaction.Amount,
                        BoughtDate = transaction.Date,
                        Profit = (int)(transaction.Price - price) * transaction.Amount,
                        SoldDate = date
                    };
                    this._soldStocks.Add(entity);
                    restAmount -= transaction.Amount;
                    target.Remove(transaction);
                }
                else
                {
                    var entity = new SoldStockEntity
                    {
                        Code = code,
                        Amount = restAmount,
                        BoughtDate = transaction.Date,
                        Profit = (int)(transaction.Price - price) * restAmount,
                        SoldDate = date
                    };
                    this._soldStocks.Add(entity);
                    transaction.Amount -= restAmount;
                    restAmount = 0;
                }
            }

            return new ValueTask(Task.CompletedTask);
        }

        /// <inheritdoc />
        public ValueTask UpsertStockCodeAsync(StockCodeEntity stockCode)
        {
            this._stockCodes[stockCode.Code] = stockCode;
            return new ValueTask(Task.CompletedTask);
        }
    }
}
