﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateService.Config
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
        /// 通知消息邮件提醒
        /// </summary>
        public bool RemindNoticeByEmail { get; set; }

        /// <summary>
        /// 通知消息系统消息提醒
        /// </summary>
        public bool RemindNoticeByMessage { get; set; }

        /// <summary>
        /// 当前交易账号
        /// </summary>
        public string CurrentAccountName { get; set; }

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
        public int UpdateStockAveragePriceInterval { get; set; } = 5 * 60;

        /// <summary>
        /// 提醒消息显示时间(秒)
        /// </summary>
        public int RemindMessageShowTime { get; set; } = 3;


        /// <summary>
        /// 提醒消息读取时间(秒)
        /// </summary>
        public int LoadMessageInterval { get; set; } = 30;

        /// <summary>
        /// 全局配置读取时间(秒)
        /// </summary>
        public int LoadGlobalConfigInterval { get; set; } = 20;

        public string DBServiceIP { get; set; }
        public string DBName { get; set; }
        public string DBUserID { get; set; }
        public string DBPwd { get; set; }
        public int DBPort { get; set; } = 3306;

        public string DBConnectionString
        {
            get
            {
                return $"Server={DBServiceIP}:{DBPort};Database={DBName};User Id={DBUserID};Password={DBPwd};Pooling=true;Max Pool Size={100};Connect Timeout=20;MultipleActiveResultSets=True;";
            }
        }
    }
}
