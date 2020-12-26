using System;
using System.Collections.Generic;
using System.Text;

namespace StockSimulateDomain.Model
{
    public class RemindInfo
    {
        public string AccountName { get; set; }

        public string StockCode { get; set; }
        public string UDPer { get; set; }

        public string UpPrice { get; set; }

        public string DownPrice { get; set; }
        public string UpAveragePrice { get; set; }
        public string DownAveragePrice { get; set; }

        public string UpAveragePriceReverse { get; set; }
        public string DownAveragePriceReverse { get; set; }
    }
}
