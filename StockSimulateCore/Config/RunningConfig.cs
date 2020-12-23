using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Config
{
    public class RunningConfig
    {
        private static readonly RunningConfig instance = new RunningConfig();
        public static RunningConfig Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// 调试模式
        /// </summary>
        public bool DebugMode { get; set; }

        /// <summary>
        /// 股票价格采集时间频率(秒)
        /// </summary>
        public int GatherStockPriceInterval { get; set; } = 30;

        /// <summary>
        /// 股票财务指标采集时间频率(秒)
        /// </summary>
        public int GatherStockFinanceTargetInterval { get; set; } = 24 * 60 * 60;

        /// <summary>
        /// 股票研报采集时间频率(秒)
        /// </summary>
        public int GatherStockReportInterval { get; set; } = 24 * 60 * 60;

        /// <summary>
        /// 股票策略提醒时间频率(秒)
        /// </summary>
        public int RemindStockStrategyInterval { get; set; } = 30;

        /// <summary>
        /// 股票价格买卖点浮动比例(%)
        /// </summary>
        public decimal RemindStockPriceFloatPer { get; set; } = 2;

        /// <summary>
        /// 股票持仓盈更新时间频率(秒)
        /// </summary>
        public int UpdateAccountStockProfitInterval { get; set; } = 5 * 60;


        /// <summary>
        /// 股票均线价格计算时间频率(秒)
        /// </summary>
        public int UpdateStockAveragePriceInterval { get; set; } = 30 * 60;
    }
}
