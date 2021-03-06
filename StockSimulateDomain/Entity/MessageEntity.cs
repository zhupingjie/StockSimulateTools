﻿using StockSimulateDomain.Attributes;
using StockSimulateDomain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StockSimulateDomain.Entity
{
    public class MessageEntity : BaseEntity
    {
        [Description("股票代码")]
        public string StockCode { get; set; }

        [Description("股票名称")]
        public string StockName { get; set; }

        [Description("股价")]
        public decimal Price { get; set; }

        [Description("浮动(%)")]
        public decimal UDPer { get; set; }

        [Description("消息内容")]
        public string Title { get; set; }

        [Description("消息时间(Time)")]
        [GridColumnIgnore]
        public DateTime NoticeTime { get; set; }

        [Description("Win读取时间(Time)")]
        public DateTime? WinReadTime { get; set; }

        [Description("App读取时间(Time)")]
        public DateTime? AppReadTime { get; set; }

        [Description("是否处理")]
        [GridColumnIgnore]
        public int Handled { get; set; }
    }
}
