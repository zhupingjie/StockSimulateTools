using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateDomain.Model
{
    public class LeftExchangeStrategyInfo : StrategyInfo
    {
        [Description("建仓价格")]
        public decimal BuyPrice { get; set; }

        [Description("建仓市值")]
        public decimal BuyAmount { get; set; }

        [Description("减仓价格")]
        public decimal SalePrice { get; set; }

        [Description("买入总市值(元)")]
        public decimal TotalBuyAmount { get; set; }

        /// <summary>
        /// 加仓股价浮动比例(%)
        /// </summary>
        [Description("加仓股价浮动比例(%)")]
        public decimal IncreasePricePer { get; set; } = -5;

        /// <summary>
        /// 加仓市值(元)
        /// </summary>
        [Description("加仓市值(元)")]
        public decimal IncreaseAmount { get; set; } = 5000;

        /// <summary>
        /// 加仓股价浮动比例限值(%)
        /// </summary>
        [Description("额外加仓股价浮动比例限值(%)")]
        public decimal IncreaseMorePer { get; set; } = -20;

        /// <summary>
        /// 额外加仓浮动市值比例(%)
        /// </summary>
        [Description("额外加仓浮动市值比例(%)")]
        public decimal IncreaseMoreAmountPer { get; set; } = 30;

        /// <summary>
        /// 额外加仓股价最大比例限制(%)
        /// </summary>
        [Description("额外加仓股价最大比例限制(%)")]
        public decimal IncreaseMaxPer { get; set; } = -30;

        /// <summary>
        /// 额外加仓最大市值比例(%)
        /// </summary>
        [Description("额外加仓最大市值比例(%)")]
        public decimal IncreaseMaxAmountPer { get; set; } = 50;

        /// <summary>
        /// 减仓股价浮动比例(%)
        /// </summary>
        [Description("减仓股价浮动比例(%)")]
        public decimal ReducePricePer { get; set; } = 10;

        /// <summary>
        /// 减仓持仓比例(%)
        /// </summary>
        [Description("减仓持仓比例(%)")]
        public decimal ReducePositionPer { get; set; } = 10;
    }
}
