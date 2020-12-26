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
    public class RemindEntity : BaseEntity
    {
        [Description("股票代码")]
        public string StockCode { get; set; }

        /// <summary>
        /// 0:涨跌幅,1:股价涨跌,2:买卖策略
        /// </summary>
        [Description("类型")]
        [GridColumnIgnore]
        public int RType { get; set; }
        [Description("类型")]
        [NotMapped]
        public string RTypeText
        {
            get
            {
                return RType == 0 ? "涨跌幅" : RType == 1 ? "上涨" : RType == 2 ? "下跌" : RType == 3 ? "突破" : RType == 4 ? "跌破" : RType == 5 ? "反转" : RType == 6 ? "逆转" : RType == 8 ? "买点" : RType == 9 ? "卖点" : "";
            }
        }

        [Description("目标")]
        public decimal Target { get; set; }

        [Description("股价上限")]
        public decimal MaxPrice { get; set; }

        [Description("股价下限")]
        public decimal MinPrice { get; set; }

        [Description("策略名称")]
        public string StrategyName { get; set; }

        [Description("策略买卖点")]
        public string StrategyTarget { get; set; }

        [Description("实际股价")]
        public decimal RemindPrice { get; set; }
        
        [Description("是否执行")]
        [GridColumnIgnore]
        public int Handled { get; set; }
        [Description("是否执行")]
        [NotMapped]
        public string HandledText
        {
            get
            {
                return Handled == 1 ? "✔" : "×";
            }
        }

        [Description("计划提醒时间")]
        public DateTime? PlanRemind { get; set; }

        [Description("最后提醒时间")]
        public DateTime? LastRemind { get; set; }


        [Description("提醒邮箱")]
        public string Email { get; set; }

        [Description("提醒QQ")]
        public string QQ { get; set; }
    }
}
