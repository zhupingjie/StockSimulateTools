using StockPriceTools;
using StockSimulateCore.Config;
using StockSimulateCore.Model;
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
    public partial class StrategyForm : Form
    {
        private SQLiteDBUtil Repository = SQLiteDBUtil.Instance;
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public StrategyForm()
        {
            InitializeComponent();
        }

        private void btnCalcuate_Click(object sender, EventArgs e)
        {
            decimal baseBuyAmount = GetTextValue(this.txtBaseBuyAmount);
            decimal lastBuyPrice = GetTextValue(this.txtBaseBuyPrice);
            decimal buyRate = GetTextValue(this.txtBuyRate);
            decimal buyAmount = GetTextValue(this.txtBuyAmount);
            decimal extraBuyPercent1 = GetTextValue(this.txtExtraBuyPercent1);
            decimal extraBuyPercent2 = GetTextValue(this.txtExtraBuyPercent2);
            decimal downPercent1 = GetTextValue(this.txtDownPercent1);
            decimal downPercent2 = GetTextValue(this.txtDownPercent2);
            decimal saleRate = GetTextValue(this.txtSaleRate);
            decimal saleHoldPer = GetTextValue(this.txtSaleHoldPer);
            decimal salePrice = GetTextValue(this.txtSalePrice);
            decimal totalBuyAmount = ObjectUtil.ToValue<decimal>(this.txtTotalBuyAmount.Text, 0);

            var strategy = new StrategyEntity()
            {
                IncreasePricePer = buyRate,
                IncreaseAmount = buyAmount,
                IncreaseMorePer = downPercent1,
                IncreaseMoreAmountPer = extraBuyPercent1,
                IncreaseMaxPer = downPercent2,
                IncreaseMaxAmountPer = extraBuyPercent2,
                ReducePricePer = saleRate,
                ReducePositionPer = saleHoldPer,
            };
            var dt = StockStrategyService.MakeStrategyData(strategy, lastBuyPrice, baseBuyAmount, salePrice, totalBuyAmount);

            this.dataGridView1.DataSource = dt.DefaultView;
            for(var i=0; i<this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].Width = 80;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.txtStockCode.Text = StockCode;
            this.txtStockName.Text = StockName;
        }


        decimal GetTextValue(TextBox textBox)
        {
            decimal d = 0;
            decimal.TryParse(textBox.Text, out d);
            return Math.Round(d, 3);
        }

        

        private void btnSave_Click(object sender, EventArgs e)
        {
            string strateyName = this.txtName.Text;
            decimal baseBuyAmount = GetTextValue(this.txtBaseBuyAmount);
            decimal lastBuyPrice = GetTextValue(this.txtBaseBuyPrice);
            decimal buyRate = GetTextValue(this.txtBuyRate);
            decimal buyAmount = GetTextValue(this.txtBuyAmount);
            decimal extraBuyPercent1 = GetTextValue(this.txtExtraBuyPercent1);
            decimal extraBuyPercent2 = GetTextValue(this.txtExtraBuyPercent2);
            decimal downPercent1 = GetTextValue(this.txtDownPercent1);
            decimal downPercent2 = GetTextValue(this.txtDownPercent2);
            decimal saleRate = GetTextValue(this.txtSaleRate);
            decimal saleHoldPer = GetTextValue(this.txtSaleHoldPer);
            decimal salePrice = GetTextValue(this.txtSalePrice);
            decimal totalBuyAmount = ObjectUtil.ToValue<decimal>(this.txtTotalBuyAmount.Text, 0);

            var strategy = new StrategyEntity()
            {
                IncreasePricePer = buyRate,
                IncreaseAmount = buyAmount,
                IncreaseMorePer = downPercent1,
                IncreaseMoreAmountPer = extraBuyPercent1,
                IncreaseMaxPer = downPercent2,
                IncreaseMaxAmountPer = extraBuyPercent2,
                ReducePricePer = saleRate,
                ReducePositionPer = saleHoldPer,

            };
            if (string.IsNullOrEmpty(strateyName)) return;

            var account = Repository.QueryFirst<AccountEntity>($"RealType='True'");
            if (account == null) return;

            Repository.Delete<StockStrategyEntity>($"StockCode='{StockCode}'");
            Repository.Delete<RemindEntity>($"StockCode='{StockCode}' and (RType={8} or RType={9})");
            var dt = StockStrategyService.MakeStrategyData(strategy, lastBuyPrice, buyAmount, salePrice, totalBuyAmount);
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var dr = dt.Rows[i];
                var detail = new StockStrategyEntity()
                {
                    StockCode = StockCode,
                    StrategyName = strateyName,
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

        private void txtName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.txtName.Text == "左侧交易")
            {

            }
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
