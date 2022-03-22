using StockSimulateDomain.Entity;
using StockSimulateCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSimulateCore.Config;
using StockSimulateCore.Data;

namespace StockSimulateService.Service
{
    public class StockConfigService
    {
        public static void LoadGlobalConfig(RunningConfig rc)
        {
            var configs = Repository.Instance.QueryAll<GlobalConfigEntity>("Value!=''", withNoLock: true);
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

        public static void SaveGlobalConfig(string key, object value)
        {
            var config = Repository.Instance.QueryFirst<GlobalConfigEntity>($"Name='{key}'");
            if(config != null)
            {
                config.Value = $"{value}";
                Repository.Instance.Update<GlobalConfigEntity>(config);
            }
        }
    }
}
