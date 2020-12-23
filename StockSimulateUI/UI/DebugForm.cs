using StockSimulateCore.Entity;
using StockSimulateCore.Service;
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
    public partial class DebugForm : Form
    {
        private SQLiteDBUtil Repository = SQLiteDBUtil.Instance;
        public string StockCode { get; set; }
        public DebugForm()
        {
            InitializeComponent();
        }

        private void DebugForm_Load(object sender, EventArgs e)
        {
            this.txtStockCode.Text = StockCode;

            var accounts = Repository.QueryAll<AccountEntity>();
            if (accounts.Length == 0) return;

            this.txtAccount.Items.Clear();
            this.txtAccount.Items.AddRange(accounts.Select(c => c.Name).ToArray());
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            var accountName = this.txtAccount.Text;
            if (string.IsNullOrEmpty(accountName)) return;

            var stockCode = this.txtStockCode.Text;
            if (string.IsNullOrEmpty(stockCode)) return;

            var stockPrice = ObjectUtil.ToValue<decimal>(this.txtStockPrice.Text, 0);
            if (stockPrice <= 0) return;

            var strDealTime = this.txtDealTime.Text;
            if (string.IsNullOrEmpty(strDealTime)) return;

            DateTime dealTime = DateTime.Now;
            DateTime.TryParse(strDealTime, out dealTime);

            StockDebugService.MakeCheckStrategyRun(accountName, stockCode, stockPrice, dealTime);
        }

        private void btnClearDebug_Click(object sender, EventArgs e)
        {
            var accountName = this.txtAccount.Text;
            if (string.IsNullOrEmpty(accountName)) return;

            var stockCode = this.txtStockCode.Text;
            if (string.IsNullOrEmpty(stockCode)) return;

            StockDebugService.ClearDeugData(accountName, stockCode);
        }

        private void btnGatherHisPrice_Click(object sender, EventArgs e)
        {
            StockGatherService.GatherHisPriceData();
        }
    }
}
