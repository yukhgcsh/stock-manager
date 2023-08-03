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

            command.CommandText = $"INSERT INTO {this._option.CurrentValue.DatabaseName}.{Constants.HoldingStockTableName} (code, date, quantity, price) VALUES (@code, @date, @quantity, @price);";
            command.Parameters.Add(new MySqlParameter("@code", entity.Code));
            command.Parameters.Add(new MySqlParameter("@date", entity.Date));
            command.Parameters.Add(new MySqlParameter("@quantity", entity.Quantity));
            command.Parameters.Add(new MySqlParameter("@price", entity.Amount));
            await command.ExecuteNonQueryAsync();
        }

        /// <inheritdoc />
        public async ValueTask<IEnumerable<HoldingStockEntity>> GetHoldingStocksAsync()
        {
            using var selectCommand = new MySqlCommand();
            selectCommand.Connection = this._connection;

            selectCommand.CommandText = $"SELECT id, code, date, quantity, price FROM {this._option.CurrentValue.DatabaseName}.{Constants.HoldingStockTableName};";
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

            selectCommand.CommandText = $"SELECT id, code, bought_date, sold_date, quantity, profit  FROM {this._option.CurrentValue.DatabaseName}.{Constants.SoldStockTableName};";
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
        public async ValueTask SellStockAsync(int code, DateTime date, int quantity, double price)
        {
            var holdingStocks = await this.GetHoldingStocksAsync();

            var restQuantity = quantity;
            foreach (var holdingStock in holdingStocks.Where(x => x.Code == code).OrderBy(x => x.Date))
            {
                if (holdingStock.Quantity >= restQuantity)
                {
                    using var insertCommand = new MySqlCommand();
                    insertCommand.Connection = this._connection;

                    insertCommand.CommandText = $"INSERT INTO {this._option.CurrentValue.DatabaseName}.{Constants.SoldStockTableName} (code, bought_date, sold_date, quantity, profit) VALUES (@code, @bought_date, @sold_date, @quantity, @profit);";
                    insertCommand.Parameters.Add(new MySqlParameter("@code", code));
                    insertCommand.Parameters.Add(new MySqlParameter("@bought_date", holdingStock.Date));
                    insertCommand.Parameters.Add(new MySqlParameter("@sold_date", date));
                    insertCommand.Parameters.Add(new MySqlParameter("@quantity", quantity));
                    insertCommand.Parameters.Add(new MySqlParameter("@profit", (price - holdingStock.Amount) * restQuantity));
                    await insertCommand.ExecuteNonQueryAsync();
                    break;
                }
                else
                {
                    using var insertCommand = new MySqlCommand();
                    insertCommand.Connection = this._connection;

                    insertCommand.CommandText = $"INSERT INTO {this._option.CurrentValue.DatabaseName}.{Constants.SoldStockTableName} (code, bought_date, sold_date, quantity, profit) VALUES (@code, @bought_date, @sold_date, @quantity, @profit);";
                    insertCommand.Parameters.Add(new MySqlParameter("@code", code));
                    insertCommand.Parameters.Add(new MySqlParameter("@bought_date", holdingStock.Date));
                    insertCommand.Parameters.Add(new MySqlParameter("@sold_date", date));
                    insertCommand.Parameters.Add(new MySqlParameter("@quantity", quantity));
                    insertCommand.Parameters.Add(new MySqlParameter("@profit", (price - holdingStock.Amount) * holdingStock.Quantity));
                    await insertCommand.ExecuteNonQueryAsync();
                    restQuantity -= holdingStock.Quantity;
                }
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
                updateCommand.Parameters.Add(new MySqlParameter("@bane", stockCode.Name));
                await updateCommand.ExecuteNonQueryAsync();
            }
        }
    }
}
