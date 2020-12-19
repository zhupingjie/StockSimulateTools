using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Entity
{
    public class MainTargetEntity : BaseEntity
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        [Description("股票代码")]
        public string StockCode { get; set; } 
        /// <summary>
        /// 指标周期类型
        /// 0=报告期
        /// 1=年度
        /// 2=季度
        /// </summary>
        [Description("指标类型")]
        [GridColumnIgnore]
        public int Rtype { get; set; }
        [Description("指标类型")]
        [NotMapped]
        public string RtypeText
        {
            get
            {
                return Rtype == 0 ? "报告期" : Rtype == 1 ? "年度" : Rtype == 2 ? "季度" : "";
            }
        }

        /// <summary>
        /// 报告期
        /// </summary>
        [Description("报告期")]
        public string Date { get; set; } //: "2019-12-31"

        /// <summary>
        /// 基本每股收益(元)
        /// </summary>
        [Description("基本每股收益(元)")]
        public decimal Jbmgsy { get; set; } //: "3.4700"
        /// <summary>
        /// 扣非每股收益(元)
        /// </summary>
        [Description("扣非每股收益(元)")]
        public decimal Kfmgsy { get; set; } //: "3.4200"
        /// <summary>
        /// 稀释每股收益(元)
        /// </summary>
        [Description("稀释每股收益(元)")]
        public decimal Xsmgsy { get; set; } //: "3.4700"

        /// <summary>
        /// 每股净资产(元)
        /// </summary>
        [Description("每股净资产(元)")]
        public decimal Mgjzc { get; set; } //: "16.6400"
        /// <summary>
        /// 每股公积金(元)
        /// </summary>
        [Description("每股公积金(元)")]
        public decimal Mggjj { get; set; } //: "1.0958"

        /// <summary>
        /// 每股未分配利润(元)
        /// </summary>
        [Description("每股未分配利润(元)")]
        public decimal Mgwfply { get; set; } //: "8.4366"
        /// <summary>
        /// 每股经营现金流(元)
        /// </summary>
        [Description("每股经营现金流(元)")]
        public decimal Mgjyxjl { get; set; } //: "4.0423"


        /// <summary>
        /// 营业总收入(亿元)
        /// </summary>
        [Description("营业总收入(亿元")]
        public decimal Yyzsr { get; set; } //: "3679亿"

        /// <summary>
        /// 毛利润(亿元)
        /// </summary>
        [Description("毛利润(亿元)")]
        public decimal Mlr { get; set; } //: "1333亿"
        /// <summary>
        /// 归属净利润(亿元)
        /// </summary>
        [Description("归属净利润(亿元)")]
        public decimal Gsjlr { get; set; } //: "389亿"
        /// <summary>
        /// 扣非净利润(亿元)
        /// </summary>
        [Description("扣非净利润(亿元)")]
        public decimal Kfjlr { get; set; } //: "383亿"
        /// <summary>
        /// 营业总收入同比增长(%)
        /// </summary>
        [Description("营业总收入同比增长(%)")]
        public decimal Yyzsrtbzz { get; set; } //: "23.59"
        /// <summary>
        /// 归属净利润同比增长(%)
        /// </summary>
        [Description("归属净利润同比增长(%)")]
        public decimal Gsjlrtbzz { get; set; } //: "15.10"

        /// <summary>
        /// 扣非净利润同比增长(%)
        /// </summary>
        [Description("扣非净利润同比增长(%)")]
        public decimal Kfjlrtbzz { get; set; } //: "14.41"

        /// <summary>
        /// 营业总收入滚动环比增长(%)
        /// </summary>
        [Description("营业总收入滚动环比增长(%)")]
        public decimal Yyzsrgdhbzz { get; set; } //: "6.46"
        /// <summary>
        /// 归属净利润滚动环比增长(%)
        /// </summary>
        [Description("归属净利润滚动环比增长(%)")]
        public decimal Gsjlrgdhbzz { get; set; } //: "2.22"

        /// <summary>
        /// 扣非净利润滚动环比增长(%)
        /// </summary>
        [Description("扣非净利润滚动环比增长(%)")]
        public decimal Kfjlrgdhbzz { get; set; } //: "1.58"

        /// <summary>
        /// 加权净资产收益率(%)
        /// </summary>
        [Description("加权净资产收益率(%)")]
        public decimal Jqjzcsyl { get; set; } //: "22.47"
        /// <summary>
        /// 摊薄净资产收益率(%)
        /// </summary>
        [Description("摊薄净资产收益率(%)")]
        public decimal Tbjzcsyl { get; set; } //: "20.67"
        /// <summary>
        /// 摊薄总资产收益率(%)
        /// </summary>
        [Description("摊薄总资产收益率(%)")]
        public decimal Tbzzcsyl { get; set; } //: "3.38"


        /// <summary>
        /// 毛利率(%)
        /// </summary>
        [Description("毛利率(%)")]
        public decimal Mll { get; set; } //: "36.25"
        /// <summary>
        /// 净利率(%)
        /// </summary>
        [Description("净利率(%)")]
        public decimal Jll { get; set; } //: "14.99"

        /// <summary>
        /// 实际税率(%)
        /// </summary>
        [Description("实际税率(%)")]
        public decimal Sjsl { get; set; } //: "27.97"


        /// <summary>
        /// 预收款/营业收入
        /// </summary>
        [Description("预收款/营业收入")]
        public decimal Yskyysr { get; set; } //: "0.00"
        /// <summary>
        /// 销售现金流/营业收入
        /// </summary>
        [Description("销售现金流/营业收入")]
        public decimal Xsxjlyysr { get; set; } //: "1.18"

        /// <summary>
        /// 经营现金流/营业收入
        /// </summary>
        [Description("经营现金流/营业收入")]
        public decimal Jyxjlyysr { get; set; } //: "0.12"



        /// <summary>
        /// 总资产周转率(次)
        /// </summary>
        [Description("总资产周转率(次)")]
        public decimal Zzczzy { get; set; } //: "0.23"

        /// <summary>
        /// 应收账款周转天数(天)
        /// </summary>
        [Description("应收账款周转天数(天)")]
        public decimal Yszkzzts { get; set; } //: "1.75"
        /// <summary>
        /// 存货周转天数(天)
        /// </summary>
        [Description("存货周转天数(天)")]
        public decimal Chzzts { get; set; }//: "1264.20"

        /// <summary>
        /// 资产负债率(%)
        /// </summary>
        [Description("资产负债率(%)")]
        public decimal Zcfzl { get; set; } //: "84.36"
        /// <summary>
        /// 流动负债/总负债(%)
        /// </summary>
        [Description("流动负债/总负债(%)")]
        public decimal Ldzczfz { get; set; } //: "87.20"
        /// <summary>
        /// 流动比率
        /// </summary>
        [Description("流动比率")]
        public decimal Ldbl { get; set; } //: "1.13"
        /// <summary>
        /// 速动比率
        /// </summary>
        [Description("速动比率")]
        public decimal Sdbl { get; set; } //: "0.43"
    }
}
