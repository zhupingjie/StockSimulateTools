﻿using StockSimulateDomain.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateDomain.Data
{
    public class BaseEntity : IBaseEntity
    {
        [Description("序号")]
        public int ID { get; set; }

        [Description("最后更新时间")]
        [GridColumnIgnore]
        public DateTime LastDate { get; set; } = DateTime.Now;
    }
}
