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
    public partial class SetRemindForm : Form
    {
        public decimal BasePrice { get; set; }
        public decimal UDPer { get; set; }
        public decimal UpPrice { get; set; }
        public decimal DownPrice { get; set; }

        public SetRemindForm()
        {
            InitializeComponent();
        }

        private void SetRemindForm_Load(object sender, EventArgs e)
        {
            this.txtPrice.Text = $"{BasePrice}";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.BasePrice = ObjectUtil.ToValue<decimal>(this.txtPrice.Text, 0);
            this.UDPer = ObjectUtil.ToValue<decimal>(this.txtUDPer.Text, 0);
            this.UpPrice = ObjectUtil.ToValue<decimal>(this.txtUpPrice.Text, 0);
            this.DownPrice = ObjectUtil.ToValue<decimal>(this.txtDownPrice.Text, 0);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
