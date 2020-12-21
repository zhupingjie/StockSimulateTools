﻿using StockSimulateCore.Entity;
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

            var newMainTargets = Repository.QueryAll<MainTargetEntity>($"StockCode='{StockCode}' and Date<>'' and Rtype=2", "Date desc", 8);
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

        private void Valuate(bool changeNetProfit = false)
        {
            var price = ObjectUtil.ToValue<decimal>(this.txtPrice.Text, 0);
            var wantGrowth = ObjectUtil.ToValue<decimal>(this.txtWantProfitGrow.Text, 0);
            var wantPE = ObjectUtil.ToValue<decimal>(this.txtWantPE.Text, 0);
            var wantNetProfit = ObjectUtil.ToValue<decimal>(this.txtWantNetProfit.Text, 0);
            var safeRate = ObjectUtil.ToValue<decimal>(this.txtSafeRate.Text, 0);

            var result = StockValuateService.Valuate(StockCode, wantGrowth, wantPE, safeRate, changeNetProfit ? wantNetProfit : 0);
            if (result == null) return;

            this.txtWantNetProfit.Text = $"{result.NetProfit}";
            this.txtLostNetProfit.Text = $"{result.LostNetProfit}";
            this.txtWantEPS.Text = $"{result.EPS}";
            this.txtWantAmount.Text = $"{result.Amount}";
            this.txtTarget.Text = $"{result.Price}";
            this.txtAdvise.Text = result.Advise;
            this.txtUPPer.Text = $"{result.UPPer}%";
            this.txtSafePrice.Text = $"{result.SafePrice}";
            this.txtSafeUPPer.Text = $"{result.SafeUPPer}%";
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
            this.Valuate(true);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var target = ObjectUtil.ToValue<decimal>(this.txtTarget.Text, 0);
            var growth = ObjectUtil.ToValue<decimal>(this.txtWantProfitGrow.Text, 0);
            var pe = ObjectUtil.ToValue<decimal>(this.txtWantPE.Text, 0);
            var safety = ObjectUtil.ToValue<decimal>(this.txtSafePrice.Text, 0);
            var advise = this.txtAdvise.Text;

            if (growth == 0 || pe == 0) return;

            StockService.SaveValuateResult(StockCode, target, safety, growth, pe, advise);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnValuateAll_Click(object sender, EventArgs e)
        {
            var safety = ObjectUtil.ToValue<decimal>(this.txtSafePrice.Text, 0);

            var results = StockValuateService.Valuate(safety);
            if(results.Length > 0)
            {
                StockService.SaveValuateResult(results);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void tabControlBottom_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadTabGridList(this.tabControlBottom.SelectedIndex, StockCode);
        }

        void LoadTabGridList(int tabIndex, string stockCode)
        {
            switch (tabIndex)
            {
                case 0:
                    this.LoadMainTargetInfo(stockCode);
                    break;
                case 1:
                    this.LoadBalanceTargetInfo(stockCode);
                    break;
                case 2:
                    this.LoadProfitTargetInfo(stockCode);
                    break;
                case 3:
                    this.LoadCashTargetInfo(stockCode);
                    break;
            }
        }

        void LoadMainTargetInfo(string stockCode, int rtype = 0)
        {
            var mainTargets = Repository.QueryAll<MainTargetEntity>($"StockCode='{stockCode}' and Rtype={rtype}", "Date desc", 60);
            var dt = EastMoneyUtil.ConvertMainTargetData(mainTargets);
            this.gridMaintargetList.DataSource = null;
            this.gridMaintargetList.DataSource = dt.DefaultView;
            for (var i = 0; i < this.gridMaintargetList.ColumnCount; i++)
            {
                this.gridMaintargetList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                if (i == 0) this.gridMaintargetList.Columns[i].Width = 160;
                else
                {
                    this.gridMaintargetList.Columns[i].Width = 100;
                }
            }
        }

        void LoadBalanceTargetInfo(string stockCode, int rtype = 0, int type = 1)
        {
            var balanceTargets = Repository.QueryAll<BalanceTargetEntity>($"SECURITYCODE='{stockCode}' and REPORTDATETYPE={rtype} and REPORTTYPE={type}", "REPORTDATE desc", 60);
            var dt = EastMoneyUtil.ConvertBalanceTargetData(balanceTargets);
            this.gridBalanceTargetList.DataSource = null;
            this.gridBalanceTargetList.DataSource = dt.DefaultView;
            for (var i = 0; i < this.gridBalanceTargetList.ColumnCount; i++)
            {
                this.gridBalanceTargetList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                if (i == 0) this.gridBalanceTargetList.Columns[i].Width = 200;
                else
                {
                    this.gridBalanceTargetList.Columns[i].Width = 100;
                }
            }
        }

        void LoadProfitTargetInfo(string stockCode, int rtype = 0, int type = 1)
        {
            var profitTargets = Repository.QueryAll<ProfitTargetEntity>($"SECURITYCODE='{stockCode}' and REPORTDATETYPE={rtype} and REPORTTYPE={type}", "REPORTDATE desc", 60);
            var dt = EastMoneyUtil.ConvertProfitTargetData(profitTargets);
            this.gridProfitTargetList.DataSource = null;
            this.gridProfitTargetList.DataSource = dt.DefaultView;
            for (var i = 0; i < this.gridProfitTargetList.ColumnCount; i++)
            {
                this.gridProfitTargetList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                if (i == 0) this.gridProfitTargetList.Columns[i].Width = 200;
                else
                {
                    this.gridProfitTargetList.Columns[i].Width = 100;
                }
            }
        }

        void LoadCashTargetInfo(string stockCode, int rtype = 0, int type = 1)
        {
            var cashTargets = Repository.QueryAll<CashTargetEntity>($"SECURITYCODE='{stockCode}' and REPORTDATETYPE={rtype} and REPORTTYPE={type}", "REPORTDATE desc", 60);
            var dt = EastMoneyUtil.ConvertCashTargetData(cashTargets);
            this.gridCashTargetList.DataSource = null;
            this.gridCashTargetList.DataSource = dt.DefaultView;
            for (var i = 0; i < this.gridCashTargetList.ColumnCount; i++)
            {
                this.gridCashTargetList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                if (i == 0) this.gridCashTargetList.Columns[i].Width = 250;
                else
                {
                    this.gridCashTargetList.Columns[i].Width = 100;
                }
            }
        }

        private void txtByReport_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtByYear_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtByQuarter_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtByReportOfZCFZB_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtByYearOfZCFZB_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtByReportOfLRB_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtByYearOfLRB_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtByQuarterOfLRB_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtByReportOfXJLLB_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtByYearOfXJLLB_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtByQuarterOfXJLLB_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
