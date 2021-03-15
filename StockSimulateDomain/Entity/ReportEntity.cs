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
    public class ReportEntity : BaseEntity
    {
        /// <summary>
        /// 0：股票研报
        /// 1:  财务报表
        /// 9:  行业研报
        /// </summary>
        [Description("研报类型")]
        [GridColumnIgnore]
        public int ReportType { get; set; }

        [Description("股票代码")]
        public string StockCode { get; set; }

        [Description("行业名称")]
        public string IndustryName { get; set; }

        [Description("发布日期")]
        public string PublishDate { get; set; }

        [Description("研报标题")]
        public string Title { get; set; }

        [Description("机构名称")]
        public string OrgNam { get; set; }

        [Description("今年PE")]
        public decimal ThisYearPE { get; set; }

        [Description("今年每股盈利")]
        public decimal ThisYearEPS { get; set; }

        [Description("明年PE")]
        public decimal NextYearPE { get; set; }

        [Description("明年每股盈利")]
        public decimal NextYearEPS { get; set; }

        [Description("后年PE")]
        public decimal NextTwoYearPE { get; set; }

        [Description("后年每股盈利")]
        public decimal NextTwoYearEPS { get; set; }

        [Description("编号")]
        public string PdfCode { get; set; }

        [Description("地址")]
        [DBNotMapped]
        [GridColumnIgnore]
        public string PdfUrl
        {
            get
            {
                return $"https://pdf.dfcfw.com/pdf/H{(ReportType == 0 ? 3 : ReportType == 1 ? 2 : 3)}_{PdfCode}_1.pdf";
            }
        }

    }
}
