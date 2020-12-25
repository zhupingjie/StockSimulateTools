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
using StockSimulateCoreService.Utils;

namespace StockSimulateUI.UI
{
    public partial class RemindForm : Form
    {
        private MySQLDBUtil Repository = MySQLDBUtil.Instance;
        public string StockCode { get; set; }

        public RemindForm()
        {
            InitializeComponent();
        }

        private void SetRemindForm_Load(object sender, EventArgs e)
        {
            var accounts = Repository.QueryAll<AccountEntity>();
            if (accounts.Length == 0) return;

            this.txtAccount.Items.Clear();
            this.txtAccount.Items.AddRange(accounts.Select(c => c.Name).ToArray());
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var accountName = this.txtAccount.Text;
            if (string.IsNullOrEmpty(accountName)) return;

            var uDPer = this.txtUDPer.Text;
            var upPrice = this.txtUpPrice.Text;
            var downPrice = this.txtDownPrice.Text;
            var upAveragePrice = this.txtUpAverage.Text;
            var downAveragePrice = this.txtDownAverage.Text;

            StockRemindService.Create(accountName, StockCode, uDPer, upPrice, downPrice, upAveragePrice, downAveragePrice);

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

            var uDPer = this.txtUDPer.Text;
            if (string.IsNullOrEmpty(uDPer)) return;

            var averagePrice = this.txtUpAverage.Text;

            StockRemindService.Create(accountName, uDPer);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
