namespace StockManager.Core.Entities
{
    /// <summary>
    ///    会社情報を取得または設定します。 
    /// </summary>
    public class StockCodeEntity
    {
        /// <summary>
        ///     銘柄コードを取得または設定します。
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///     会社名を取得または設定します。
        /// </summary>
        public string Name { get; set; } = null!;
    }
}
