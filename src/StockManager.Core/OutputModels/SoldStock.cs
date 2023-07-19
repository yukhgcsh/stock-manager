using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManager.Core.OutputModels
{
    public class SoldStock
    {
        public int Code { get; set; }

        public string Name { get; set; }

        public DateTime BoughtDate { get; set; }

        public DateTime SoldDate { get; set; }

        public int Amount { get; set; }

        public int Profit { get; set; }
    }
}
