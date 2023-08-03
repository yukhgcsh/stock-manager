namespace StockManager.Core.Transactions
{
    public class StubTransaction : ITransaction
    {
        public ValueTask CommitAsync()
        {
            return ValueTask.CompletedTask;
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }

        public ValueTask RollBackAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
