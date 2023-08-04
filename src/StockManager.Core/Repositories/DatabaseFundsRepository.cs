using AutoMapper;
using Microsoft.Extensions.Options;
using MySqlConnector;
using StockManager.Core.Entities;
using StockManager.Core.Options;
using StockManager.Core.Utils;

namespace StockManager.Core.Repositories
{
    /// <summary>
    ///     <see cref="IFundsRepository"/> のデータベース実装です。
    /// </summary>
    public class DatabaseFundsRepository : IFundsRepository
    {
        private readonly IMapper _mapper;
        private readonly MySqlConnection _connection;
        private readonly IOptionsMonitor<DatabaseOptions> _option;

        public DatabaseFundsRepository(MySqlConnection connection, IOptionsMonitor<DatabaseOptions> options, IMapper mapper)
        {
            this._connection = connection;
            this._option = options;
            this._mapper = mapper;
        }

        /// <inheritdoc />
        public async ValueTask<IList<FundsHistoryEntity>> FetchFundsHistoryAsync()
        {
            using var command = new MySqlCommand();
            command.Connection = this._connection;

            command.CommandText = $"SELECT id, date, amount, memo, type FROM {this._option.CurrentValue.DatabaseName}.{Constants.FundsHistoryTableName};";
            using var reader = await command.ExecuteReaderAsync();
            var result = new List<FundsHistoryEntity>();

            while (reader.Read())
            {
                result.Add(
                    new FundsHistoryEntity
                    {
                        Index = reader.GetInt32(0),
                        Date = reader.GetDateTime(1),
                        Amount = reader.GetInt32(2),
                        Memo = reader.GetString(3),
                        Type = (FundsHistoryType)reader.GetByte(4),
                    }
                );
            }

            return result;
        }

        /// <inheritdoc />
        public async ValueTask<int> GetCapitalAsync()
        {
            using var command = new MySqlCommand();
            command.Connection = this._connection;

            command.CommandText = $"SELECT capital, profit FROM {this._option.CurrentValue.DatabaseName}.{Constants.FundsTableName};";
            using var reader = await command.ExecuteReaderAsync();

            var result = 0;
            if (reader.Read())
            {
                result = reader.GetInt32(0);
            }

            return result;
        }

        /// <inheritdoc />
        public async ValueTask IncreaseCapitalAsync(int amount)
        {
            using var command = new MySqlCommand();
            command.Connection = this._connection;

            command.CommandText = $"UPDATE {this._option.CurrentValue.DatabaseName}.{Constants.FundsTableName} SET capital = capital + @capital;";
            command.Parameters.Add(new MySqlParameter("@capital", amount));
            await command.ExecuteNonQueryAsync();
        }

        /// <inheritdoc />
        public async ValueTask ReduceCapitalAsync(int amount)
        {
            using var command = new MySqlCommand();
            command.Connection = this._connection;

            command.CommandText = $"UPDATE {this._option.CurrentValue.DatabaseName}.{Constants.FundsTableName} SET capital = capital - @capital;";
            command.Parameters.Add(new MySqlParameter("@capital", amount));
            await command.ExecuteNonQueryAsync();
        }

        public async ValueTask RegisterHistoryAsync(FundsHistoryEntity entity)
        {
            using var command = new MySqlCommand();
            command.Connection = this._connection;

            command.CommandText = $"INSERT INTO {this._option.CurrentValue.DatabaseName}.{Constants.FundsHistoryTableName} (date, amount, memo, type) VALUES(@date, @amount, @memo, @type);";
            command.Parameters.Add(new MySqlParameter("@date", entity.Date));
            command.Parameters.Add(new MySqlParameter("@amount", entity.Amount));
            command.Parameters.Add(new MySqlParameter("@memo", entity.Memo));
            command.Parameters.Add(new MySqlParameter("@type", entity.Type));
            await command.ExecuteNonQueryAsync();
        }
    }
}
