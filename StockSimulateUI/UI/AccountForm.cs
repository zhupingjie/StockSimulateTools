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
    public partial class AccountForm : Form
    {
        private SQLiteDBUtil Repository = SQLiteDBUtil.Instance;
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
            var accounts = Repository.QueryAll<AccountEntity>();
            if (accounts.Length == 0) return;

            this.txtName.Items.Clear();
            this.txtName.Items.AddRange(accounts.Select(c => c.Name).ToArray());

            var realAccount = accounts.FirstOrDefault(c => c.RealType);
            if (realAccount != null)
            {
                this.txtName.Text = realAccount.Name;
                this.txtAmount.Text = $"{realAccount.Amount}";
                this.txtHoldAmount.Text = $"{realAccount.HoldAmount}";
                this.txtBuyAmount.Text = $"{realAccount.BuyAmount}";
                this.txtCash.Text = $"{realAccount.Cash}";
                this.txtProfit.Text = $"{realAccount.Profit}";
                this.txtEmail.Text = realAccount.Email;
                this.txtQQ.Text = realAccount.QQ;
                this.txtRealType.Checked = realAccount.RealType;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var account = Repository.QueryFirst<AccountEntity>($"Name='{this.txtName.Text}'");
            if (account == null)
            {
                account = new AccountEntity();
                account.Name = this.txtName.Text;
                account.Amount = ObjectUtil.ToValue<decimal>(this.txtAmount.Text, 0);
                account.Cash = account.Amount;
                account.Email = this.txtEmail.Text;
                account.QQ = this.txtQQ.Text;
                account.RealType = this.txtRealType.Checked;
                Repository.Insert<AccountEntity>(account);
            }
            else
            {
                account.Name = this.txtName.Text;
                account.Amount = ObjectUtil.ToValue<decimal>(this.txtAmount.Text, 0);
                account.Email = this.txtEmail.Text;
                account.Cash = account.Amount - account.BuyAmount;
                account.QQ = this.txtQQ.Text;
                account.RealType = this.txtRealType.Checked;
                Repository.Update<AccountEntity>(account);
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
            var account = Repository.QueryFirst<AccountEntity>($"Name='{this.txtName.Text}'");
            if (account != null)
            {
                this.txtName.Text = account.Name;
                this.txtAmount.Text = $"{account.Amount}";
                this.txtHoldAmount.Text = $"{account.HoldAmount}";
                this.txtBuyAmount.Text = $"{account.BuyAmount}";
                this.txtCash.Text = $"{account.Cash}";
                this.txtProfit.Text = $"{account.Profit}";
                this.txtEmail.Text = account.Email;
                this.txtQQ.Text = account.QQ;
                this.txtRealType.Checked = account.RealType;
            }
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {

        }
    }
}