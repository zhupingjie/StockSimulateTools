using StockPriceTools.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockPriceTools
{
    public partial class DesktopFrom : Form
    {
        public DesktopFrom()
        {
            InitializeComponent();
        }

        private void btnCalcuate_Click(object sender, EventArgs e)
        {
            var frm = new CalcuateForm();
            frm.Show();
        }

        private void btnValuatie_Click(object sender, EventArgs e)
        {
            var frm = new VoluatieForm();
            frm.Show();
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            SQLiteDBHelper.Instance.InitSQLiteDB();
        }
    }
}
