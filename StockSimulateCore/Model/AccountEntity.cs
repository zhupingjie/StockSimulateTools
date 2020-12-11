using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Model
{
    public class AccountEntity: BaseEntity
    {
        [Description("交易者")]
        public string Name { get; set; }

        [Description("总投资市值(元)")]
        public decimal Amount { get; set; }

        [Description("持有市值(元)")]
        public decimal HoldAmount { get; set; }

        [Description("盈亏(元)")]
        public decimal Profit { get; set; }
    }
}
