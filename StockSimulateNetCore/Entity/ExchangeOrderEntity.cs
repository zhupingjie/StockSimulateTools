using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateNetCore.Entity
{
    public class ExchangeOrderEntity : BaseEntity
    {
        [Description("交易账户")]
        public string AccountName { get; set; }

        [Description("股票代码")]
        public string StockCode { get; set; }

        [Description("交易类型")]
        public string ExchangeType { get; set; }

        [Description("成交价格")]
        public decimal Price { get; set; }

        [Description("成交数量")]
        public int Qty { get; set; }

        [Description("成交市值")]
        public decimal Amount { get; set; }

        [Description("持有数量")]
        public decimal HoldQty { get; set; }

        [Description("成本")]
        public decimal Cost { get; set; }

        [Description("买卖策略")]
        public string Strategy { get; set; }

        [Description("买卖交易点")]
        public string Target { get; set; }

        [Description("最后成交时间")]
        public DateTime ExchangeTime { get; set; } = DateTime.Now;

    }
}
