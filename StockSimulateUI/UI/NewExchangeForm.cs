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
        public decimal DealQty { get; set; }
        public decimal DealPrice { get; set; }

        public int DealType { get; set; }

        public NewExchangeForm()
        {
            InitializeComponent();
        }

        private void NewExchangeForm_Load(object sender, EventArgs e)
        {
            if (this.DealType == 0) this.txtDealType.Text = "买入";
            else this.txtDealType.Text = "卖出";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DealQty = ObjectUtil.ToValue<int>(this.txtDealQty.Text, 0);
            this.DealPrice = ObjectUtil.ToValue<decimal>(this.txtDealPrice.Text, 0);

            if (this.DealQty == 0 || this.DealQty % 100 != 0) return;
            if (this.DealPrice <= 0) return;

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
    }
}
