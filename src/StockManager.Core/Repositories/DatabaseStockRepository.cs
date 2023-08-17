using Microsoft.Extensions.Options;
using MySqlConnector;
using StockManager.Core.Entities;
using StockManager.Core.Options;
using StockManager.Core.Utils;
using System.Data;

namespace StockManager.Core.Repositories
{
    /// <summary>
    ///     <see cref="IStockRepository"/> のデータベース実装です。
    /// </summary>
    public class DatabaseStockRepository : IStockRepository
    {
        private readonly MySqlConnection _connection;
        private readonly IOptionsMonitor<DatabaseOptions> _option;

        public DatabaseStockRepository(MySqlConnection connection, IOptionsMonitor<DatabaseOptions> options)
        {
            this._connection = connection;
            this._option = options;
        }

        /// <inheritdoc />
        public async ValueTask BuyStockAsync(HoldingStockEntity entity)
        {
            using var command = new MySqlCommand();
            command.Connection = this._connection;

            command.CommandText = $"INSERT INTO {this._option.CurrentValue.DatabaseName}.{Constants.HoldingStockTableName} (code, date, quantity, price, is_nisa) VALUES (@code, @date, @quantity, @price, @is_nisa);";
            command.Parameters.Add(new MySqlParameter("@code", entity.Code));
            command.Parameters.Add(new MySqlParameter("@date", entity.Date));
            command.Parameters.Add(new MySqlParameter("@quantity", entity.Quantity));
            command.Parameters.Add(new MySqlParameter("@price", entity.Amount));
            command.Parameters.Add(new MySqlParameter("@is_nisa", entity.IsNisa));
            await command.ExecuteNonQueryAsync();
        }

        /// <inheritdoc />
        public async ValueTask<IEnumerable<HoldingStockEntity>> GetHoldingStocksAsync()
        {
            using var selectCommand = new MySqlCommand();
            selectCommand.Connection = this._connection;

            selectCommand.CommandText = $"SELECT id, code, date, quantity, price, is_nisa FROM {this._option.CurrentValue.DatabaseName}.{Constants.HoldingStockTableName};";
            using var reader = await selectCommand.ExecuteReaderAsync();
            var holdingStocks = new List<HoldingStockEntity>();

            while (reader.Read())
            {
                holdingStocks.Add(
                    new HoldingStockEntity
                    {
                        Index = reader.GetInt32(0),
                        Code = reader.GetInt32(1),
                        Date = reader.GetDateTime(2),
                        Quantity = reader.GetInt32(3),
                        Amount = reader.GetDouble(4),
                        IsNisa = reader.GetBoolean(5),
                    }
                );
            }

            return holdingStocks;
        }

        /// <inheritdoc />
        public async ValueTask<IEnumerable<SoldStockEntity>> GetSoldStocksAsync()
        {
            using var selectCommand = new MySqlCommand();
            selectCommand.Connection = this._connection;

            selectCommand.CommandText = $"SELECT id, code, bought_date, sold_date, quantity, profit, is_nisa  FROM {this._option.CurrentValue.DatabaseName}.{Constants.SoldStockTableName};";
            using var reader = await selectCommand.ExecuteReaderAsync();
            var soldStocks = new List<SoldStockEntity>();

            while (reader.Read())
            {
                soldStocks.Add(
                    new SoldStockEntity
                    {
                        Index = reader.GetInt32(0),
                        Code = reader.GetInt32(1),
                        BoughtDate = reader.GetDateTime(2),
                        SoldDate = reader.GetDateTime(3),
                        Quantity = reader.GetInt32(4),
                        Profit = reader.GetInt32(5),
                        IsNisa = reader.GetBoolean(6)
                    }
                );
            }

            return soldStocks;
        }

        /// <inheritdoc />
        public async ValueTask<IEnumerable<StockCodeEntity>> GetStockCodesAsync()
        {
            using var selectCommand = new MySqlCommand();
            selectCommand.Connection = this._connection;

            selectCommand.CommandText = $"SELECT code, name FROM {this._option.CurrentValue.DatabaseName}.{Constants.StockCodeTableName};";
            var stockCodes = new List<StockCodeEntity>();
            using var reader = await selectCommand.ExecuteReaderAsync();
            while (reader.Read())
            {
                stockCodes.Add(
                    new StockCodeEntity
                    {
                        Code = reader.GetInt32(0),
                        Name = reader.GetString(1),
                    }
                );
            }

            return stockCodes;
        }

