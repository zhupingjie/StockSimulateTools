using StockPriceTools;
using StockSimulateCore.Config;
using StockSimulateCore.Model;
using StockSimulateCore.Service;
using StockSimulateCore.Utils;
using StockSimulateUI.UC;
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
    public partial class StrategyForm : Form
    {
        private SQLiteDBUtil Repository = SQLiteDBUtil.Instance;
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public StrategyForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.txtStockCode.Text = StockCode;
            this.txtStockName.Text = StockName;
        }

        private void txtName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.txtName.Text == "左侧交易")
            {
                var uc = new LeftExchangeUC();
                uc.Dock = DockStyle.Fill;
                uc.StockCode = StockCode;
                uc.StockName = StockName;
                uc.StrategyName = this.txtName.Text;

                if (this.pnlContainer.HasChildren) this.pnlContainer.Controls.Clear();
                this.pnlContainer.Controls.Add(uc);
            }
            else if (this.txtName.Text == "T交易")
            {
                var uc = new TExchangeUC();
                uc.Dock = DockStyle.Fill;
                uc.StockCode = StockCode;
                uc.StockName = StockName;
                uc.StrategyName = this.txtName.Text;

                if (this.pnlContainer.HasChildren) this.pnlContainer.Controls.Clear();
                this.pnlContainer.Controls.Add(uc);
            }
        }

        private void btnCalcuate_Click(object sender, EventArgs e)
        {
            if (!pnlContainer.HasChildren) return;

            var uc = pnlContainer.Controls[0] as BaseUC;
            uc.CalcuateStrategy();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var strategyName = this.txtName.Text;
            if (string.IsNullOrEmpty(strategyName)) return;

            if (!pnlContainer.HasChildren) return;

            var uc = pnlContainer.Controls[0] as BaseUC;
            var strategy = uc.GetStrategyInfo();

            var account = Repository.QueryFirst<AccountEntity>($"RealType='True'");
            if (account == null) return;

            Repository.Delete<StockStrategyEntity>($"StockCode='{StockCode}'");
            Repository.Delete<RemindEntity>($"StockCode='{StockCode}' and (RType={8} or RType={9})");
            var dt = StockStrategyService.MakeStrategyData(strategy);
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var dr = dt.Rows[i];
                var detail = new StockStrategyEntity()
                {
                    StockCode = StockCode,
                    StrategyName = strategyName,
                    Target = dr["操作"].ToString(),
                    Price = ObjectUtil.ToValue<decimal>(dr["收盘价"].ToString(), 0),
                    BuyQty = ObjectUtil.ToValue<int>(dr["买入数"].ToString(), 0),
                    BuyAmount = ObjectUtil.ToValue<decimal>(dr["买入市值"].ToString(), 0),
                    SaleQty = ObjectUtil.ToValue<int>(dr["卖出数"].ToString(), 0),
                    SaleAmount = ObjectUtil.ToValue<decimal>(dr["卖出市值"].ToString(), 0),
                    HoldQty = ObjectUtil.ToValue<int>(dr["持有数"].ToString(), 0),
                    HoldAmount = ObjectUtil.ToValue<decimal>(dr["持有市值"].ToString(), 0),
                    TotalBuyAmount = ObjectUtil.ToValue<decimal>(dr["投入市值"].ToString(), 0),
                    FloatAmount = ObjectUtil.ToValue<decimal>(dr["浮动市值"].ToString(), 0),
                    Cost = ObjectUtil.ToValue<decimal>(dr["成本"].ToString(), 0),
                    Profit = ObjectUtil.ToValue<decimal>(dr["盈亏"].ToString(), 0),
                };
                Repository.Insert<StockStrategyEntity>(detail);

                if (detail.BuyQty > 0 || detail.SaleQty > 0)
                {
                    var remind = new RemindEntity()
                    {
                        StockCode = StockCode,
                        Target = detail.Price,
                        RType = detail.BuyQty > 0 ? 8 : 9,
                        Email = account.Email,
                        QQ = account.QQ,
                        StrategyName = detail.StrategyName,
                        StrategyTarget = detail.Target
                    };
                    remind.MaxPrice = Math.Round(remind.Target * (1 + RunningConfig.Instance.RemindStockPriceFloatPer / 100), 2);
                    remind.MinPrice = Math.Round(remind.Target * (1 - RunningConfig.Instance.RemindStockPriceFloatPer / 100), 2);
                    Repository.Insert<RemindEntity>(remind);
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }

    public class StockPriceInfo
    {
        public decimal CurrentPrice { get; set; }

        public decimal Amount { get; set; }

        public decimal Count
        {
            get
            {
                return Math.Floor(Amount / CurrentPrice);
            }
        }
    }
}
