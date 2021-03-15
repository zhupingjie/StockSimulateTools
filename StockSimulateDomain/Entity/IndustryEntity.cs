using StockSimulateDomain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StockSimulateDomain.Entity
{
    [Description("行业板块")]
    public class IndustryEntity : BaseEntity
    {

        [Description("行业名称")]
        public string Name { get; set; }
    }
}
