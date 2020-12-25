using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateDomain.Model
{
    public class StrategyInfo
    {
        /// <summary>
        /// 策略名称
        /// </summary>
        [Description("策略名称")]
        public string Name { get; set; }

        [Description("交易账户")]
        public string AccountName { get; set; }

        [Description("执行策略")]
        public int ExecuteMode { get; set; }

        [Description("股票代码")]
        public string StockCode { get; set; }
    }
}
