using StockSimulateDomain.Attributes;
using StockSimulateDomain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [GridColumnIgnore]
        public string TypeText
        {
            get
            {
                return Type == 0 ? "沪深股" : Type == 1 ? "基金(ETF)" : Type == 2 ? "指数" : "";
            }
        }

        [Description("股票代码")]
        public string Code { get; set; }

        [Description("股票名称")]
        [GatherColumn]
        public string Name { get; set; }

        [Description("行业名称")]
        [GatherColumn]
        public string IndustryName { get; set; }

        [Description("关注")]
        [GridColumnIgnore]
        public int Foucs { get; set; }

        [Description("排序")]
        [GridColumnIgnore]
        public int Seq { get; set; }


        //[Description("关注")]
        //[DBNotMapped]
        //public string FoucsText {
        //    get {
        //        return (Foucs == 1 ? "★" : Foucs == 2 ? "☆" : "")  + (Top == 1 ? " 🖤" : "");
        //    }
        //}

        [Description("浮动(%)")]
        [GatherColumn]
        public decimal UDPer { get; set; }

        [Description("股价")]
        [GatherColumn]
        public decimal Price { get; set; }

        [Description("股价E")]
        [GridColumnIgnore]
        public decimal EPrice { get; set; }

        [Description("目标股价")]
        [GridColumnIgnore]
        public decimal Target { get; set; }

        [Description("预测股价")]
        [DBNotMapped]
        public decimal GoPrice
        {
            get
            {
                if (Target != 0) return Target;
                if (EPrice != 0) return EPrice;
                return 0;
            }
        }

        [Description("安全股价")]
        public decimal Safety { get; set; }

        [Description("市盈率")]
        [GatherColumn]
        public decimal TTM { get; set; }

        [Description("市盈率E")]
        public decimal EPE { get; set; }

        [Description("PEG")]
        [GatherColumn]
        public decimal PEG { get; set; }

        [Description("推荐操作")]
        public string Advise { get; set; }

        [Description("市净率")]
        [GatherColumn]
        public decimal PB { get; set; }

        [Description("ROE")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal ROE { get; set; }

        [Description("每股收益E(元)")]
        [GridColumnIgnore]
        public decimal EEPS { get; set; }

        [Description("净利润E(亿)")]
        [GridColumnIgnore]
        public decimal ENetProfit { get; set; }

        [Description("净利润同比E(%)")]
        [GridColumnIgnore]
        public decimal EGrowth { get; set; }



        [Description("今开盘价")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal StartPrice { get; set; }

        [Description("今最高价")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal MaxPrice { get; set; }

        [Description("今最低价")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal MinPrice { get; set; }


        [Description("交易锁定")]
        [GridColumnIgnore]
        public int LockDay { get; set; }

        [Description("总股本(亿)")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal Capital { get; set; }

        [Description("总市值(亿)")]
        [GatherColumn]
        public decimal Amount { get; set; }


        [Description("每股收益")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal EPS { get; set; }

        [Description("净利润(亿)")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal NetProfit { get; set; }

        [Description("净利润同比(%)")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal ProfitGrewPer { get; set; }

        [Description("市盈率(动)")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal PE { get; set; }

        [Description("资本回报率")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal ROIC { get; set; }


        [Description("每股净资产")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal BVPS { get; set; }

        [Description("总营收(亿)")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal TotalRevenue { get; set; }

        [Description("营收同比(%)")]
        [GatherColumn]
        [GridColumnIgnore]
        public decimal RevenueGrewPer { get; set; }


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


        [Description("报告日期")]
        public string ReportDate { get; set; }

        [Description("股价日期")]
        public string PriceDate { get; set; }

        [Description("置顶")]
        public int Top { get; set; }
    }
}
