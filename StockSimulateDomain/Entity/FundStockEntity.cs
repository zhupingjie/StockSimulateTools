using StockSimulateDomain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StockSimulateDomain.Entity
{
    public class FundStockEntity : BaseEntity
    {
        [Description("股票代码")]
        public string StockCode { get; set; }

        [Description("序号")]
        public int Seq { get; set; }

        [Description("持仓股票代码")]
        public string HoldStockCode { get; set; }

        [Description("持仓股票名称")]
        public string HoldStockName { get; set; }

        [Description("占净值比例(%)")]
        public decimal PositionPer { get; set; }

        [Description("持仓股数(万)")]
        public decimal HoldQty { get; set; }

        [Description("持仓市值(%)")]
        public decimal HoldAmount { get; set; }

        [Description("报告日期")]
        public string ReportDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
    }
}
