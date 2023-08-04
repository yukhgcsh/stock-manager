using Microsoft.Extensions.Options;
using MySqlConnector;
using StockManager.Core.Entities;
using StockManager.Core.Options;
using StockManager.Core.Utils;
using System.Data;

namespace StockManager.Core.Repositories
{
    /// <summary>
    ///     <see cref="IStockHistoryRepository"/> のデータベース実装です。
    /// </summary>
    public class DatabaseStockHistoryRepository : IStockHistoryRepository
    {
        private readonly MySqlConnection _connection;
        private readonly IOptionsMonitor<DatabaseOptions> _option;

        public DatabaseStockHistoryRepository(MySqlConnection connection, IOptionsMonitor<DatabaseOptions> options)
        {
            this._connection = connection;
            this._option = options;
        }

        public async ValueTask<IEnumerable<StockTransactionHistoryEntity>> FetchHistoryAsync(TimeSpan? period)
        {
            using var command = new MySqlCommand();
            command.Connection = this._connection;

            if (period == null)
            {
                command.CommandText = $"SELECT id, code, date, quantity, price, type, memo FROM {this._option.CurrentValue.DatabaseName}.{Constants.StockTransactionHistoryTableName};";
            }
            else
            {
                var time = DateTime.Now - period;
                command.CommandText = $"SELECT id, code, date, quantity, price, type, memo FROM {this._option.CurrentValue.DatabaseName}.{Constants.StockTransactionHistoryTableName} WHERE date >= @time;";
                command.Parameters.Add(new MySqlParameter("@time", time));
            }
            using var reader = await command.ExecuteReaderAsync();
            var result = new List<StockTransactionHistoryEntity>();

            while (reader.Read())
            {
                result.Add(
                    new StockTransactionHistoryEntity
                    {
                        Index = reader.GetInt32(0),
                        Code = reader.GetInt32(1),
                        Date = reader.GetDateTime(2),
                        Quantity = reader.GetInt32(3),
                        Amount = reader.GetDouble(4),
                        Type = (TransactionType)reader.GetByte(5),
                        Memo = reader.GetString(6),
                    }
                );
            }

            return result;
        }

        /// <inheritdoc />
        public async ValueTask RegisterTransactionAsync(StockTransactionHistoryEntity transaction)
        {
            using var command = new MySqlCommand();
            command.Connection = this._connection;

            command.CommandText = $"INSERT INTO {this._option.CurrentValue.DatabaseName}.{Constants.StockTransactionHistoryTableName} (code, date, quantity, price, type, memo) VALUES (@code, @date, @quantity, @price, @type, @memo);";
            command.Parameters.Add(new MySqlParameter("@code", transaction.Code));
            command.Parameters.Add(new MySqlParameter("@date", transaction.Date));
            command.Parameters.Add(new MySqlParameter("@quantity", transaction.Quantity));
            command.Parameters.Add(new MySqlParameter("@price", transaction.Amount));
            command.Parameters.Add(new MySqlParameter("@type", transaction.Type));
            command.Parameters.Add(new MySqlParameter("@memo", transaction.Memo));
            await command.ExecuteNonQueryAsync();
        }
    }
}
