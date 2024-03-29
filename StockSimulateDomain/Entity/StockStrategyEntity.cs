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
    public class StockStrategyEntity : BaseEntity
    {
        [Description("股票代码")]
        public string StockCode { get; set; }

        [Description("策略名称")]
        public string StrategyName { get; set; }

        [Description("交易账户")]
        public string AccountName { get; set; }

        [Description("执行策略")]
        [GridColumnIgnore]
        public int ExecuteMode { get; set; }

        [Description("执行策略")]
        [DBNotMapped]
        public string ExecuteModeText
        {
            get
            {
                return ExecuteMode == 0 ? "买卖提醒" : ExecuteMode == 1 ? "自动交易" : "";
            }
        }

        [Description("策略买卖点")]
        public string Target { get; set; }

        [Description("条件")]
        [GridColumnIgnore]
        public int Condition { get; set; }
        [Description("条件")]
        [DBNotMapped]
        public string 条件Text
        {
            get
            {
                return Condition == 0 ? "低于" : Condition == 1 ? "高于" : Condition == 2 ? "等待" : "";
            }
        }
        

        [Description("股价")]
        public decimal Price { get; set; }

        [Description("买入数")]
        public int BuyQty { get; set; }

        [Description("买入市值(元)")]
        public decimal BuyAmount { get; set; }

        [Description("卖出数")]
        public int SaleQty { get; set; }

        [Description("卖出市值(元)")]
        public decimal SaleAmount { get; set; }

        [Description("持有数")]
        public int HoldQty { get; set; }

        [Description("持有市值(元)")]
        public decimal HoldAmount { get; set; }

        [Description("买入总市值(元)")]
        public decimal TotalBuyAmount { get; set; }

        [Description("浮动市值(元)")]
        public decimal FloatAmount { get; set; }

        [Description("成本(元)")]
        public decimal Cost { get; set; }

        [Description("盈亏(元)")]
        public decimal Profit { get; set; }

        [Description("执行结果")]
        [GridColumnIgnore]
        public int ExecuteOK { get; set; }
        [Description("执行结果")]
        [DBNotMapped]
        public string ExecuteOKText
        {
            get
            {
                return ExecuteOK == 1 ? "成功" : ExecuteOK == 2 ? "失败" : ExecuteOK == 0 ? "等待" : "";
            }
        }

        [Description("异常信息")]
        public string Message { get; set; }

        [Description("批策略号")]
        [GridColumnIgnore]
        public string BatchNo { get; set; }

        [Description("策略源类型")]
        [GridColumnIgnore]
        public string StrategyInfoType { get; set; }

        [Description("策略源内容")]
        [GridColumnIgnore]
        public string StrategySource { get; set; }
    }
}
