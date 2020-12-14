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
            var account = Repository.QueryFirst<AccountEntity>();
            if (account == null) return;

            this.txtName.Text = account.Name;
            this.txtAmount.Text = $"{account.Amount}";
            this.txtHoldAmount.Text = $"{account.HoldAmount}";
            this.txtProfit.Text = $"{account.Profit}";
            this.txtEmail.Text = account.Email;
            this.txtQQ.Text = account.QQ;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var account = Repository.QueryFirst<AccountEntity>();
            if (account == null)
            {
                account = new AccountEntity();
                account.Name = this.txtName.Text;
                account.Amount = ObjectUtil.ToValue<decimal>(this.txtAmount.Text, 0);
                account.Email = this.txtEmail.Text;
                account.QQ = this.txtQQ.Text;
                Repository.Insert<AccountEntity>(account);
            }
            else
            {
                account.Name = this.txtName.Text;
                account.Amount = ObjectUtil.ToValue<decimal>(this.txtAmount.Text, 0);
                account.Email = this.txtEmail.Text;
                account.QQ = this.txtQQ.Text;
                Repository.Update<AccountEntity>(account);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}