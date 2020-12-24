using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateNetCore.Entity
{
    public class StockAverageEntity : BaseEntity
    {
        [Description("股票代码")]
        public string StockCode { get; set; }

        [Description("结算日期")]
        public string DealDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

        [Description("收盘价")]
        public decimal Price { get; set; }

        [Description("5日均价")]
        public decimal AvgPrice5 { get; set; }

        [Description("10日均价")]
        public decimal AvgPrice10 { get; set; }

        [Description("20日均价")]
        public decimal AvgPrice20 { get; set; }

        [Description("60日均价")]
        public decimal AvgPrice60 { get; set; }

        [Description("120日均价")]
        public decimal AvgPrice120 { get; set; }

        [Description("180日均价")]
        public decimal AvgPrice180 { get; set; }

        [Description("360日均价")]
        public decimal AvgPrice360 { get; set; }
    }
}
