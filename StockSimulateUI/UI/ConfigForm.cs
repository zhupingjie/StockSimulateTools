using StockSimulateCore.Config;
using StockSimulateDomain.Entity;
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
using StockSimulateNetCore.Utils;
using StockSimulateDomain.Utils;

namespace StockSimulateUI.UI
{
    public partial class ConfigForm : Form
    {
        private MySQLDBUtil Repository = MySQLDBUtil.Instance;
        public ConfigForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            RunningConfig.Instance.DebugMode = this.txtDebugMode.Checked;
            RunningConfig.Instance.GatherStockPriceInterval = ObjectUtil.ToValue<int>(this.txtGatherStockPriceInterval.Text, 0);
            RunningConfig.Instance.GatherStockFinanceTargetInterval = ObjectUtil.ToValue<int>(this.txtGatherStockMainTargetInterval.Text, 0);
            RunningConfig.Instance.GatherStockReportInterval = ObjectUtil.ToValue<int>(this.txtGatherStockReportInterval.Text, 0);
            RunningConfig.Instance.RemindStockStrategyInterval = ObjectUtil.ToValue<int>(this.txtRemindStockStrategyInterval.Text, 0);
            RunningConfig.Instance.RemindStockPriceFloatPer = ObjectUtil.ToValue<decimal>(this.txtRemindStockPriceFloatPer.Text, 0);
            RunningConfig.Instance.UpdateAccountStockProfitInterval = ObjectUtil.ToValue<int>(this.txtUpdateAccountStockProfitInterval.Text, 0);

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
            this.txtGatherStockMainTargetInterval.Text = $"{RunningConfig.Instance.GatherStockFinanceTargetInterval}";
            this.txtGatherStockReportInterval.Text = $"{RunningConfig.Instance.GatherStockReportInterval}";
            this.txtRemindStockStrategyInterval.Text = $"{RunningConfig.Instance.RemindStockStrategyInterval}";
            this.txtRemindStockPriceFloatPer.Text = $"{RunningConfig.Instance.RemindStockPriceFloatPer}";
            this.txtUpdateAccountStockProfitInterval.Text = $"{RunningConfig.Instance.UpdateAccountStockProfitInterval}";
            this.txtDebugMode.Checked = RunningConfig.Instance.DebugMode;
        }
    }
}
