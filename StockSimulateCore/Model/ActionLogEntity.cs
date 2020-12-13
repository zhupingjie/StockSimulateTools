using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Model
{
    public class ActionLogEntity : BaseEntity
    {
        [Description("操作项")]
        public string ActionName { get; set; }

        [Description("操作时间")]
        public string ActionTime { get; set; }

        [Description("操作状态")]
        public bool ActionStatus { get; set; }

        [Description("错误信息")]
        public string ActionError { get; set; }

        [Description("提醒对象")]
        public string ActionRemind { get; set; }
    }
}
