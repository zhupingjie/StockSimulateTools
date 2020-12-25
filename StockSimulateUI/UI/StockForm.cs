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
using StockSimulateCoreService.Utils;
using StockSimulateDomain.Utils;
using StockSimulateService.Helper;

namespace StockPriceTools.UI
{
    public partial class NewStockForm : Form
    {
        private MySQLDBUtil Repository = MySQLDBUtil.Instance;
        public NewStockForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var stockCode = $"{this.txtType.Text}{this.txtCode.Text.Trim()}";
            if (string.IsNullOrEmpty(stockCode)) return;

            var stockInfo = EastMoneyUtil.GetStockPrice(stockCode);
            if (stockInfo == null) return;

            var stock = Repository.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if (stock == null)
            {
                //类型
                stockInfo.Stock.Type = ObjectUtil.ToValue<int>(this.txtSTypeValue.Text, 0);
                stockInfo.Stock.LockDay = ObjectUtil.ToValue<int>(this.txtDay.Text, 0);

                Repository.Insert<StockEntity>(stockInfo.Stock);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.txtCode.Text.Length > 6) this.txtCode.Text = this.txtCode.Text.Substring(0, 6);
            if (this.txtCode.Text.StartsWith("6"))
            {
                this.txtType.Text = "SH";
                this.txtSType.Text = "沪深";
                this.txtSTypeValue.Text = "0";
            }
            if (this.txtCode.Text.StartsWith("0"))
            {
                this.txtType.Text = "SZ";
                this.txtSType.Text = "沪深";
                this.txtSTypeValue.Text = "0";
            }
            if (this.txtCode.Text.StartsWith("5"))
            {
                this.txtType.Text = "SH";
                this.txtSType.Text = "ETF";
                this.txtSTypeValue.Text = "1";
            }
            if (this.txtCode.Text.StartsWith("1"))
            {
                this.txtType.Text = "SZ";
                this.txtSType.Text = "指数";
                this.txtSTypeValue.Text = "2";
            }
        }

        private void NewStockForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnOK_Click(sender, e);
            }
        }

        private void NewStockForm_Load(object sender, EventArgs e)
        {

        }
    }
}
