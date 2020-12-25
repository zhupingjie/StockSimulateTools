using StockSimulateDomain.Entity;
using StockSimulateDomain.Interface;
using StockSimulateDomain.Utils;
using StockSimulateNetCore.Config;
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
        public static void LoadGlobalConfig(RunningConfig rc)
        {
            LogUtil.Debug($"初始化数据库...");
            MySQLDBUtil.Instance.InitDataBase();

            LogUtil.Debug($"初始化系统配置...");
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
            LogUtil.Debug($"初始化系统配置完成...");
        }
    }
}
