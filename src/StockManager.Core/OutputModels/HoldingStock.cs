using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManager.Core.OutputModels
{
    public class HoldingStock
    {
        public int Code { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public int Amount { get; set; }

        public double Price { get; set; }
    }
}