        /// <inheritdoc />
        public async ValueTask SellStockAsync(int code, DateTime date, int quantity, double amount, bool isNisa)
        {
            var holdingStocks = await this.GetHoldingStocksAsync();

            var restQuantity = quantity;
            foreach (var holdingStock in holdingStocks.Where(x => x.Code == code).OrderBy(x => x.Date))
            {
                if (holdingStock.Quantity >= restQuantity)
                {
                    using var insertCommand = new MySqlCommand();
                    insertCommand.Connection = this._connection;

                    insertCommand.CommandText = $"INSERT INTO {this._option.CurrentValue.DatabaseName}.{Constants.SoldStockTableName} (code, bought_date, sold_date, quantity, profit, is_nisa) VALUES (@code, @bought_date, @sold_date, @quantity, @profit, @is_nisa);";
                    insertCommand.Parameters.Add(new MySqlParameter("@code", code));
                    insertCommand.Parameters.Add(new MySqlParameter("@bought_date", holdingStock.Date));
                    insertCommand.Parameters.Add(new MySqlParameter("@sold_date", date));
                    insertCommand.Parameters.Add(new MySqlParameter("@quantity", quantity));
                    insertCommand.Parameters.Add(new MySqlParameter("@profit", (amount - holdingStock.Amount) * restQuantity));
                    insertCommand.Parameters.Add(new MySqlParameter("@is_nisa", isNisa));
                    await insertCommand.ExecuteNonQueryAsync();
                    restQuantity = 0;
                    break;
                }
                else
                {
                    using var insertCommand = new MySqlCommand();
                    insertCommand.Connection = this._connection;

                    insertCommand.CommandText = $"INSERT INTO {this._option.CurrentValue.DatabaseName}.{Constants.SoldStockTableName} (code, bought_date, sold_date, quantity, profit, is_nisa) VALUES (@code, @bought_date, @sold_date, @quantity, @profit, @is_nisa);";
                    insertCommand.Parameters.Add(new MySqlParameter("@code", code));
                    insertCommand.Parameters.Add(new MySqlParameter("@bought_date", holdingStock.Date));
                    insertCommand.Parameters.Add(new MySqlParameter("@sold_date", date));
                    insertCommand.Parameters.Add(new MySqlParameter("@quantity", quantity));
                    insertCommand.Parameters.Add(new MySqlParameter("@profit", (amount - holdingStock.Amount) * holdingStock.Quantity));
                    insertCommand.Parameters.Add(new MySqlParameter("@is_nisa", isNisa));
                    await insertCommand.ExecuteNonQueryAsync();
                    restQuantity -= holdingStock.Quantity;
                }
            }

            if (restQuantity != 0)
            {
                throw new InvalidOperationException($"売却しようとしているコード({code})の株を所有していません。");
            }
        }

        /// <inheritdoc />
        public async ValueTask UpsertStockCodeAsync(StockCodeEntity stockCode)
        {
            var stockCodes = await this.GetStockCodesAsync();

            var target = stockCodes.SingleOrDefault(x => x.Code == stockCode.Code);
            if (target == null)
            {
                using var insertCommand = new MySqlCommand();
                insertCommand.Connection = this._connection;

                insertCommand.CommandText = $"INSERT INTO {this._option.CurrentValue.DatabaseName}.{Constants.StockCodeTableName} (code, name) VALUES (@code, @name);";
                insertCommand.Parameters.Add(new MySqlParameter("@code", stockCode.Code));
                insertCommand.Parameters.Add(new MySqlParameter("@name", stockCode.Name));
                await insertCommand.ExecuteNonQueryAsync();
            }
            else
            {
                using var updateCommand = new MySqlCommand();
                updateCommand.Connection = this._connection;

                updateCommand.CommandText = $"UPDATE {this._option.CurrentValue.DatabaseName}.{Constants.StockCodeTableName} SET name = @name WHERE code = @code;";
                updateCommand.Parameters.Add(new MySqlParameter("@code", stockCode.Code));
                updateCommand.Parameters.Add(new MySqlParameter("@name", stockCode.Name));
                await updateCommand.ExecuteNonQueryAsync();
            }
        }
    }
}
