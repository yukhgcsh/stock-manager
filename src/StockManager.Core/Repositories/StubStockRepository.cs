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
                        Quantity = 100,
                        Code = 1234,
                        Date = new DateTime(2022, 1, 2),
                        Amount = 1000,
                        IsNisa = true,
                    }
                }
            }
        };

        private IList<SoldStockEntity> _soldStocks = new List<SoldStockEntity> {
            new SoldStockEntity
            {
                Quantity = 100,
                Code = 1234,
                BoughtDate = new DateTime(2022, 1, 2),
                SoldDate = new DateTime(2022, 1, 4),
                Profit = 10000,
                IsNisa = true
            },
            new SoldStockEntity
            {
                Quantity = 300,
                Code = 5678,
                BoughtDate = new DateTime(2022, 10, 1),
                SoldDate = new DateTime(2022, 12, 3),
                Profit = 9000,
                IsNisa = false
            },
            new SoldStockEntity
            {
                Quantity = 400,
                Code = 9876,
                BoughtDate = new DateTime(2022, 10, 1),
                SoldDate = new DateTime(2023, 3, 3),
                Profit = -2000,
                IsNisa = false
            },
            new SoldStockEntity
            {
                Quantity = 200,
                Code = 5432,
                BoughtDate = new DateTime(2022, 10, 1),
                SoldDate = new DateTime(2023, 6, 27),
                Profit = 12000,
                IsNisa = false
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
            return ValueTask.CompletedTask;
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
        public ValueTask SellStockAsync(int code, DateTime date, int quantity, double amount, bool isNisa)
        {
            if (!this._holdingStocks.TryGetValue(code, out var target))
            {
                throw new InvalidOperationException($"売却しようとしているコード({code})の株を所有していません。");
            }


            var restQuantity = quantity;
            foreach (var transaction in target.OrderBy(x => x.Date))
            {
                if (restQuantity == 0)
                {
                    break;
                }

                if (transaction.Quantity <= restQuantity)
                {
                    var entity = new SoldStockEntity
                    {
                        Code = code,
                        Quantity = transaction.Quantity,
                        BoughtDate = transaction.Date,
                        Profit = (int)(amount - transaction.Amount) * transaction.Quantity,
                        SoldDate = date,
                        IsNisa = isNisa
                    };
                    this._soldStocks.Add(entity);
                    restQuantity -= transaction.Quantity;
                    target.Remove(transaction);
                }
                else
                {
                    var entity = new SoldStockEntity
                    {
                        Code = code,
                        Quantity = restQuantity,
                        BoughtDate = transaction.Date,
                        Profit = (int)(amount - transaction.Amount) * restQuantity,
                        SoldDate = date,
                        IsNisa = isNisa
                    };
                    this._soldStocks.Add(entity);
                    transaction.Quantity -= restQuantity;
                    restQuantity = 0;
                }
            }

            if(restQuantity != 0)
            {
                throw new InvalidOperationException($"売却しようとしているコード({code})の株を所有していません。");
            }

            return ValueTask.CompletedTask;
        }

        /// <inheritdoc />
        public ValueTask UpsertStockCodeAsync(StockCodeEntity stockCode)
        {
            this._stockCodes[stockCode.Code] = stockCode;
            return ValueTask.CompletedTask;
        }
    }
}
