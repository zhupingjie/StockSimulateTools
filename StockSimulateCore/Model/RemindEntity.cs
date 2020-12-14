﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Model
{
    public class RemindEntity : BaseEntity
    {
        [Description("股票代码")]
        public string StockCode { get; set; }

        /// <summary>
        /// 0:涨跌幅,1:股价涨跌,2:买卖策略
        /// </summary>
        [Description("类型")]
        public int RType { get; set; }

        [Description("基准价格")]
        public decimal BasePrice { get; set; }

        [Description("目标")]
        public decimal Target { get; set; }

        [Description("股价上限")]
        public decimal MaxPrice { get; set; }

        [Description("股价下限")]
        public decimal MinPrice { get; set; }

        [Description("提醒内容")]
        public string Title { get; set; }

        [Description("提醒邮箱")]
        public string Email { get; set; }

        [Description("提醒QQ")]
        public string QQ { get; set; }

        [Description("是否执行")]
        public bool Handled { get; set; }
    }
}
