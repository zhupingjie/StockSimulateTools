﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Model
{
    public class AccountStockEntity :BaseEntity
    {
        [Description("交易者")]
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

        [Description("买入总市值(元)")]
        public decimal TotalBuyAmount { get; set; }

        [Description("成本(元)")]
        public decimal Cost { get; set; }

        [Description("盈亏(元)")]
        public decimal Profit { get; set; }
    }
}
