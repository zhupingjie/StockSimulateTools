using StockSimulateCore.Entity;
using StockSimulateCore.Service;
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
    public partial class ValuateForm : Form
    {
        private SQLiteDBUtil Repository = SQLiteDBUtil.Instance;
        public string StockCode { get; set; }
        public ValuateForm()
        {
            InitializeComponent();
        }

        private void ValuateForm_Load(object sender, EventArgs e)
        {
            var stock = Repository.QueryFirst<StockEntity>($"Code='{StockCode}'");
            if (stock == null) return;

            var newMainTargets = Repository.QueryAll<MainTargetEntity>($"StockCode='{StockCode}' and Rtype=2", "Date desc", 8);
            if (newMainTargets.Length == 0) return;

            var lastMainTarget = newMainTargets.FirstOrDefault();
            var nowYear = lastMainTarget.Date.Substring(0, 4);
            var yetYear = (int.Parse(nowYear) - 1).ToString();
            var yetDates = newMainTargets.Where(c => c.Date.StartsWith(nowYear)).Select(c => c.Date.Replace(nowYear, yetYear)).ToArray();
            var date = $"{yetYear}-12-31";
            var mainTarget = Repository.QueryFirst<MainTargetEntity>($"StockCode='{StockCode}' and Date='{date}' and Rtype=1");
            if(mainTarget != null)
            {
                this.txtYetNetProfit.Text = $"{mainTarget.Gsjlr}";
                this.txtYetGrow.Text = $"{mainTarget.Gsjlrtbzz}";
                this.txtYetEPS.Text = $"{mainTarget.Jbmgsy}";
            }

            var hasJlrs = newMainTargets.Where(c => c.Date.StartsWith(yetYear) && !yetDates.Contains(c.Date)).ToArray();
            if(hasJlrs.Length > 0)
            {
                this.txtYetLostNetProfit.Text = $"{hasJlrs.Sum(c => c.Gsjlr)}";
            }
            this.txtName.Text = stock.Name;
            this.txtCapital.Text = $"{stock.Capital}";
            this.txtTTM.Text = $"{stock.TTM}";
            this.txtNetProfit.Text = $"{stock.NetProfit}";
            this.txtPrice.Text = $"{stock.Price}";
            this.txtAmount.Text = $"{stock.Amount}";
        }

        private void Valuate()
        {
            var capital = ObjectUtil.ToValue<decimal>(this.txtCapital.Text, 0);
            var yetNetProfit = ObjectUtil.ToValue<decimal>(this.txtYetNetProfit.Text, 0);
            var wantProfitGrow = ObjectUtil.ToValue<decimal>(this.txtWantProfitGrow.Text, 0);
            var price = ObjectUtil.ToValue<decimal>(this.txtPrice.Text, 0);
            var yetEPS = ObjectUtil.ToValue<decimal>(this.txtYetEPS.Text, 0);
            var wantPE = ObjectUtil.ToValue<decimal>(this.txtWantPE.Text, 0);
            var safeRate = ObjectUtil.ToValue<decimal>(this.txtSafeRate.Text, 0);
            var yetLostNetProfit = ObjectUtil.ToValue<decimal>(this.txtYetLostNetProfit.Text, 0);

            var wantNetProfit = yetNetProfit * (1 + wantProfitGrow / 100m);
            this.txtWantNetProfit.Text = $"{wantNetProfit}";

            var lostNetProfig = wantNetProfit - yetLostNetProfit;
            this.txtLostNetProfit.Text = $"{lostNetProfig}";

            var wantEPS = Math.Round(wantNetProfit / capital, 3);
            this.txtWantEPS.Text = $"{wantEPS}";

            var wantAmount = Math.Round(wantPE * wantNetProfit, 2);
            this.txtWantAmount.Text = $"{wantAmount}";

            var wantPrice = Math.Round(wantPE * wantEPS, 2) * (safeRate / 100m); ;
            this.txtTarget.Text = $"{wantPrice}";
        }

        private void txtWantProfitGrow_TextChanged(object sender, EventArgs e)
        {
            this.Valuate();
        }

        private void txtWantPE_TextChanged(object sender, EventArgs e)
        {
            this.Valuate();
        }
        private void txtSafeRate_TextChanged(object sender, EventArgs e)
        {
            this.Valuate();
        }
        private void txtWantNetProfit_TextChanged(object sender, EventArgs e)
        {
            this.Valuate();
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            var target = ObjectUtil.ToValue<decimal>(this.txtTarget.Text, 0);
            var growth = ObjectUtil.ToValue<decimal>(this.txtWantProfitGrow.Text, 0);
            var pe = ObjectUtil.ToValue<decimal>(this.txtWantPE.Text, 0);

            if (target == 0 || growth == 0 || pe == 0) return;

            StockService.Valate(StockCode, target, growth, pe);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
