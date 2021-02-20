using StockSimulateDomain.Attributes;
using StockSimulateDomain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StockSimulateDomain.Entity
{
    public class StockForecastEntity : BaseEntity
    {
        [Description("股票代码")]
        public string StockCode { get; set; }

        [Description("基准年份")]
        public string BaseYear { get; set; }

        [Description("预测年份")]
        public string TargetYear { get; set; }

        [Description("每股收益")]
        public decimal Mgsy { get; set; }

        [Description("每股收益增加率(%)")]
        public decimal MgsyRaito { get; set; }

        [Description("净资产收益率")]
        public decimal ROE { get; set; }

        [Description("PE")]
        public decimal PE { get; set; }

        [Description("归属净利润(亿)")]
        public decimal Gsjlr { get; set; }

        [Description("营业收入(亿)")]
        public decimal Yysr { get; set; }
    }
}
