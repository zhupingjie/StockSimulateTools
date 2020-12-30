using System;
using System.Collections.Generic;
using System.Text;

namespace StockSimulateDomain.Model
{
    public class StockResultInfo
    {
        public string Code { get;set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal UDPer { get; set; }
        public decimal TTM { get; set; }
        public decimal Amount { get; set; }
    }
}
