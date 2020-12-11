using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Model
{
    public class ExchangeOrderEntity : BaseEntity
    {
        [Description("股票代码")]
        public string StockCode { get; set; }

        [Description("成交价格")]
        public double Price { get; set; }

        [Description("成交数量")]
        public int Qty { get; set; }

        [Description("成交市值")]
        public double Amount { get; set; }

        [Description("手续费")]
        public double Charge { get; set; }

        [Description("成交时间")]
        public DateTime ExchangeTime { get; set; } = DateTime.Now;

        [Description("持有数量")]
        public double HoldQty { get; set; }

        [Description("买卖策略")]
        public string Strategy { get; set; }

        [Description("买卖点")]
        public string Target { get; set; }
    }
}
