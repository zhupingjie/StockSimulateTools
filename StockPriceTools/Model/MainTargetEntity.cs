using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPriceTools.Model
{
    public class MainTargetEntity : BaseEntity
    {
        /// <summary>
        /// 指标周期类型
        /// 0=报告期
        /// 1=年度
        /// 2=季度
        /// </summary>
        [Description("指标周期类型")]
        public int rtype { get; set; }
        /// <summary>
        /// 报告期
        /// </summary>
        [Description("报告期")]
        public DateTime date { get; set; } //: "2019-12-31"
        /// <summary>
        /// 存货周转天数(天)
        /// </summary>
        [Description("存货周转天数(天)")]
        public decimal chzzts { get; set; }//: "1264.20"
        /// <summary>
        /// 归属净利润(亿元)
        /// </summary>
        [Description("归属净利润(亿元)")]
        public decimal gsjlr { get; set; } //: "389亿"
        /// <summary>
        /// 扣非净利润(亿元)
        /// </summary>
        [Description("扣非净利润(亿元)")]
        public decimal kfjlr { get; set; } //: "383亿"
        /// <summary>
        /// 归属净利润滚动环比增长(%)
        /// </summary>
        [Description("归属净利润滚动环比增长(%)")]
        public decimal gsjlrgdhbzz { get; set; } //: "2.22"
        /// <summary>
        /// 归属净利润同比增长(%)
        /// </summary>
        [Description("归属净利润同比增长(%)")]
        public decimal gsjlrtbzz { get; set; } //: "15.10"
        /// <summary>
        /// 基本每股收益(元)
        /// </summary>
        [Description("基本每股收益(元)")]
        public decimal jbmgsy { get; set; } //: "3.4700"
        /// <summary>
        /// 净利率(%)
        /// </summary>
        [Description("净利率(%)")]
        public decimal jll { get; set; } //: "14.99"
        /// <summary>
        /// 加权净资产收益率(%)
        /// </summary>
        [Description("加权净资产收益率(%)")]
        public decimal jqjzcsyl { get; set; } //: "22.47"
        /// <summary>
        /// 经营现金流/营业收入
        /// </summary>
        [Description("经营现金流/营业收入")]
        public decimal jyxjlyysr { get; set; } //: "0.12"
        /// <summary>
        /// 扣非净利润滚动环比增长(%)
        /// </summary>
        [Description("扣非净利润滚动环比增长(%)")]
        public decimal kfjlrgdhbzz { get; set; } //: "1.58"
        /// <summary>
        /// 扣非净利润同比增长(%)
        /// </summary>
        [Description("扣非净利润同比增长(%)")]
        public decimal kfjlrtbzz { get; set; } //: "14.41"
        /// <summary>
        /// 扣非每股收益(元)
        /// </summary>
        [Description("扣非每股收益(元)")]
        public decimal kfmgsy { get; set; } //: "3.4200"
        /// <summary>
        /// 流动比率
        /// </summary>
        [Description("流动比率")]
        public decimal ldbl { get; set; } //: "1.13"
        /// <summary>
        /// 流动负债/总负债(%)
        /// </summary>
        [Description("流动负债/总负债(%)")]
        public decimal ldzczfz { get; set; } //: "87.20"
        /// <summary>
        /// 每股公积金(元)
        /// </summary>
        [Description("每股公积金(元)")]
        public decimal mggjj { get; set; } //: "1.0958"
        /// <summary>
        /// 每股经营现金流(元)
        /// </summary>
        [Description("归属净利润")]
        public decimal mgjyxjl { get; set; } //: "4.0423"
        /// <summary>
        /// 每股净资产(元)
        /// </summary>
        [Description("每股净资产(元)")]
        public decimal mgjzc { get; set; } //: "16.6400"
        /// <summary>
        /// 每股未分配利润(元)
        /// </summary>
        [Description("每股未分配利润(元)")]
        public decimal mgwfply { get; set; } //: "8.4366"
        /// <summary>
        /// 毛利率(%)
        /// </summary>
        [Description("毛利率(%)")]
        public decimal mll { get; set; } //: "36.25"
        /// <summary>
        /// 毛利润(亿元)
        /// </summary>
        [Description("毛利润(亿元)")]
        public decimal mlr { get; set; } //: "1333亿"
        /// <summary>
        /// 速动比率
        /// </summary>
        [Description("速动比率")]
        public decimal sdbl { get; set; } //: "0.43"
        /// <summary>
        /// 实际税率(%)
        /// </summary>
        [Description("实际税率(%)")]
        public decimal sjsl { get; set; } //: "27.97"
        /// <summary>
        /// 摊薄净资产收益率(%)
        /// </summary>
        [Description("摊薄净资产收益率(%)")]
        public decimal tbjzcsyl { get; set; } //: "20.67"
        /// <summary>
        /// 摊薄总资产收益率(%)
        /// </summary>
        [Description("摊薄总资产收益率(%)")]
        public decimal tbzzcsyl { get; set; } //: "3.38"
        /// <summary>
        /// 稀释每股收益(元)
        /// </summary>
        [Description("稀释每股收益(元)")]
        public decimal xsmgsy { get; set; } //: "3.4700"
        /// <summary>
        /// 销售现金流/营业收入
        /// </summary>
        [Description("销售现金流/营业收入")]
        public decimal xsxjlyysr { get; set; } //: "1.18"
        /// <summary>
        /// 预收款/营业收入
        /// </summary>
        [Description("预收款/营业收入")]
        public decimal yskyysr { get; set; } //: "0.00"
        /// <summary>
        /// 应收账款周转天数(天)
        /// </summary>
        [Description("应收账款周转天数(天)")]
        public decimal yszkzzts { get; set; } //: "1.75"
        /// <summary>
        /// 营业总收入(亿元)
        /// </summary>
        [Description("营业总收入(亿元")]
        public decimal yyzsr { get; set; } //: "3679亿"
        /// <summary>
        /// 营业总收入滚动环比增长(%)
        /// </summary>
        [Description("营业总收入滚动环比增长(%)")]
        public decimal yyzsrgdhbzz { get; set; } //: "6.46"
        /// <summary>
        /// 营业总收入同比增长(%)
        /// </summary>
        [Description("营业总收入同比增长(%)")]
        public decimal yyzsrtbzz { get; set; } //: "23.59"
        /// <summary>
        /// 资产负债率(%)
        /// </summary>
        [Description("资产负债率(%)")]
        public decimal zcfzl { get; set; } //: "84.36"
        /// <summary>
        /// 总资产周转率(次)
        /// </summary>
        [Description("总资产周转率(次)")]
        public decimal zzczzy { get; set; } //: "0.23"
    }
}
