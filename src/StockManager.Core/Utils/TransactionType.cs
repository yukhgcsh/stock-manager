namespace StockManager.Core.Utils
{
    /// <summary>
    ///     金銭の授受が発生する取引種類を定義します。
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        ///     購入。
        /// </summary>
        Buy = 1,

        /// <summary>
        ///     売却。
        /// </summary>
        Sell = 2,

        /// <summary>
        ///     配当金、分配金
        /// </summary>
        Dividend = 3
    }
}