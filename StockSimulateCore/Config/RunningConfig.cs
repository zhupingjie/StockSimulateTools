using System;
using System.Collections.Generic;
using System.IO;
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
        /// 采集历史数据
        /// </summary>
        public bool GatherHistoryStockPrice { get; set; } = false;
        /// <summary>
        /// 采集历史股价起始日期
        /// </summary>
        public string GatherHistoryStockPriceStartDate { get; set; } = "20220321";
        public string GatherHistoryStockPriceEndDate { get; set; } = "20220321";

        /// <summary>
        /// 股票财务研报采集时间频率(秒)
        /// </summary>
        public int GatherStockFinanceReportInterval { get; set; } = 24;

        /// <summary>
        /// 股票策略提醒时间频率(秒)
        /// </summary>
        public int RemindStockStrategyInterval { get; set; } = 2 * 60;

        /// <summary>
        /// 股票价格买卖点浮动比例(%)
        /// </summary>
        public decimal RemindStockPriceFloatPer { get; set; } = 2;

        /// <summary>
        /// 股票持仓盈更新时间频率(秒)
        /// </summary>
        public int UpdateAccountStockProfitInterval { get; set; } = 5 * 60;


        /// <summary>
        /// 股票辅助指标计算时间频率(秒)(均线/MACD)
        /// </summary>
        public int UpdateStockAssistTargetInterval { get; set; } = 5 * 60;


        /// <summary>
        /// 股票辅助指标数据保留天数(天)
        /// </summary>
        public int KeepStockAssistTargetDays { get; set; } = 5;

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
        public int LoadGlobalConfigInterval { get; set; } = 300;

        /// <summary>
        /// 行业研报采集天数
        /// </summary>
        public int GatherIndustryReportPreDays { get; set; } = 0;


        public int BatchExecuteSQLRowCount { get; set; } = 1000;

        public string BaseDirectory
        {
            get
            {
                return Path.Combine(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, "cache");
            }
        }

        public string SaveReportFilePath { get; set; } = "report";

        /// <summary>
        /// 上证指数
        /// </summary>
        public string SHZSOfStockCode { get; set; } = "SH000001";

        public string DBServiceIP { get; set; } = "localhost";
        public string DBName { get; set; } = "stock";
        public string DBUserID { get; set; } = "root";
        public string DBPwd { get; set; } = "root";
        public int DBPort { get; set; } = 3306;

        //Server=121.4.29.105;Database=stock;User Id=root;Password=sa!123456;Pooling=true;Max Pool Size={100};Connect Timeout=20;
        public string DBConnectionString
        {
            get
            {
                return $"Server={DBServiceIP};Database={DBName};User Id={DBUserID};Password={DBPwd};Connect Timeout=20;";
            }
        }
    }
}
