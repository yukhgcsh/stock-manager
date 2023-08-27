using Microsoft.Extensions.Options;
using MySqlConnector;
using StockManager.Core.Entities;
using StockManager.Core.Options;
using StockManager.Core.Utils;

namespace StockManager.Core.Repositories
{
    /// <summary>
    ///     <see cref="IInvestmentTrustHistoryRepository"/> のデータベース実装です。
    /// </summary>
    public class DatabaseInvestmentTrustHistoryRepository : IInvestmentTrustHistoryRepository
    {
        private readonly MySqlConnection _connection;
        private readonly IOptionsMonitor<DatabaseOptions> _option;

        /// <summary>
        ///     新しいインスタンスを作成します。
        /// </summary>
        /// <param name="connection">MySQLへのコネクション。</param>
        public DatabaseInvestmentTrustHistoryRepository(MySqlConnection connection, IOptionsMonitor<DatabaseOptions> option)
        {
            this._connection = connection;
            this._option = option;
        }

        /// <inheritdoc />
        public async ValueTask<IEnumerable<InvestmentTrustHistoryEntity>> FetchAsync(TimeSpan? period = null)
        {
            using var command = this._connection.CreateCommand();
            if (period == null)
            {
                command.CommandText = $"SELECT id, code, name, date, quantity, price, unit, type, is_nisa, commission, memo FROM {this._option.CurrentValue.DatabaseName}.{Constants.InvestmentTrustHistoryTableName};";
            }
            else
            {
                var time = DateTime.Now - period;
                command.CommandText = $"SELECT id, code, name, date, quantity, price, unit, type, is_nisa, commission, memo FROM {this._option.CurrentValue.DatabaseName}.{Constants.InvestmentTrustHistoryTableName} WHERE date >= @time;";
                command.Parameters.Add(new MySqlParameter("@time", time));
            }

            var result = new List<InvestmentTrustHistoryEntity>();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    result.Add(
                        new InvestmentTrustHistoryEntity
                        {
                            Index = reader.GetInt32(0),
                            Code = reader.GetString(1),
                            Name = reader.GetString(2),
                            Date = reader.GetDateTime(3),
                            Quantity = reader.GetInt32(4),
                            Amount = reader.GetDouble(5),
                            Unit = reader.GetInt32(6),
                            Type = (TransactionType)reader.GetByte(7),
                            IsNisa = reader.GetBoolean(8),
                            Commission = reader.GetInt32(9),
                            Memo = reader.GetString(10)
                        }
                    );
                }
            }

            return result;
        }

        /// <inheritdoc />
        public async ValueTask RegisterAsync(InvestmentTrustHistoryEntity entity)
        {
            using var command = this._connection.CreateCommand();
            command.CommandText = $"INSERT INTO {this._option.CurrentValue.DatabaseName}.{Constants.InvestmentTrustHistoryTableName} (code, name, date, quantity, price, unit, type, is_nisa, commission, memo) VALUES (@code, @name, @date, @quantity, @price, @unit, @type, @is_nisa, @commission, @memo);";
            command.Parameters.Add(new MySqlParameter("@code", entity.Code));
            command.Parameters.Add(new MySqlParameter("@name", entity.Name));
            command.Parameters.Add(new MySqlParameter("@date", entity.Date));
            command.Parameters.Add(new MySqlParameter("@quantity", entity.Quantity));
            command.Parameters.Add(new MySqlParameter("@price", entity.Amount));
            command.Parameters.Add(new MySqlParameter("@unit", entity.Unit));
            command.Parameters.Add(new MySqlParameter("@type", entity.Type));
            command.Parameters.Add(new MySqlParameter("@is_nisa", entity.IsNisa));
            command.Parameters.Add(new MySqlParameter("@commission", entity.Commission));
            command.Parameters.Add(new MySqlParameter("@memo", entity.Memo));

            await command.ExecuteNonQueryAsync();
        }
    }
}
