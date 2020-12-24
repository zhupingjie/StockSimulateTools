using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateNetCore.Entity
{
    public class BalanceTargetEntity : BaseEntity
    {
        [Description("股票代码")]
        public string SECURITYCODE { get; set; }

        [Description("报告类型")]
        public int REPORTTYPE { get; set; } //"1",

        [Description("指标类型")]
        public int REPORTDATETYPE { get; set; } //"0",

        [Description("其他类型")]
        public int TYPE { get; set; } //"13",

        [Description("报告期")]
        public string REPORTDATE { get; set; } //"2020/9/30 0:00:00",

        [Description("人民币")]
        public string CURRENCY { get; set; } //"人民币",


        [Description("->货币资金")]
        public decimal MONETARYFUND { get; set; }//": "173050032088.35",


        [Description("->应收票据及应收账款")]
        public decimal ACCOUNTBILLREC { get; set; }//": "2700534486.19",


        [Description("->其中:应收票据")]
        public decimal BILLREC { get; set; }//": "21815870.9",

        [Description("->应收账款")]
        public decimal ACCOUNTREC { get; set; }//": "2678718615.29",

        [Description("->预付款项")]
        public decimal ADVANCEPAY { get; set; }//": "71528940927.72",

        [Description("->其他应收款合计")]
        public decimal TOTAL_OTHER_RECE { get; set; }//": "268580832349.92",

        [Description("->其中:应收利息")]
        public decimal INTERESTREC { get; set; }//": "19029755.06",

        [Description("->应收股利")]
        public decimal DIVIDENDREC { get; set; }//": "19422475.8",

        [Description("->其他应收款")]
        public decimal OTHERREC { get; set; }//": "268542380119.06",

        [Description("->存货")]
        public decimal INVENTORY { get; set; }//": "955712807568.24",

        [Description("->其他流动资产")]
        public decimal OTHERLASSET { get; set; }//": "24335454610.78",

        [Description("流动资产合计")]
        public decimal SUMLASSET { get; set; }//": "1503850304031.17",

        [Description("->长期股权投资")]
        public decimal LTEQUITYINV { get; set; }//": "134633748565.48",

        [Description("->投资性房地产")]
        public decimal ESTATEINVEST { get; set; }//": "80703896860.62",

        [Description("->固定资产")]
        public decimal FIXEDASSET { get; set; }//": "12691995618.35",

        [Description("->在建工程")]
        public decimal CONSTRUCTIONPROGRESS { get; set; }//": "2047764911.09",

        [Description("->无形资产")]
        public decimal INTANGIBLEASSET { get; set; }//": "5220902092.22",

        [Description("->商誉")]
        public decimal GOODWILL { get; set; }//": "215252976.06",

        [Description("->长期待摊费用")]
        public decimal LTDEFERASSET { get; set; }//": "8825648614.69",

        [Description("->递延所得税资产")]
        public decimal DEFERINCOMETAXASSET { get; set; }//": "27270376854.34",

        [Description("->其他非流动资产")]
        public decimal OTHERNONLASSET { get; set; }//": "14386736913.16",

        [Description("非流动资产合计")]
        public decimal SUMNONLASSET { get; set; }//": "310615437910.55",

        [Description("资产总计")]
        public decimal SUMASSET { get; set; }//": "1814465741941.72",

        [Description("->短期借款")]
        public decimal STBORROW { get; set; }//": "19613829435.11",

        [Description("->应付票据及应付账款")]
        public decimal ACCOUNTBILLPAY { get; set; }//": "273177430087.02",

        [Description("->其中:应付票据")]
        public decimal BILLPAY { get; set; }//": "566201077.46",

        [Description("->应付账款")]
        public decimal ACCOUNTPAY { get; set; }//": "272611229009.56",

        [Description("-> 预收款项")]
        public decimal ADVANCERECEIVE { get; set; }//": "837181549.13",

        [Description("->应付职工薪酬")]
        public decimal SALARYPAY { get; set; }//": "5038348222.59",

        [Description("->应交税费")]
        public decimal TAXPAY { get; set; }//": "22991360729.04",

        [Description("->其他应付款合计")]
        public decimal TOTAL_OTHER_PAYABLE { get; set; }//": "232603463344.19",

        [Description("->其中:应付股利")]
        public decimal DIVIDENDPAY { get; set; }//": "528930089.31",

        [Description("->其他应付款")]
        public decimal OTHERPAY { get; set; }//": "232074533254.88",

        [Description("->一年内到期的非流动负债")]
        public decimal NONLLIABONEYEAR { get; set; }//": "68311597275.03",

        [Description("->其他流动负债")]
        public decimal OTHERLLIAB { get; set; }//": "54339665094.21",

        [Description("流动负债合计")]
        public decimal SUMLLIAB { get; set; }//": "1317539097359.36",

        [Description("->长期借款")]
        public decimal LTBORROW { get; set; }//": "121578169647.52",

        [Description("->应付债券")]
        public decimal BONDPAY { get; set; }//": "49842405556.23",

        [Description("->预计负债")]
        public decimal ANTICIPATELIAB { get; set; }//": "173299251.27",

        [Description("->递延所得税负债")]
        public decimal DEFERINCOMETAXLIAB { get; set; }//": "267396534.89",

        [Description("->其他非流动负债")]
        public decimal OTHERNONLLIAB { get; set; }//": "1049658485.4",

        [Description("非流动负债合计")]
        public decimal SUMNONLLIAB { get; set; }//": "194284232259.94",

        [Description("负债合计")]
        public decimal SUMLIAB { get; set; }//": "1511823329619.3",

        [Description("->实收资本")]
        public decimal SHARECAPITAL { get; set; }//": "11617732201",

        [Description("->资本公积")]
        public decimal CAPITALRESERVE { get; set; }//": "18756659496.89",

        [Description("->盈余公积")]
        public decimal SURPLUSRESERVE { get; set; }//": "70826254100.68",

        [Description("->未分配利润")]
        public decimal RETAINEDEARNING { get; set; }//": "103404124622.45",

        [Description("归属于母公司股东权益合计")]
        public decimal SUMPARENTEQUITY { get; set; }//": "202675004733.29",

        [Description("->少数股东权益")]
        public decimal MINORITYEQUITY { get; set; }//": "99967407589.13",

        [Description("股东权益合计")]
        public decimal SUMSHEQUITY { get; set; }//": "302642412322.42",

        [Description("负债和股东权益合计")]
        public decimal SUMLIABSHEQUITY { get; set; }//": "1814465741941.72",
    }
}
