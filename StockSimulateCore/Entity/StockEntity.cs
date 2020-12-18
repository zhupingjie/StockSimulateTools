using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Entity
{
    public class StockEntity : BaseEntity
    {
        [Description("股票类型")]
        [GridColumnIgnore]
        public int Type { get; set; }

        [Description("股票类型")]
        [NotMapped]
        public string TypeText
        {
            get
            {
                return Type == 0 ? "沪深股" : Type == 1 ? "基金(ETF)" : "";
            }
        }

        [Description("关注")]
        [GridColumnIgnore]
        public int Foucs { get; set; }

        [Description("关注")]
        [NotMapped]
        public string FoucsText {
            get {
                return Foucs == 1 ? "✔" : Foucs == 0 ? "×" : "";
            }
        }

        [Description("股票代码")]
        public string Code { get; set; }

        [Description("股票名称")]
        public string Name { get; set; }

        [Description("股价")]
        public decimal Price { get; set; }

        [Description("浮动(%)")]
        public decimal UDPer { get; set; }

        [Description("目标价")]
        public decimal Target { get; set; }

        [Description("总股本(万股)")]
        public decimal Capital { get; set; }

        [Description("总市值(亿元)")]
        public decimal Amount { get; set; }

        [Description("市盈率(动)")]
        public decimal PE { get; set; }

        [Description("市盈率(TTM)")]
        public decimal TTM { get; set; }

        [Description("市净率")]
        public decimal PB { get; set; }

        [Description("市盈率增长率")]
        public decimal PEG { get; set; }

        [Description("净资产收益率")]
        public decimal ROE { get; set; }

        [Description("投入资本回报率")]
        public decimal ROIC { get; set; }

        [Description("每股收益(元)")]
        public decimal EPS { get; set; }

        [Description("每股净资产(元)")]
        public decimal BVPS { get; set; }

        [Description("总营收(亿元)")]
        public decimal TotalRevenue { get; set; }

        [Description("净利润(亿元)")]
        public decimal NetProfit { get; set; }

        [Description("营收同比(%)")]
        public decimal RevenueGrewPer { get; set; }

        [Description("净利润同比(%)")]
        public decimal ProfitGrewPer { get; set; }

        [Description("毛利率(%)")]
        public decimal GrossRate { get; set; }

        [Description("净利率(%)")]
        public decimal NetRate { get; set; }

        [Description("负债率(%)")]
        public decimal DebtRage { get; set; }

    }
}
