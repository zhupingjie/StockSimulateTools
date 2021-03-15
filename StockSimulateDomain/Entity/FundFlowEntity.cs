using StockSimulateDomain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StockSimulateDomain.Entity
{
    [Description("资金流向")]
    public class FundFlowEntity : BaseEntity
    {
        [Description("股票代码")]
        public string StockCode { get; set; } = "";

        [Description("行业名称")]
        public string IndustryName { get; set; } = "";

        [Description("日期")]
        public string DealDate { get; set; }

        [Description("主力净额")]
        public decimal Amount { get; set; }
    }
}
