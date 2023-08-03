namespace StockManager.Core.Transactions
{
    public interface ITransaction : IAsyncDisposable
    {
        public ValueTask CommitAsync();

        public ValueTask RollBackAsync();
    }
}
