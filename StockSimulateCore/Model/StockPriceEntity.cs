using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Model
{
    public class StockPriceEntity  : BaseEntity
    {
        [Description("股票代码")]
        public string StockCode { get; set; }

        [Description("浮动(%)")]
        public decimal UDPer { get; set; }

        [Description("当前股价(元)")]
        public decimal Price { get; set; }

        [Description("今日开盘价(元)")]
        public decimal TodayStartPrice { get; set; }

        [Description("今日收盘价(元)")]
        public decimal TodayEndPrice { get; set; }

        [Description("今日最高价(元)")]
        public decimal TodayMaxPrice { get; set; }

        [Description("今日最低价(元)")]
        public decimal TodayMinPrice { get; set; }

        [Description("昨日收盘价(元)")]
        public decimal YesterdayEndPrice { get; set; }

        [Description("成交量")]
        public decimal DealQty { get; set; }

        [Description("成交额(亿元)")]
        public decimal DealAmount { get; set; }

        [Description("结算日")]
        public string DealDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

        [Description("总股本(万股)")]
        public decimal Capital { get; set; }

        [Description("总市值(亿元)")]
        public decimal Amount { get; set; }

        [Description("市盈率(TTM)")]
        public decimal TTM { get; set; }

        [Description("市盈率(PE)")]
        public decimal PE { get; set; }
    }
}
