using StockSimulateDomain.Attributes;
using StockSimulateDomain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateDomain.Entity
{
    public class StockEntity : BaseEntity
    {
        [Description("股票类型")]
        [GridColumnIgnore]
        public int Type { get; set; }

        [Description("股票类型")]
        [DBNotMapped]
        public string TypeText
        {
            get
            {
                return Type == 0 ? "沪深股" : Type == 1 ? "基金(ETF)" : Type == 2 ? "指数" : "";
            }
        }

        [Description("关注")]
        [GridColumnIgnore]
        public int Foucs { get; set; }

        [Description("关注")]
        [DBNotMapped]
        public string FoucsText {
            get {
                return (Foucs == 1 ? "★" : Foucs == 2 ? "☆" : "")  + (Top == 1 ? " 🖤" : "");
            }
        }

        [Description("股票代码")]
        public string Code { get; set; }

        [Description("股票名称")]
        [GatherColumn]
        public string Name { get; set; }

        [Description("股价")]
        [GatherColumn]
        public decimal Price { get; set; }

        [Description("浮动(%)")]
        [GatherColumn]
        public decimal UDPer { get; set; }


        [Description("当日开盘价")]
        [GatherColumn]
        public decimal StartPrice { get; set; }

        [Description("当日最高价")]
        [GatherColumn]
        public decimal MaxPrice { get; set; }

        [Description("当日最低价")]
        [GatherColumn]
        public decimal MinPrice { get; set; }

        [Description("安全股价")]
        public decimal Safety { get; set; }

        [Description("预测股价")]
        public decimal Target { get; set; }

        [Description("交易锁定(天)")]
        [GridColumnIgnore]
        public int LockDay { get; set; }

        [Description("总股本(亿股)")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal Capital { get; set; }

        [Description("总市值(亿元)")]
        [GatherColumn]
        public decimal Amount { get; set; }

        [Description("市盈率(动)")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal PE { get; set; }

        [Description("市盈率")]
        [GatherColumn]
        public decimal TTM { get; set; }

        [Description("市净率")]
        [GatherColumn]
        public decimal PB { get; set; }

        [Description("市盈率增长率")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal PEG { get; set; }

        [Description("净资产收益率")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal ROE { get; set; }

        [Description("投入资本回报率")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal ROIC { get; set; }

        [Description("每股收益(元)")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal EPS { get; set; }

        [Description("每股净资产(元)")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal BVPS { get; set; }

        [Description("总营收(亿元)")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal TotalRevenue { get; set; }

        [Description("净利润(亿元)")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal NetProfit { get; set; }

        [Description("营收同比(%)")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal RevenueGrewPer { get; set; }

        [Description("净利润同比(%)")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal ProfitGrewPer { get; set; }

        [Description("毛利率(%)")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal GrossRate { get; set; }

        [Description("净利率(%)")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal NetRate { get; set; }

        [Description("负债率(%)")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal DebtRage { get; set; }


        [Description("预测增长率(%)")]
        [GridColumnIgnore]
        public decimal Growth { get; set; }

        [Description("预测PE")]
        public decimal EPE { get; set; }

        [Description("预测盈利(%)")]
        [DBNotMapped]
        [GridColumnIgnore]
        public decimal UPPer
        {
            get
            {
                if (Safety == 0) return 0;
                return Math.Round((Target - Safety) / Safety * 100m, 2);
            }
        }

        [Description("预测结果")]
        public string Advise { get; set; }

        [Description("报告日期")]
        public string ReportDate { get; set; }

        [Description("股价日期")]
        public string PriceDate { get; set; }

        [Description("置顶")]
        public int Top { get; set; }
    }
}
