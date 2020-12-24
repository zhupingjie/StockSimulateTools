using StockSimulateNetCore.Config;
using StockSimulateNetCore.Data;
using StockSimulateNetCore.Entity;
using StockSimulateNetCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateNetCore.Service
{
    public class StockConfigService: ServiceBase
    {
        public StockConfigService(RunningConfig _rc) : base(_rc)
        {

        }

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
