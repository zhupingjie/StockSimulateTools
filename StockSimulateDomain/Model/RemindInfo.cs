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

        public string UpMinDayPricePer { get; set; }
        public string DownMaxDayPricePer { get; set; }

        public bool GoldMacd { get; set; }
        public bool DieMacd { get; set; }

        public bool UpMacd { get; set; }
        public bool DownMacd { get; set; }
    }
}
