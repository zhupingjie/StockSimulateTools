using StockSimulateDomain.Entity;
using StockSimulateDomain.Model;
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
    public partial class ExchangeForm : Form
    {
        private MySQLDBUtil Repository = MySQLDBUtil.Instance;
        public int DealQty { get; set; }
        public decimal DealPrice { get; set; }

        public string AccountName { get; set; }

        public string StockCode { get; set; }
        public string StockName { get; set; }

        public int DealType { get; set; }

        public ExchangeForm()
        {
            InitializeComponent();
        }

        private void NewExchangeForm_Load(object sender, EventArgs e)
        {
            var accounts = Repository.QueryAll<AccountEntity>();
            if (accounts.Length == 0) return;

            this.txtAccount.Items.Clear();
            this.txtAccount.Items.AddRange(accounts.Select(c => c.Name).ToArray());

            this.txtDealPrice.Text = $"{this.DealPrice}";

            if (this.DealType == 0)
            {
                this.txtDealType.Text = "买入";
                this.lblCouldText.Text = "可交易金额";
            }
            else
            {
                this.txtDealType.Text = "卖出";
                this.lblCouldText.Text = "可交易数量";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DealQty = ObjectUtil.ToValue<int>(this.txtDealQty.Text, 0);
            this.DealPrice = ObjectUtil.ToValue<decimal>(this.txtDealPrice.Text, 0);

            if (this.DealQty == 0 || this.DealQty % 100 != 0) return;
            if (this.DealPrice <= 0) return;

            var accountName = this.txtAccount.Text;
            if (string.IsNullOrEmpty(accountName)) return;

            var strategyName = this.txtStrategyName.Text;
            var strategyTarget = this.txtStrategyTarget.Text;

            //买入
            if (this.DealType == 0)
            {
                var result = StockExchangeService.Buy(new ExchangeInfo()
                {
                    AccountName = accountName,
                    StockCode = StockCode,
                    Qty = this.DealQty,
                    Price = this.DealPrice,
                    StrategyName = strategyName,
                    Target = strategyTarget
                });
                if (result.Success)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(result.Message, "交易提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                var result = StockExchangeService.Sale(new ExchangeInfo()
                {
                    AccountName = accountName,
                    StockCode = StockCode,
                    Qty = this.DealQty,
                    Price = this.DealPrice,
                    StrategyName = strategyName,
                    Target = strategyTarget
                });
                if (result.Success)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(result.Message, "交易提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void txtDealPrice_TextChanged(object sender, EventArgs e)
        {
            var amount = ObjectUtil.ToValue<decimal>(this.txtDealPrice.Text, 0) * ObjectUtil.ToValue<int>(this.txtDealQty.Text, 0);
            this.txtDealAmount.Text = $"{amount}"; 
        }

        private void txtDealQty_TextChanged(object sender, EventArgs e)
        {
            var amount = ObjectUtil.ToValue<decimal>(this.txtDealPrice.Text, 0) * ObjectUtil.ToValue<int>(this.txtDealQty.Text, 0);
            this.txtDealAmount.Text = $"{amount}";
        }

        private void txtAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            var account = Repository.QueryFirst<AccountEntity>($"Name='{this.txtAccount.Text}'");
            if (account == null) return;

            if (this.DealType == 0)
            {
                this.txtCouldExchange.Text = $"{account.Cash}";

                var stockStrategys = Repository.QueryAll<StockStrategyEntity>($"AccountName='{this.txtAccount.Text}' and StockCode='{StockCode}' and ExecuteOK != 1");

                this.txtStrategyName.Items.Clear();
                this.txtStrategyName.Items.AddRange(stockStrategys.Select(c => c.StrategyName).Distinct().ToArray());

            }
            else
            {
                var accountStock = Repository.QueryFirst<AccountStockEntity>($"AccountName='{this.txtAccount.Text}' and StockCode='{StockCode}'");
                if (accountStock == null)
                {
                    this.txtDealQty.Text = "0";
                    this.txtDealQty.Enabled = false;
                }
                else
                {
                    var couldQty = accountStock.HoldQty;
                    if (accountStock.LockDate.HasValue && accountStock.LockDate == DateTime.Now.Date) couldQty -= accountStock.LockQty;

                    this.txtCouldExchange.Text = $"{couldQty}";
                    this.txtDealQty.Enabled = true;
                }
            }
        }

        private void txtStrategyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            var strategyName = this.txtStrategyName.Text;
            var accountName = this.txtAccount.Text;

            if (string.IsNullOrEmpty(strategyName) || string.IsNullOrEmpty(accountName)) return;

            var stockStrategys = Repository.QueryAll<StockStrategyEntity>($"AccountName='{this.txtAccount.Text}' and StockCode='{StockCode}' and StrategyName='{strategyName}' and Condition={this.DealType} and ExecuteOK != 1");

            this.txtStrategyTarget.Items.Clear();
            this.txtStrategyTarget.Items.AddRange(stockStrategys.Select(c => c.Target).Distinct().ToArray());
        }
    }
}
