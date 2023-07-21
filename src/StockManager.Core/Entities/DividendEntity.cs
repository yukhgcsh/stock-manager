﻿namespace StockManager.Core.Entities
{
    /// <summary>
    ///     配当金の情報を定義します。
    /// </summary>
    public class DividendEntity
    {
        /// <summary>
        ///     主キー用のインデックスを取得または設定します。
        ///     自動採番のため設定不要。
        /// </summary>
        public int? Index { get; set; }

        /// <summary>
        ///     銘柄コードを取得または設定します。
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///     支給日付を取得または設定します。
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     配当金額を取得または設定します。
        /// </summary>
        public int Profit { get; set; }
    }
}
