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
        [Description("股票代码")]
        public string StockCode { get; set; }

        [Description("行业名称")]
        public string IndustryName { get; set; }

        [Description("发布日期")]
        public string PublishDate { get; set; }

        [Description("研报标题")]
        public string Title { get; set; }

        [Description("机构")]
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
                return $"https://pdf.dfcfw.com/pdf/H3_{PdfCode}_1.pdf";
            }
        }

    }
}
