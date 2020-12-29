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
using StockSimulateService.Helper;
using StockSimulateCore.Data;

namespace StockPriceTools.UI
{
    public partial class NewStockForm : Form
    {
        public string StockCode { get; set; }
        public NewStockForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var stockCode = $"{this.txtType.Text}{this.txtCode.Text.Trim()}";
            if (string.IsNullOrEmpty(stockCode)) return;

            //var stockInfo = EastMoneyUtil.GetStockPrice(stockCode);
            //if (stockInfo == null) return;

            var stock = Repository.Instance.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if (stock == null)
            {
                stock = new StockEntity();
                stock.Code = stockCode;
                stock.Type = ObjectUtil.ToValue<int>(this.txtSTypeValue.Text, 0);
                stock.LockDay = ObjectUtil.ToValue<int>(this.txtDay.Text, 0);

                Repository.Instance.Insert<StockEntity>(stock);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.txtCode.Text.Length > 6) this.txtCode.Text = this.txtCode.Text.Substring(0, 6);
            this.txtType.Text = ObjectUtil.GetStockMarket(this.txtCode.Text);

            if (this.txtCode.Text.StartsWith("6"))
            {
                this.txtSType.Text = "沪深股";
                this.txtSTypeValue.Text = "0";
            }
            if (this.txtCode.Text.StartsWith("0"))
            {
                this.txtSType.Text = "沪深股";
                this.txtSTypeValue.Text = "0";
            }
            if (this.txtCode.Text.StartsWith("3"))
            {
                this.txtSType.Text = "创业板";
                this.txtSTypeValue.Text = "3";
            }
            if (this.txtCode.Text.StartsWith("5"))
            {
                this.txtSType.Text = "基金(ETF)";
                this.txtSTypeValue.Text = "1";
            }
            if (this.txtCode.Text.StartsWith("1"))
            {
                this.txtSType.Text = "基金(ETF)";
                this.txtSTypeValue.Text = "1";
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
            if (!string.IsNullOrEmpty(StockCode) && StockCode.Length >= 8)
            {
                this.txtCode.Text = StockCode.Substring(2, 6);
            }
        }

        private void txtSType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.txtSType.Text == "沪深股")
            {
                this.txtSTypeValue.Text = "0";
            }
            else if(this.txtSType.Text == "基金(ETF)")
            {
                this.txtSTypeValue.Text = "1";
            }
            else if (this.txtSType.Text == "指数")
            {
                this.txtSTypeValue.Text = "2";
            }
        }
    }
}
