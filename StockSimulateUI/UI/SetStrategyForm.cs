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
    public partial class SetStrategyForm : Form
    {
        private SQLiteDBUtil Repository = SQLiteDBUtil.Instance;
        public string StrategyName { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal BuyAmount { get; set; }
        public decimal SalePrice { get; set; }
        public SetStrategyForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.StrategyName = txtStrategy.Text;
            this.BuyPrice = GetTextValue(this.txtBuyPrice);
            this.BuyAmount = GetTextValue(this.txtBuyAmount);
            this.SalePrice = GetTextValue(this.txtSalePrice);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void SetStrategyForm_Load(object sender, EventArgs e)
        {
            var strategys = Repository.QueryAll<StrategyEntity>();
            foreach(var item in strategys)
            {
                this.txtStrategy.Items.Add(item.Name);
            }
            this.txtStrategy.SelectedIndex = 0;
        }

        decimal GetTextValue(TextBox textBox)
        {
            decimal d = 0;
            decimal.TryParse(textBox.Text, out d);
            return Math.Round(d, 3);
        }
    }
}
