using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Model
{
    public class BaseEntity : IBaseEntity
    {
        public int ID { get; set; }

        [Description("更新时间")]
        public DateTime LastDate { get; set; } = DateTime.Now;
    }
}
