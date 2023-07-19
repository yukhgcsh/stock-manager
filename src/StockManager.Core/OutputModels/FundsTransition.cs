using StockManager.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManager.Core.OutputModels
{
    public class FundsTransition
    {
        public DateTime Date { get; set; }

        public int Capital { get; set; }
    }
}
