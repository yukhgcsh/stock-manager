using MySqlConnector;
using System.Data;

namespace StockManager.Core.Transactions
{
    public class DatabaseTransactionManager : ITransactionManager
    {
        private readonly MySqlConnection _connection;

        public DatabaseTransactionManager(MySqlConnection connection)
        {
            this._connection = connection;
        }

        public async ValueTask<ITransaction> BeginTransactionAsync()
        {
            if (this._connection.State != ConnectionState.Open)
            {
                await this._connection.OpenAsync();
            }
            var transaction = await this._connection.BeginTransactionAsync();
            return new DatabaseTransaction(transaction);
        }

        public ValueTask OpenAsync()
        {
            if (this._connection.State == ConnectionState.Open)
            {
                return ValueTask.CompletedTask;
            }

            return new ValueTask(this._connection.OpenAsync());
        }
    }
}
