using StockSimulateDomain.Attributes;
using StockSimulateDomain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StockSimulateDomain.Entity
{
    public class StockMacdEntity : BaseEntity
    {
        [Description("股票代码")]
        public string StockCode { get; set; }

        [Description("结算日期")]
        public string DealDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

        [Description("结算时间点")]
        public string DealTime { get; set; }

        [Description("时间类型")]
        public int DateType { get; set; }

        [Description("时间类型")]
        [DBNotMapped]
        public string DateTypeText
        {
            get
            {
                return DateType == 0 ? "日" : DateType == 1 ? "60分钟" : "";
            }
        }

        [Description("EMA12")]
        public decimal EMA12 { get; set; }

        [Description("EMA26")]
        public decimal EMA26 { get; set; }

        [Description("DIF")]
        public decimal DIF { get; set; }

        [Description("DEA")]
        public decimal DEA { get; set; }

        [Description("MACD")]
        public decimal MACD { get; set; }
    }
}
