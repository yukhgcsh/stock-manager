namespace StockManager.Core.Options
{
    public class DatabaseOptions
    {
        public const string OptionName = "Database";

        public string DatabaseName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
