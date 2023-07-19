using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManager.Core.Entities
{
    public class StockCodeEntity
    {
        public int Code { get; set; }

        public string Name { get; set; } = null!;
    }
}
