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
    public partial class RemindForm : Form
    {
        public string UDPer { get; set; }
        public string UpPrice { get; set; }
        public string DownPrice { get; set; }

        public RemindForm()
        {
            InitializeComponent();
        }

        private void SetRemindForm_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.UDPer = this.txtUDPer.Text;
            this.UpPrice = this.txtUpPrice.Text;
            this.DownPrice = this.txtDownPrice.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
