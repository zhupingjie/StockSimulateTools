using StockSimulateCore.Config;
using StockSimulateCore.Entity;
using StockSimulateCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Service
{
    public class StockConfigService
    {
        public static void LoadGlobalConfig(RunningConfig rc)
        {
            MySQLDBUtil.Instance.InitDataBase();

            var configs = MySQLDBUtil.Instance.QueryAll<GlobalConfigEntity>();
            foreach (var config in configs)
            {
                var prepInfo = ObjectUtil.GetPropertyInfo(rc, config.Name);
                if (prepInfo == null) continue;

                var value = ObjectUtil.ToValue(config.Value, prepInfo.PropertyType);
                if (value != null)
                {
                    ObjectUtil.SetPropertyValue(rc, config.Name, value);
                }
            }
        }
    }
}
