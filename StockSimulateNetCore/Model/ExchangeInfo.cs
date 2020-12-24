using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateNetCore.Model
{
    public class ExchangeInfo
    {
        public string AccountName { get; set; }
        public string StockCode { get; set; }
        public string StrategyName { get; set; }
        public string Target { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }

        public DateTime ExchangeTime { get; set; } = DateTime.Now;

    }
}
