﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSimulateDomain.Attributes;
using StockSimulateDomain.Data;

namespace StockSimulateDomain.Entity
{
    public class AccountStockEntity :BaseEntity
    {
        [Description("交易账户")]
        public string AccountName { get; set; }

        [Description("股票代码")]
        public string StockCode { get; set; }

        [Description("股票名称")]
        public string StockName { get; set; }

        [Description("股价")]
        public decimal Price { get; set; }

        [Description("浮动(%)")]
        public decimal UDPer { get; set; }

        [Description("持有数")]
        public int HoldQty { get; set; }

        [Description("持有市值(元)")]
        public decimal HoldAmount { get; set; }

        [Description("总市值(元)")]
        public decimal TotalAmount { get; set; }

        [Description("成本(元)")]
        public decimal Cost { get; set; }

        [Description("盈亏(元)")]
        public decimal Profit { get; set; }

        [Description("锁定数量")]
        public int LockQty { get; set; }

        [Description("锁定日期")]
        public DateTime? LockDate { get; set; }
    }
}
