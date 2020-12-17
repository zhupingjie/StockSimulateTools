using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Model
{
    public class TExchangeStrategyInfo : StrategyInfo
    {
        [Description("基准价格")]
        public decimal BasePrice { get; set; }

        [Description("数量")]
        public int HoldQty { get; set; }

        [Description("股价下跌幅度(%)")]
        public decimal ReducePricePer { get; set; }

        [Description("股价上涨幅度(%)")]
        public decimal IncreasePricePer { get; set; }

        [Description("交易数量")]
        public int BSQty { get; set; }

        [Description("锁定数量")]
        public int LockQty { get; set; }

        [Description("连续单向买卖最大次数")]
        public int MaxSingleBS { get; set; }

        [Description("连续匹配失败终止次数")]
        public int MaxErrorMatch { get; set; }

        [Description("股价上涨达到终止")]
        public decimal MaxPriceStop { get; set; }

        [Description("股价下跌达到终止")]
        public decimal MinPriceStop { get; set; }
    }
}
