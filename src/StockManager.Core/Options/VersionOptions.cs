namespace StockManager.Core.Options
{
    /// <summary>
    ///     バージョンのオプションを定義します。
    /// </summary>
    public class VersionOptions
    {
        public const string OptionName = "Version";

        /// <summary>
        ///     アプリケーションのバージョンを取得または設定します。
        /// </summary>
        public Version Version { get; set; } = null!;
    }
}
