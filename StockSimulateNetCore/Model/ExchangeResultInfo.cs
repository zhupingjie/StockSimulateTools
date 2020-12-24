using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateNetCore.Model
{
    public class ExchangeResultInfo
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }
    }
}
