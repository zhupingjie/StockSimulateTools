using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Entity
{
    public class CashTargetEntity : BaseEntity
    {
        [Description("股票代码")]
        public string SECURITYCODE { get; set; }

        [Description("报告类型")]
        public int REPORTTYPE{get;set;} //"1",

        [Description("指标类型")]
        public int REPORTDATETYPE { get;set;} //"0",

        [Description("其他类型")]
        public int TYPE { get;set; } //"13",

        [Description("报告期")]
        public string REPORTDATE { get;set;} //"2020/9/30 0:00:00",

        [Description("人民币")]
        public string CURRENCY{get;set;} //"人民币",

        [Description("->销售商品、提供劳务收到的现金")]
        public decimal SALEGOODSSERVICEREC{get;set;} //"304532471869.22",

        [Description("->收到其他与经营活动有关的现金")]
        public decimal OTHEROPERATEREC { get; set; } //"20437136996.69",

        [Description("经营活动现金流入小计")]
        public decimal SUMOPERATEFLOWIN { get; set; } //"324969608865.91",

        [Description("->购买商品、接受劳务支付的现金")]
        public decimal BUYGOODSSERVICEPAY { get; set; } //"182233778605.42",

        [Description("->支付给职工以及为职工支付的现金")]
        public decimal EMPLOYEEPAY { get; set; } //"12684419708.42",

        [Description("->支付的各项税费")]
        public decimal TAXPAY { get; set; } //"48279029034.33",

        [Description("->支付其他与经营活动有关的现金")]
        public decimal OTHEROPERATEPAY { get; set; } //"48391512931.38",

        [Description("经营活动现金流出小计")]
        public decimal SUMOPERATEFLOWOUT { get; set; } //"291588740279.55",

        [Description("经营活动产生的现金流量净额")]
        public decimal NETOPERATECASHFLOW { get; set; } //"33380868586.36",

        [Description("->收回投资收到的现金")]
        public decimal DISPOSALINVREC { get; set; } //"938818261.82",

        [Description("->取得投资收益收到的现金")]
        public decimal INVINCOMEREC { get; set; } //"2378627395.98",

        [Description("->处置固定资产无形资产和其他长期资产收回的现金净额")]
        public decimal DISPFILASSETREC { get; set; } //"39412146.41",

        [Description("->处置子公司及其他营业单位收到的现金净额")]
        public decimal DISPSUBSIDIARYREC { get; set; } //"4118520034.49",

        [Description("->收到其他与投资活动有关的现金")]
        public decimal OTHERINVREC { get; set; } //"14403901371.29",

        [Description("投资活动现金流入小计")]
        public decimal SUMINVFLOWIN { get; set; } //"21879279209.99",

        [Description("->购建固定资产无形资产和其他长期资产支付的现金")]
        public decimal BUYFILASSETPAY { get; set; } //"3470608874.61",

        [Description("->投资支付的现金")]
        public decimal INVPAY { get; set; } //"9901209472.99",

        [Description("->取得子公司及其他营业单位支付的现金净额")]
        public decimal GETSUBSIDIARYPAY { get; set; } //"2115820247.24",

        [Description("->支付其他与投资活动有关的现金")]
        public decimal OTHERINVPAY { get; set; } //"202066843.83",

        [Description("投资活动现金流出小计")]
        public decimal SUMINVFLOWOUT { get; set; } //"15689705438.67",

        [Description("投资活动产生的现金流量净额")]
        public decimal NETINVCASHFLOW { get; set; } //"6189573771.32",

        [Description("->吸收投资收到的现金")]
        public decimal ACCEPTINVREC { get; set; } //"19606266779.03",

        [Description("->子公司吸收少数股东投资收到的现金")]
        public decimal SUBSIDIARYACCEPT { get; set; } //"12440971228.3",

        [Description("->取得借款收到的现金")]
        public decimal LOANREC { get; set; } //"54712411972.63",

        [Description("->发行债券收到的现金")]
        public decimal ISSUEBONDREC { get; set; } //"6986000000",

        [Description("->收到其他与筹资活动有关的现金")]
        public decimal OTHERFINAREC { get; set; } //"",

        [Description("筹资活动现金流入小计")]
        public decimal SUMFINAFLOWIN { get; set; } //"81304678751.66",

        [Description("->偿还债务支付的现金")]
        public decimal REPAYDEBTPAY { get; set; } //"61384019116.62",

        [Description("->分配股利、利润或偿付利息支付的现金")]
        public decimal DIVIPROFITORINTPAY { get; set; } //"26179143946.4",

        [Description("->子公司支付给少数股东的股利、利润")]
        public decimal SUBSIDIARYPAY { get; set; } //"3339502437.62",

        [Description("->支付其他与筹资活动有关的现金")]
        public decimal OTHERFINAPAY { get; set; } //"19631208509.84",

        [Description("筹资活动现金流出小计")]
        public decimal SUMFINAFLOWOUT { get; set; } //"113080522835.64",

        [Description("筹资活动产生的现金流量净额")]
        public decimal NETFINACASHFLOW { get; set; } //"-31775844083.98",

        [Description("汇率变动对现金及现金等价物的影响")]
        public decimal EFFECTEXCHANGERATE { get; set; } //"-382832497.16",

        [Description("现金及现金等价物净增加额")]
        public decimal NICASHEQUI { get; set; } //"7411765776.54",

        [Description("->加:期初现金及现金等价物余额")]
        public decimal CASHEQUIBEGINNING { get; set; } //"159738651471.96",

        [Description("期末现金及现金等价物余额")]
        public decimal CASHEQUIENDING { get;set;} //"167150417248.5",
    }
}
