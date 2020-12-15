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
    public partial class NewExchangeForm : Form
    {
        private SQLiteDBUtil Repository = SQLiteDBUtil.Instance;
        public int DealQty { get; set; }
        public decimal DealPrice { get; set; }

        public string AccountName { get; set; }

        public string StockCode { get; set; }
        public string StockName { get; set; }

        public int DealType { get; set; }

        public NewExchangeForm()
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

            var account = Repository.QueryFirst<AccountEntity>($"Name='{this.txtAccount.Text}'");
            if (account == null) return;

            //买入
            if(this.DealType == 0)
            {
                if (this.DealQty * this.DealPrice > account.Cash) return;

                var exchange = new ExchangeOrderEntity()
                {
                    AccountName = account.Name,
                    StockCode = StockCode,
                    Qty = this.DealQty,
                    Price = this.DealPrice,
                    ExchangeType = "买入",
                    Amount = this.DealPrice * this.DealQty,
                    ExchangeTime = DateTime.Now,
                    HoldQty = this.DealQty,
                };
                Repository.Insert<ExchangeOrderEntity>(exchange);

                account.BuyAmount += exchange.Amount;
                account.Cash -= exchange.Amount;
                Repository.Update<AccountEntity>(account);

                var accountStock = Repository.QueryFirst<AccountStockEntity>($"AccountName='{this.txtAccount.Text}' and StockCode='{StockCode}'");
                if (accountStock == null)
                {
                    accountStock = new AccountStockEntity()
                    {
                        AccountName = account.Name,
                        StockCode = StockCode,
                        StockName = StockName,
                        HoldQty = exchange.Qty,
                        TotalBuyAmount = exchange.Amount,
                        Cost = exchange.Price,
                    };
                    Repository.Insert<AccountStockEntity>(accountStock);
                }
                else
                {
                    accountStock.HoldQty += exchange.Qty;
                    accountStock.TotalBuyAmount += exchange.Amount;
                    accountStock.Cost = Math.Round(accountStock.TotalBuyAmount / accountStock.HoldQty, 2);
                    Repository.Update<AccountStockEntity>(accountStock);
                }
            }
            else
            {
                var accountStock = Repository.QueryFirst<AccountStockEntity>($"AccountName='{this.txtAccount.Text}' and StockCode='{StockCode}'");
                if (accountStock == null) return;

                if (this.DealQty > accountStock.HoldQty) return;

                var exchange = new ExchangeOrderEntity()
                {
                    AccountName = account.Name,
                    StockCode = StockCode,
                    Qty = this.DealQty,
                    Price = this.DealPrice,
                    ExchangeType = "卖出",
                    Amount = this.DealPrice * this.DealQty,
                    ExchangeTime = DateTime.Now,
                    HoldQty = accountStock.HoldQty - this.DealQty
                };
                Repository.Insert<ExchangeOrderEntity>(exchange);

                account.BuyAmount -= exchange.Amount;
                account.Cash += exchange.Amount;
                Repository.Update<AccountEntity>(account);

                accountStock.HoldQty -= exchange.Qty;
                accountStock.TotalBuyAmount -= exchange.Amount;
                if (accountStock.HoldQty == 0)
                {
                    accountStock.Cost = 0;
                }
                else
                {
                    accountStock.Cost = Math.Round(accountStock.TotalBuyAmount / accountStock.HoldQty, 2);
                }
                Repository.Update<AccountStockEntity>(accountStock);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
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
                    this.txtCouldExchange.Text = $"{accountStock.HoldQty}";
                }
            }
        }
    }
}
