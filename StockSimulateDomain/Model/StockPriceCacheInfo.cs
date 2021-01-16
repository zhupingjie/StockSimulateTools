using System;
using System.Collections.Generic;
using System.Text;

namespace StockSimulateDomain.Model
{
    public class StockPriceCacheInfo
    {
        public string StockCode { get; set; }
        public string DealDate { get; set; }
        public decimal Price { get; set; }
        public decimal DealQty { get; set; }
    }
}
