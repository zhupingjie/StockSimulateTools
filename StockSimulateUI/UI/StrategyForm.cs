using StockPriceTools;
using StockSimulateCore.Config;
using StockSimulateCore.Entity;
using StockSimulateCore.Model;
using StockSimulateCore.Service;
using StockSimulateCore.Utils;
using StockSimulateUI.UC;
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
    public partial class StrategyForm : Form
    {
        private SQLiteDBUtil Repository = SQLiteDBUtil.Instance;
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public StrategyForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.txtStockCode.Text = StockCode;
            this.txtStockName.Text = StockName;

            var accounts = Repository.QueryAll<AccountEntity>();
            if (accounts.Length == 0) return;

            this.txtAccountName.Items.Clear();
            this.txtAccountName.Items.AddRange(accounts.Select(c => c.Name).ToArray());
        }

        private void txtName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.txtStrategyName.Text == "左侧交易")
            {
                var uc = new LeftExchangeUC();
                uc.Dock = DockStyle.Fill;
                uc.StockCode = StockCode;
                uc.StockName = StockName;
                uc.StrategyName = this.txtStrategyName.Text;

                if (this.pnlContainer.HasChildren) this.pnlContainer.Controls.Clear();
                this.pnlContainer.Controls.Add(uc);
            }
            else if (this.txtStrategyName.Text == "右侧交易")
            {
                var uc = new RightExchangeUC();
                uc.Dock = DockStyle.Fill;
                uc.StockCode = StockCode;
                uc.StockName = StockName;
                uc.StrategyName = this.txtStrategyName.Text;

                if (this.pnlContainer.HasChildren) this.pnlContainer.Controls.Clear();
                this.pnlContainer.Controls.Add(uc);
            }
            else if (this.txtStrategyName.Text == "差价交易")
            {
                var uc = new TExchangeUC();
                uc.Dock = DockStyle.Fill;
                uc.StockCode = StockCode;
                uc.StockName = StockName;
                uc.StrategyName = this.txtStrategyName.Text;

                if (this.pnlContainer.HasChildren) this.pnlContainer.Controls.Clear();
                this.pnlContainer.Controls.Add(uc);
            }
        }

        private void btnCalcuate_Click(object sender, EventArgs e)
        {
            if (!pnlContainer.HasChildren) return;

            var strategyName = this.txtStrategyName.Text;
            if (string.IsNullOrEmpty(strategyName)) return;

            var accountName = this.txtAccountName.Text;
            if (string.IsNullOrEmpty(accountName)) return;

            var executeMode = this.txtAutoBS.Checked ? 1 : 0;

            var uc = pnlContainer.Controls[0] as BaseUC;
            uc.CalcuateStrategy(accountName, strategyName, StockCode);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var strategyName = this.txtStrategyName.Text;
            if (string.IsNullOrEmpty(strategyName)) return;

            var accountName = this.txtAccountName.Text;
            if (string.IsNullOrEmpty(accountName)) return;

            var executeMode = this.txtAutoBS.Checked ? 1 : 0;

            if (!pnlContainer.HasChildren) return;

            var uc = pnlContainer.Controls[0] as BaseUC;
            var strategy = uc.GetStrategyInfo();
            strategy.AccountName = accountName;
            strategy.StockCode = StockCode;
            strategy.Name = strategyName;
            strategy.ExecuteMode = executeMode;

            StockStrategyService.MakeStockStrategys(strategy, true);
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }

    public class StockPriceInfo
    {
        public decimal CurrentPrice { get; set; }

        public decimal Amount { get; set; }

        public decimal Count
        {
            get
            {
                return Math.Floor(Amount / CurrentPrice);
            }
        }
    }
}
