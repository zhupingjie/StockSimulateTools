using System;
using System.Collections.Generic;
using System.Text;

namespace StockSimulateDomain.Model
{
    public class StockCacheInfo
    {
        public int StockID { get; set; }
        public int Type { get; set; }
        public int Focus { get; set; }
        public string StockCode { get; set; }
        public string StockName { get; set; }

        public string ReportDate { get; set; }

        public string PriceDate { get; set; }

        public string CacheDate { get; set; } = DateTime.Now.Date.ToString("yyyy-MM-dd");
    }
}
