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
using StockSimulateDomain.Model;
using StockSimulateCore.Config;
using StockSimulateCore.Data;

namespace StockSimulateUI.UI
{
    public partial class RemindForm : Form
    {
        public string StockCode { get; set; }

        public RemindForm()
        {
            InitializeComponent();
        }

        private void SetRemindForm_Load(object sender, EventArgs e)
        {
            var accounts = Repository.Instance.QueryAll<AccountEntity>();
            if (accounts.Length == 0) return;

            this.txtAccount.Items.Clear();
            this.txtAccount.Items.AddRange(accounts.Select(c => c.Name).ToArray());

            this.txtAccount.Text = RunningConfig.Instance.CurrentAccountName;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var accountName = this.txtAccount.Text;
            if (string.IsNullOrEmpty(accountName)) return;

            var remindInfo = new RemindInfo()
            {
                AccountName = accountName,
                StockCode = StockCode,
                UDPer = this.txtUDPer.Text,
                UpPrice = this.txtUpPrice.Text,
                DownPrice = this.txtDownPrice.Text,
                UpAveragePrice = this.txtUpAverage.Text,
                DownAveragePrice = this.txtDownAverage.Text,
                UpMinDayPricePer = this.txtUpMinDayPricePer.Text,
                DownMaxDayPricePer = this.txtDownMaxDayPricePer.Text,
                GoldMacd = this.txtGoldMacd.Checked,
                DieMacd = this.txtDieMacd.Checked,
                UpMacd = this.txtMacdUp.Checked,
                DownMacd = this.txtMacdDown.Checked
            };
            if (string.IsNullOrEmpty($"{remindInfo.UDPer}{remindInfo.UpPrice}{remindInfo.DownPrice}{remindInfo.UpAveragePrice}{remindInfo.DownAveragePrice}{remindInfo.UpMinDayPricePer}{remindInfo.DownMaxDayPricePer}") 
                && !this.txtGoldMacd.Checked && !this.txtDieMacd.Checked
                && !this.txtMacdUp.Checked && !this.txtMacdDown.Checked) return;

            StockRemindService.Create(remindInfo);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnRemindAll_Click(object sender, EventArgs e)
        {
            var accountName = this.txtAccount.Text;
            if (string.IsNullOrEmpty(accountName)) return;

            StockRemindService.AutoCreate(accountName);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnOKAll_Click(object sender, EventArgs e)
        {
            var accountName = this.txtAccount.Text;
            if (string.IsNullOrEmpty(accountName)) return;

            var remindInfo = new RemindInfo()
            {
                AccountName = accountName,
                UDPer = this.txtUDPer.Text,
                UpAveragePrice = this.txtUpAverage.Text,
                DownAveragePrice = this.txtDownAverage.Text,
                UpMinDayPricePer = this.txtUpMinDayPricePer.Text,
                DownMaxDayPricePer = this.txtDownMaxDayPricePer.Text,
                GoldMacd = this.txtGoldMacd.Checked,
                DieMacd = this.txtDieMacd.Checked
            };
            if (string.IsNullOrEmpty($"{remindInfo.UDPer}{remindInfo.UpAveragePrice}{remindInfo.DownAveragePrice}{remindInfo.UpMinDayPricePer}{remindInfo.DownMaxDayPricePer}") && !this.txtGoldMacd.Checked && !this.txtDieMacd.Checked) return;

            StockRemindService.Create(remindInfo);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnUDPer_Click(object sender, EventArgs e)
        {
            this.txtUDPer.Text = "3,5,9";
        }

        private void btnUpAverage_Click(object sender, EventArgs e)
        {
            this.txtUpAverage.Text = "30";
        }

        private void btnDownAverage_Click(object sender, EventArgs e)
        {
            this.txtDownAverage.Text = "20";
        }

        private void btnUpAverageReverse_Click(object sender, EventArgs e)
        {
            this.txtUpMinDayPricePer.Text = "2";
        }

        private void btnDownAverageReverse_Click(object sender, EventArgs e)
        {
            this.txtDownMaxDayPricePer.Text = "2";
        }

        private void btnClearRemind_Click(object sender, EventArgs e)
        {
            StockRemindService.Clear(StockCode, true);
        }

        private void btnClearAllRemind_Click(object sender, EventArgs e)
        {
            StockRemindService.Clear(clearAll: true);
        }
    }
}
