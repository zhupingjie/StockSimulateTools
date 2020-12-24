using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateNetCore.Model
{
    public class ValuateResultInfo
    {
        public string StockCode { get; set; }
        public decimal Growth { get; set; }
        public decimal PE { get; set; }
        public decimal NetProfit { get; set; }
        public decimal LostNetProfit { get; set; }
        public decimal EPS { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public decimal UPPer { get; set; }
        public decimal SafePrice { get; set; }
        public decimal SafeUPPer { get; set; }
        public string Advise { get; set; }
    }
}
