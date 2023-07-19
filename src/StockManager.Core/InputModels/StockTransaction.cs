using StockManager.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManager.Core.InputModels
{
    public class StockTransaction
    {
        public int Code { get; set; }

        public string Name { get; set; } = null!;

        public int Amount { get; set; }

        public string? Memo { get; set; }

        public DateTime Date { get; set; }

        public TransactionType Type { get; set; }

        public double Price { get; set; }
    }
}
