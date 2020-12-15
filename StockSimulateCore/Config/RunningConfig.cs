using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Config
{
    public class RunningConfig
    {
        private static readonly RunningConfig instance = new RunningConfig();
        public static RunningConfig Instance
        {
            get
            {
                return instance;
            }
        }

        public int GatherStockPriceInterval { get; set; } = 30;

        public int GatherStockMainTargetInterval { get; set; } = 12 * 60 * 60;

        public int RemindStockStrategyInterval { get; set; } = 30;

        public int RemindStockPriceFloatPer { get; set; } = 2;
    }
}
