using StockSimulateDomain.Entity;
using StockSimulateService.Service;
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
using StockSimulateNetService.Serivce;
using StockSimulateCore.Config;
using StockSimulateCore.Data;

namespace StockSimulateUI.UI
{
    public partial class DebugForm : Form
    {
        public string StockCode { get; set; }
        public DebugForm()
        {
            InitializeComponent();
        }

        private void DebugForm_Load(object sender, EventArgs e)
        {
            this.txtStockCode.Text = StockCode;

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

        private void btnCheckDBTable_Click(object sender, EventArgs e)
        {
            Repository.Instance.InitDataBase();
        }

        private void btnClearDebug_Click(object sender, EventArgs e)
        {
            var accountName = this.txtAccount.Text;
            if (string.IsNullOrEmpty(accountName)) return;

            var stockCode = this.txtStockCode.Text;
            if (string.IsNullOrEmpty(stockCode)) return;

            StockDebugService.ClearDeugData(accountName, stockCode);
        }

        private void btnCalcAvgPrice_Click(object sender, EventArgs e)
        {
            StockPriceService.CalculateAllAvgrage(0);

            var lastAvgPrices = Repository.Instance.QueryAll<StockAverageEntity>($"StockCode='{RunningConfig.Instance.SHZSOfStockCode}'", "DealDate desc", RunningConfig.Instance.KeepStockAssistTargetDays);
            if (lastAvgPrices.Length == RunningConfig.Instance.KeepStockAssistTargetDays)
            {
                var dealDate = lastAvgPrices.OrderBy(c => c.DealDate).FirstOrDefault().DealDate;

                Repository.Instance.Delete<StockAverageEntity>($"DealDate<'{dealDate}'");
            }
        }

        private void btnCalcMACD_Click(object sender, EventArgs e)
        {
            StockPriceService.CalculateAllMACD(startDate:"1900-01-01");

            var lastAvgPrices = Repository.Instance.QueryAll<StockMacdEntity>($"StockCode='{RunningConfig.Instance.SHZSOfStockCode}'", "DealDate desc", RunningConfig.Instance.KeepStockAssistTargetDays);
            if (lastAvgPrices.Length == RunningConfig.Instance.KeepStockAssistTargetDays)
            {
                var dealDate = lastAvgPrices.OrderBy(c => c.DealDate).FirstOrDefault().DealDate;

                Repository.Instance.Delete<StockMacdEntity>($"DealDate<'{dealDate}'");
            }
        }

        private void btnClearHisData_Click(object sender, EventArgs e)
        {
            StockPriceService.Clear();
            StockRemindService.Clear();
        }
    }
}
