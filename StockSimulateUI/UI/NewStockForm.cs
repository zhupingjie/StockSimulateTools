using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            this.StockCode = $"{this.comboBox1.Text}{this.textBox1.Text.Trim()}";
            if (string.IsNullOrEmpty(StockCode)) return;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Length > 6) this.textBox1.Text = this.textBox1.Text.Substring(0, 6);
            if (this.textBox1.Text.StartsWith("6")) this.comboBox1.Text = "SH";
            if (this.textBox1.Text.StartsWith("0")) this.comboBox1.Text = "SZ";
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
