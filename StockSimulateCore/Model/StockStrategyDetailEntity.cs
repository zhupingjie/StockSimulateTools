using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Model
{
    public class StockStrategyDetailEntity : BaseEntity
    {
        [Description("股票代码")]
        public string StockCode { get; set; }

        [Description("买卖策略")]
        public string StrategyName { get; set; }

        [Description("买卖点")]
        public string Target { get; set; }

        [Description("股价(元)")]
        public decimal Price { get; set; }

        [Description("股价上限(元)")]
        public decimal MaxPrice { get; set; }

        [Description("股价下限(元)")]
        public decimal MinPrice { get; set; }

        [Description("买入数")]
        public int BuyQty { get; set; }

        [Description("买入市值(元)")]
        public decimal BuyAmount { get; set; }

        [Description("卖出数")]
        public int SaleQty { get; set; }

        [Description("卖出市值(元)")]
        public decimal SaleAmount { get; set; }

        [Description("持有数")]
        public int HoldQty { get; set; }

        [Description("持有市值(元)")]
        public decimal HoldAmount { get; set; }

        [Description("买入总市值(元)")]
        public decimal TotalBuyAmount { get; set; }

        [Description("浮动市值(元)")]
        public decimal FloatAmount { get; set; }

        [Description("成本(元)")]
        public decimal Cost { get; set; }

        [Description("盈亏(元)")]
        public decimal Profit { get; set; }

        [Description("执行策略")]
        public bool Execute { get; set; }
    }
}
