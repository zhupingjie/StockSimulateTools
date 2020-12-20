using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Entity
{
    public class BaseEntity : IBaseEntity
    {
        public int ID { get; set; }

        [Description("最后更新时间")]
        [GridColumnIgnore]
        public DateTime LastDate { get; set; } = DateTime.Now;
    }
}
