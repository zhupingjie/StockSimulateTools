using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Entity
{
    public class AccountEntity: BaseEntity
    {
        [Description("实体账户")]
        public int RealType { get; set; }

        [Description("交易账户")]
        public string Name { get; set; }

        [Description("总投资市值(元)")]
        public decimal Amount { get; set; }

        [Description("持有现金(元)")]
        public decimal Cash { get; set; }

        [Description("总市值(元)")]
        public decimal TotalAmount { get; set; }

        [Description("投入市值(元)")]
        public decimal BuyAmount { get; set; }

        [Description("持有市值(元)")]
        public decimal HoldAmount { get; set; }

        [Description("盈亏(元)")]
        public decimal Profit { get; set; }

        [Description("邮箱")]
        public string Email { get; set; } = "zhupj@foxmail.com";

        [Description("QQ")]
        public string QQ { get; set; } = "47426568";

        [Description("单股最大持仓(%)")]
        public decimal MaxPositionPer { get; set; }

        [Description("持有总仓位(%)")]
        public decimal HoldPositionPer { get; set; }
    }
}
