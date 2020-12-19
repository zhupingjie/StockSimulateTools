using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Entity
{
    public class ProfitTargetEntity : BaseEntity
    {
        [Description("股票代码")]
         public string SECURITYCODE{get;set; } // "000002.SZ",

        [Description("报告类型")]
        public int REPORTTYPE { get; set; } //"1",

        [Description("指标类型")]
        public int REPORTDATETYPE { get; set; } //"0",

        [Description("其他类型")]
        public int TYPE{get;set; } // "4",

        [Description("报告期")]
        public string REPORTDATE{get;set; } // "2020/9/30 0:00:00",

        [Description("人民币")]
        public string CURRENCY{get;set; } // "人民币",

        [Description("营业总收入")]
        public decimal TOTALOPERATEREVE{get;set; } // "241491467190.53",

        [Description("->营业收入")]
        public decimal OPERATEREVE{get;set; } // "241491467190.53",

        [Description("营业总成本")]
        public decimal TOTALOPERATEEXP{get;set; } // "203753344602.32",

        [Description("->营业成本")]
        public decimal OPERATEEXP{get;set; } // "169195782461.98",

        [Description("->研发费用")]
        public decimal RDEXP{get;set; } // "394374362.44",

        [Description("->营业税金及附加")]
        public decimal OPERATETAX{get;set; } // "16520767645.77",

        [Description("->销售费用")]
        public decimal SALEEXP{get;set; } // "5803606153.27",

        [Description("->管理费用")]
        public decimal MANAGEEXP{get;set; } // "7041498839.31",

        [Description("->财务费用")]
        public decimal FINANCEEXP{get;set; } // "4797315139.55",
        
        [Description("->加:公允价值变动收益")]
        public decimal FVALUEINCOME{get;set; } // "228277.7",

        [Description("->投资收益")]
        public decimal INVESTINCOME{get;set; } // "4502245832.25",

        [Description("->其中:对联营企业和合营企业的投资收益")]
        public decimal INVESTJOINTINCOME{get;set; } // "2307807461.03",

        [Description("营业利润")]
        public decimal OPERATEPROFIT{get;set; } // "42153893183.32",

        [Description("->加:营业外收入")]
        public decimal NONOPERATEREVE{get;set; } // "672440748.24",

        [Description("->减:营业外支出")]
        public decimal NONOPERATEEXP{get;set; } // "988337023.3",

        [Description("利润总额")]
        public decimal SUMPROFIT{get;set; } // "41837996908.26",

        [Description("->减:所得税费用")]
        public decimal INCOMETAX{get;set; } // "11766453990.22",

        [Description("净利润")]
        public decimal NETPROFIT{get;set; } // "30071542918.04",


        [Description("->其中:归属于母公司股东的净利润")]
        public decimal PARENTNETPROFIT{get;set; } // "19862827129.73",

        [Description("->少数股东损益")]
        public decimal MINORITYINCOME { get; set; }//"10208715788.31

        [Description("->扣除非经常性损益后的净利润")]
        public decimal KCFJCXSYJLR { get; set; } // "18889318112.36",


        [Description("->基本每股收益")]
        public decimal BASICEPS{get;set; } // "1.741",

        [Description("->稀释每股收益")]
        public decimal DILUTEDEPS{get;set; } // "1.741",

        [Description("其他综合收益")]
        public decimal OTHERCINCOME{get;set; } // "-144771208.33",

        [Description("->归属于母公司股东的其他综合收益")]
        public decimal PARENTOTHERCINCOME{get;set; } // "-123339056.11",

        [Description("->归属于少数股东的其他综合收益")]
        public decimal MINORITYOTHERCINCOME{get;set; } // "-21432152.22",

        [Description("综合收益总额")]
        public decimal SUMCINCOME{get;set; } // "29926771709.71",

        [Description("->归属于母公司所有者的综合收益总额")]
        public decimal PARENTCINCOME{get;set; } // "19739488073.62",

        [Description("->归属于少数股东的综合收益总额")]
        public decimal MINORITYCINCOME{get;set;} // "10187283636.09"
    }
}
