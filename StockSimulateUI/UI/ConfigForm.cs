using StockSimulateCore.Config;
using StockSimulateCore.Model;
using StockSimulateCore.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockSimulateUI.UI
{
    public partial class ConfigForm : Form
    {
        private SQLiteDBUtil Repository = SQLiteDBUtil.Instance;
        public ConfigForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            RunningConfig.Instance.GatherStockPriceInterval = ObjectUtil.ToValue<int>(this.txtGatherStockPriceInterval.Text, 0);
            RunningConfig.Instance.GatherStockMainTargetInterval = ObjectUtil.ToValue<int>(this.txtGatherStockMainTargetInterval.Text, 0);
            RunningConfig.Instance.RemindStockStrategyInterval = ObjectUtil.ToValue<int>(this.txtRemindStockStrategyInterval.Text, 0);
            RunningConfig.Instance.RemindStockPriceFloatPer = ObjectUtil.ToValue<int>(this.txtRemindStockPriceFloatPer.Text, 0);

            var configs = Repository.QueryAll<GlobalConfigEntity>();
            var dic = ObjectUtil.GetPropertyValues(RunningConfig.Instance, true);
            foreach(var item in dic)
            {
                var config = configs.FirstOrDefault(c => c.Name == item.Key);
                if(config == null)
                {
                    config = new GlobalConfigEntity()
                    {
                        Name = item.Key,
                        Value = $"{item.Value}"
                    };
                    Repository.Insert<GlobalConfigEntity>(config);
                }
                else
                {
                    config.Value = $"{item.Value}";
                    Repository.Update<GlobalConfigEntity>(config);
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            this.txtGatherStockPriceInterval.Text = $"{RunningConfig.Instance.GatherStockPriceInterval}";
            this.txtGatherStockMainTargetInterval.Text = $"{RunningConfig.Instance.GatherStockMainTargetInterval}";
            this.txtRemindStockStrategyInterval.Text = $"{RunningConfig.Instance.RemindStockStrategyInterval}";
            this.txtRemindStockPriceFloatPer.Text = $"{RunningConfig.Instance.RemindStockPriceFloatPer}";
        }
    }
}
