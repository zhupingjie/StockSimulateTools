using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Model
{
    public class AccountStockEntity :BaseEntity
    {
        [Description("交易者")]
        public string AccountName { get; set; }

        [Description("股票代码")]
        public string StockCode { get; set; }

        [Description("买卖策略")]
        public string StrategyName { get; set; }

        [Description("建仓起始价格")]
        public decimal BuyPrice { get; set; }

        [Description("建仓市值")]
        public decimal BuyAmount { get; set; }

        [Description("减仓起始价格")]
        public decimal SalePrice { get; set; }

        [Description("持有数")]
        public int HoldQty { get; set; }

        [Description("持有市值(元)")]
        public decimal HoldAmount { get; set; }

        [Description("买入总市值(元)")]
        public decimal TotalBuyAmount { get; set; }

        [Description("成本(元)")]
        public decimal Cost
        {
            get
            {
                return Math.Round(TotalBuyAmount / HoldQty, 3);
            }
        }

        [Description("盈亏(元)")]
        public decimal Profit
        {
            get
            {
                return HoldAmount - TotalBuyAmount;
            }
        }
    }
}
