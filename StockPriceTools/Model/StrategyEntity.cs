using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPriceTools.Model
{
    public class StrategyEntity: BaseEntity
    {
        /// <summary>
        /// 策略名称
        /// </summary>
        [Description("策略名称")]
        public string Name { get; set; }

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
        /// 单股最大持仓比例(%)
        /// </summary>
        [Description("单股最大持仓比例(%)")]
        public decimal MaxPositionPer { get; set; } = 20;

        /// <summary>
        /// 加仓股价下跌最大限制(%)
        /// </summary>
        [Description("加仓股价下跌最大限制(%)")]
        public decimal IncreaseMaxSlidePer { get; set; } = -25;

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

        /// <summary>
        /// 股价提醒浮动比例(%)
        /// </summary>
        [Description("股价提醒浮动比例(%)")]
        public decimal RemindPer { get; set; } = 10;

        /// <summary>
        /// 提醒邮箱
        /// </summary>
        [Description("提醒邮箱")]
        public string RemindEmail { get; set; } = "zhupj@foxmail.com";

        /// <summary>
        /// 提醒QQ
        /// </summary>
        [Description("提醒QQ")]
        public string RemindQQ { get; set; } = "47426568";

        /// <summary>
        /// 提醒次数
        /// </summary>
        [Description("提醒次数")]
        public int RemindCount { get; set; } = 3;
    }
}
