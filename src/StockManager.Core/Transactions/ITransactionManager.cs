namespace StockManager.Core.Transactions
{
    /// <summary>
    ///     サービス層でのトランザクション管理機能を提供します。
    /// </summary>
    public interface ITransactionManager
    {
        /// <summary>
        ///     トランザクション管理なしに処理を開始します。
        ///     OpenAsync または BeginTransactionAsync のいずれかを使用してください。
        /// </summary>
        /// <returns>非同期処理の状態。</returns>
        public ValueTask OpenAsync();

        /// <summary>
        ///     トランザクションを開始します。
        ///     すでにトランザクションを開始している場合は、そのトランザクションに参加します。
        ///     OpenAsync または BeginTransactionAsync のいずれかを使用してください。
        /// </summary>
        /// <returns>非同期処理の状態。</returns>
        public ValueTask<ITransaction> BeginTransactionAsync();
    }
}
