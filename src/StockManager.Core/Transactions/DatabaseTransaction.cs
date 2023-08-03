using MySqlConnector;

namespace StockManager.Core.Transactions
{
    public class DatabaseTransaction : ITransaction
    {
        private readonly MySqlTransaction _transaction;
        private bool _isCommited = false;

        public DatabaseTransaction(MySqlTransaction transaction)
        {
            this._transaction = transaction;
        }

        /// <summary>
        ///     これまで実行した処理をコミットします。
        /// </summary>
        /// <returns>非同期処理の状態。</returns>
        public async ValueTask CommitAsync()
        {
            await _transaction.CommitAsync();
            this._isCommited = true;
        }

        public async ValueTask DisposeAsync()
        {
            if (!this._isCommited)
            {
                await this.RollBackAsync();
            }
        }

        /// <summary>
        ///     これまでに実行した処理をロールバックします。
        /// </summary>
        /// <returns>非同期処理の状態。</returns>
        public ValueTask RollBackAsync()
        {
            return new ValueTask(this._transaction.RollbackAsync());
        }

    }
}
