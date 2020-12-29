using StockSimulateDomain.Entity;
using StockSimulateDomain.Model;
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
    public partial class AccountForm : Form
    {
        public AccountForm()
        {
            InitializeComponent();
        }

        private void AccountForm_Load(object sender, EventArgs e)
        {
            this.LoadAccountInfo();
        }

        void LoadAccountInfo()
        {
            var accounts = Repository.Instance.QueryAll<AccountEntity>();
            if (accounts.Length == 0) return;

            this.txtName.Items.Clear();
            this.txtName.Items.AddRange(accounts.Select(c => c.Name).ToArray());

            var realAccount = accounts.FirstOrDefault(c => c.Name == RunningConfig.Instance.CurrentAccountName);
            if (realAccount != null)
            {
                this.txtName.Text = realAccount.Name;
                this.txtAmount.Text = $"{realAccount.Amount}";
                this.txtHoldAmount.Text = $"{realAccount.HoldAmount}";
                this.txtBuyAmount.Text = $"{realAccount.BuyAmount}";
                this.txtTotalAmount.Text = $"{realAccount.TotalAmount}";
                this.txtCash.Text = $"{realAccount.Cash}";
                this.txtProfit.Text = $"{realAccount.Profit}";
                this.txtEmail.Text = realAccount.Email;
                this.txtQQ.Text = realAccount.QQ;
                this.txtRealType.Text = realAccount.RealType == 1 ? "实盘" : "模拟盘";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var accountName = this.txtName.Text;
            if (string.IsNullOrEmpty(accountName)) return;

            if (MessageBox.Show($"确认要保存选中的账户数据?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;

            var account = Repository.Instance.QueryFirst<AccountEntity>($"Name='{this.txtName.Text}'");
            if (account == null)
            {
                account = new AccountEntity();
                account.Name = this.txtName.Text;
                account.Amount = ObjectUtil.ToValue<decimal>(this.txtAmount.Text, 0);
                account.Cash = account.Amount;
                account.Email = this.txtEmail.Text;
                account.QQ = this.txtQQ.Text;
                account.RealType = this.txtRealType.Text == "实盘" ? 1 : 0;
                Repository.Instance.Insert<AccountEntity>(account);
            }
            else
            {
                account.Name = this.txtName.Text;
                account.Amount = ObjectUtil.ToValue<decimal>(this.txtAmount.Text, 0);
                account.Email = this.txtEmail.Text;
                account.Cash = account.Amount - account.BuyAmount;
                account.QQ = this.txtQQ.Text;
                account.RealType = this.txtRealType.Text == "实盘" ? 1 : 0;
                Repository.Instance.Update<AccountEntity>(account);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtName_SelectedIndexChanged(object sender, EventArgs e)
        {
            var account = Repository.Instance.QueryFirst<AccountEntity>($"Name='{this.txtName.Text}'");
            if (account != null)
            {
                this.txtName.Text = account.Name;
                this.txtAmount.Text = $"{account.Amount}";
                this.txtHoldAmount.Text = $"{account.HoldAmount}";
                this.txtBuyAmount.Text = $"{account.BuyAmount}";
                this.txtTotalAmount.Text = $"{account.TotalAmount}";
                this.txtCash.Text = $"{account.Cash}";
                this.txtProfit.Text = $"{account.Profit}";
                this.txtEmail.Text = account.Email;
                this.txtQQ.Text = account.QQ;
                this.txtRealType.Text = account.RealType == 1 ? "实盘" : "模拟盘";
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var accountName =  this.txtName.Text;
            if (string.IsNullOrEmpty(accountName)) return;

            if (MessageBox.Show($"确认要删除选中的账户数据?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;

            var account = Repository.Instance.QueryFirst<AccountEntity>($"Name='{this.txtName.Text}'");
            if (account == null) return;

            Repository.Instance.Delete<AccountEntity>(account);
            Repository.Instance.Delete<AccountStockEntity>($"AccountName='{accountName}'");
            Repository.Instance.Delete<ExchangeOrderEntity>($"AccountName='{accountName}'");

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}