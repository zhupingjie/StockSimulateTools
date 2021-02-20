using StockSimulateService.Service;
using StockSimulateDomain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StockSimulateCore.Utils;
using StockSimulateCore.Config;
using StockSimulateCore.Data;

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
            RunningConfig.Instance.DebugMode = this.txtDebugMode.Checked;
            RunningConfig.Instance.RemindNoticeByEmail = this.txtRemindNoticeByEmail.Checked;
            RunningConfig.Instance.RemindNoticeByMessage = this.txtRemindNoticeByMessage.Checked;
            RunningConfig.Instance.GatherStockPriceInterval = ObjectUtil.ToValue<int>(this.txtGatherStockPriceInterval.Text, 0);
            RunningConfig.Instance.GatherStockFinanceReportInterval = ObjectUtil.ToValue<int>(this.txtGatherStockMainTargetInterval.Text, 0);
            RunningConfig.Instance.RemindStockStrategyInterval = ObjectUtil.ToValue<int>(this.txtRemindStockStrategyInterval.Text, 0);
            RunningConfig.Instance.RemindStockPriceFloatPer = ObjectUtil.ToValue<decimal>(this.txtRemindStockPriceFloatPer.Text, 0);
            RunningConfig.Instance.UpdateAccountStockProfitInterval = ObjectUtil.ToValue<int>(this.txtUpdateAccountStockProfitInterval.Text, 0);
            RunningConfig.Instance.UpdateStockAssistTargetInterval = ObjectUtil.ToValue<int>(this.txtUpdateStockAssistTargetInterval.Text, 0);
            RunningConfig.Instance.LoadMessageInterval = ObjectUtil.ToValue<int>(this.txtLoadMessageInterval.Text, 0);
            RunningConfig.Instance.LoadGlobalConfigInterval = ObjectUtil.ToValue<int>(this.txtLoadGlobalConfigInterval.Text, 0);
            RunningConfig.Instance.KeepStockAssistTargetDays = ObjectUtil.ToValue<int>(this.txtKeepStockAssistTargetDays.Text, 0);
            RunningConfig.Instance.CurrentAccountName = this.txtAccount.Text;

            var configs = Repository.Instance.QueryAll<GlobalConfigEntity>();
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
                    Repository.Instance.Insert<GlobalConfigEntity>(config);
                }
                else
                {
                    config.Value = $"{item.Value}";
                    Repository.Instance.Update<GlobalConfigEntity>(config);
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            var accounts = Repository.Instance.QueryAll<AccountEntity>();
            if (accounts.Length == 0) return;

            this.txtAccount.Items.Clear();
            this.txtAccount.Items.AddRange(accounts.Select(c => c.Name).ToArray());

            this.txtGatherStockPriceInterval.Text = $"{RunningConfig.Instance.GatherStockPriceInterval}";
            this.txtGatherStockMainTargetInterval.Text = $"{RunningConfig.Instance.GatherStockFinanceReportInterval}";
            this.txtUpdateStockAssistTargetInterval.Text = $"{RunningConfig.Instance.UpdateStockAssistTargetInterval}";
            this.txtRemindStockStrategyInterval.Text = $"{RunningConfig.Instance.RemindStockStrategyInterval}";
            this.txtRemindStockPriceFloatPer.Text = $"{RunningConfig.Instance.RemindStockPriceFloatPer}";
            this.txtUpdateAccountStockProfitInterval.Text = $"{RunningConfig.Instance.UpdateAccountStockProfitInterval}";
            this.txtLoadMessageInterval.Text = $"{RunningConfig.Instance.LoadMessageInterval}";
            this.txtLoadGlobalConfigInterval.Text = $"{RunningConfig.Instance.LoadGlobalConfigInterval}";
            this.txtDebugMode.Checked = RunningConfig.Instance.DebugMode;
            this.txtRemindNoticeByMessage.Checked = RunningConfig.Instance.RemindNoticeByMessage;
            this.txtRemindNoticeByEmail.Checked = RunningConfig.Instance.RemindNoticeByEmail;
            this.txtKeepStockAssistTargetDays.Text = $"{RunningConfig.Instance.KeepStockAssistTargetDays}";
            this.txtAccount.Text = RunningConfig.Instance.CurrentAccountName;
        }
    }
}
