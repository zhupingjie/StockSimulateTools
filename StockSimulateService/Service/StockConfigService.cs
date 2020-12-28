using StockSimulateDomain.Entity;
using StockSimulateService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockSimulateService.Config;

namespace StockSimulateService.Service
{
    public class StockConfigService
    {
        public static void LoadGlobalConfig(RunningConfig rc)
        {
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

        public static void LastUpdateTime()
        {
            var config = MySQLDBUtil.Instance.QueryFirst<GlobalConfigEntity>($"Name='LastServiceUpdateTime'");
            if(config == null)
            {
                config = new GlobalConfigEntity()
                {
                    Name = "LastServiceUpdateTime",
                    Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                MySQLDBUtil.Instance.Insert<GlobalConfigEntity>(config);
            }
            else
            {
                config.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                MySQLDBUtil.Instance.Update<GlobalConfigEntity>(config);
            }
        }
    }
}
