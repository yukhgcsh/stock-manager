namespace StockManager.Core.Transactions
{
    /// <summary>
    ///     <see cref="ITransactionManager"/> のスタブ実装です。
    /// </summary>
    public class StubTransactionManager : ITransactionManager
    {
        public ValueTask<ITransaction> BeginTransactionAsync()
        {
            return new ValueTask<ITransaction>(new StubTransaction());
        }

        public ValueTask OpenAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
