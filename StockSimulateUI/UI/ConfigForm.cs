using StockSimulateCore.Config;
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
        public ConfigForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            RunningConfig.Instance.LoadStockDataInterval = ObjectUtil.ToValue<int>(this.txtLoadStockDataInterval.Text, 0);
            RunningConfig.Instance.GatherStockPriceInterval = ObjectUtil.ToValue<int>(this.txtGatherStockPriceInterval.Text, 0);
            RunningConfig.Instance.GatherStockMainTargetInterval = ObjectUtil.ToValue<int>(this.txtGatherStockMainTargetInterval.Text, 0);
            RunningConfig.Instance.RemindStockStrategyInterval = ObjectUtil.ToValue<int>(this.txtRemindStockStrategyInterval.Text, 0);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            this.txtLoadStockDataInterval.Text = $"{RunningConfig.Instance.LoadStockDataInterval}";
            this.txtGatherStockPriceInterval.Text = $"{RunningConfig.Instance.GatherStockPriceInterval}";
            this.txtGatherStockMainTargetInterval.Text = $"{RunningConfig.Instance.GatherStockMainTargetInterval}";
            this.txtRemindStockStrategyInterval.Text = $"{RunningConfig.Instance.RemindStockStrategyInterval}";
        }
    }
}
