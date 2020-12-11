using StockPriceTools;
using StockSimulateCore.Model;
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
        public StrategyForm()
        {
            InitializeComponent();
        }

        private DataTable DataSource = null;
        public StrategyEntity Strategy { get; set; }

        private void btnCalcuate_Click(object sender, EventArgs e)
        {
            MockData();
            this.dataGridView1.DataSource = this.DataSource.DefaultView;
            for(var i=0; i<this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].Width = 80;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.DataSource = new DataTable("stockprice");
            this.DataSource.Columns.Add("操作");
            this.DataSource.Columns.Add("收盘价");
            this.DataSource.Columns.Add("买入数");
            this.DataSource.Columns.Add("买入市值");
            this.DataSource.Columns.Add("卖出数");
            this.DataSource.Columns.Add("卖出市值");
            this.DataSource.Columns.Add("持有数");
            this.DataSource.Columns.Add("持有市值");
            this.DataSource.Columns.Add("投入市值");
            this.DataSource.Columns.Add("浮动市值");
            this.DataSource.Columns.Add("成本");
            this.DataSource.Columns.Add("盈亏");
        }

        void MockData()
        {
            this.DataSource.Rows.Clear();

            
            decimal baseBuyAmount = GetTextValue(this.txtBaseBuyAmount);
            decimal lastBuyPrice = GetTextValue(this.txtBaseBuyPrice);
            decimal buyRate = GetTextValue(this.txtBuyRate);
            decimal buyAmount = GetTextValue(this.txtBuyAmount);
            decimal buyCount = GetExchangeCount(Math.Floor(baseBuyAmount / lastBuyPrice));
            decimal totalBuyAmount = Math.Round(buyCount * lastBuyPrice, 3);
            decimal hasCount = buyCount;
            decimal totalDownPercent = 0;
            decimal extraBuyPercent1 = GetTextValue(this.txtExtraBuyPercent1);
            decimal extraBuyPercent2 = GetTextValue(this.txtExtraBuyPercent2);
            decimal downPercent1 = GetTextValue(this.txtDownPercent1);
            decimal downPercent2 = GetTextValue(this.txtDownPercent2);
            decimal maxBuyPrice = GetTextValue(this.txtMaxBuyPrice);
            decimal maxPer = GetTextValue(this.txtSingleBuyPercent);
            decimal saleRate = GetTextValue(this.txtSaleRate);
            decimal saleHoldPer = GetTextValue(this.txtSaleHoldPer);
            decimal maxDownPer = GetTextValue(this.txtMaxDownPer);

            decimal downPer = 0;
            decimal cost = lastBuyPrice;
            decimal saleCount = 0;
            decimal saleAmount = 0;
            decimal profit = 0;

            var dr = this.DataSource.NewRow();
            dr["操作"] = $"建仓";
            dr["收盘价"] = $"{lastBuyPrice}";
            dr["买入数"] = $"{buyCount}";
            dr["买入市值"] = $"{totalBuyAmount}";
            dr["卖出数"] = $"{saleCount}";
            dr["卖出市值"] = $"{saleAmount}";
            dr["持有数"] = $"{buyCount}";
            dr["持有市值"] = $"{totalBuyAmount}";
            dr["投入市值"] = $"{totalBuyAmount}";
            dr["浮动市值"] = "0";
            dr["成本"] = "0";
            dr["盈亏"] = "0";

            this.DataSource.Rows.Add(dr);

            while (true)
            {
                var buyPer = totalBuyAmount / maxBuyPrice * 100;
                //单股持股比率超过最大值
                if (buyPer >= maxPer) break;
                //跌幅操作最大值
                if (downPer <= maxDownPer) break;

                decimal thsBuyAmount = buyAmount;
                downPer += buyRate;

                totalDownPercent += Math.Abs(buyRate);
                if(totalDownPercent >= Math.Abs(downPercent1))
                {
                    thsBuyAmount = buyAmount * (1 + extraBuyPercent1/100);
                }
                if(totalDownPercent >= Math.Abs(downPercent2))
                {
                    thsBuyAmount = buyAmount * (1 + extraBuyPercent2 / 100);
                }
                lastBuyPrice = lastBuyPrice * (1 + buyRate / 100);
                buyCount = GetExchangeCount(Math.Floor(thsBuyAmount / lastBuyPrice));
                thsBuyAmount = Math.Round(buyCount * lastBuyPrice,3);

                totalBuyAmount += thsBuyAmount;
                hasCount += buyCount;
                cost = Math.Round(totalBuyAmount / hasCount, 3);

                dr = this.DataSource.NewRow();
                dr["操作"] = $"下跌{downPer}%";
                dr["收盘价"] = $"{Math.Round(lastBuyPrice, 3)}";
                dr["买入数"] = $"{buyCount}";
                dr["买入市值"] = $"{thsBuyAmount}";
                dr["卖出数"] = $"{saleCount}";
                dr["卖出市值"] = $"{saleAmount}";
                dr["持有数"] = $"{hasCount}";
                dr["持有市值"] = $"{Math.Round(lastBuyPrice * hasCount,3)}";
                dr["投入市值"] = $"{totalBuyAmount}";
                dr["浮动市值"] = $"{Math.Round(lastBuyPrice * hasCount - totalBuyAmount, 3)}";
                dr["成本"] = $"{cost}";
                dr["盈亏"] = "0";
                this.DataSource.Rows.Add(dr);
            }

            decimal lastPer = downPer;
            decimal upPer = 0;
            while (true)
            {
                if (lastPer >= Math.Abs(downPer)) break;
                upPer += Math.Abs(buyRate);
                lastPer = downPer + upPer;

                lastBuyPrice = lastBuyPrice * (1 +  -1 * buyRate / 100);
                if(lastPer >= saleRate)
                {
                    saleCount = GetExchangeCount(hasCount * saleHoldPer / 100);
                    hasCount -= saleCount;
                    saleAmount = Math.Round(lastBuyPrice * saleCount, 3);
                    totalBuyAmount -= saleAmount;
                    profit += saleAmount;
                    cost = Math.Round(totalBuyAmount / hasCount, 3);
                }

                dr = this.DataSource.NewRow();
                dr["操作"] = $"上涨{lastPer}%";
                dr["收盘价"] = $"{Math.Round(lastBuyPrice, 3)}";
                dr["买入数"] = $"{0}";
                dr["买入市值"] = $"{0}";
                dr["卖出数"] = $"{saleCount}";
                dr["卖出市值"] = $"{saleAmount}";
                dr["持有数"] = $"{hasCount}";
                dr["持有市值"] = $"{Math.Round(lastBuyPrice * hasCount, 3)}";
                dr["投入市值"] = $"{totalBuyAmount}";
                dr["浮动市值"] = $"{Math.Round(lastBuyPrice * hasCount - totalBuyAmount, 3)}";
                dr["成本"] = $"{cost}";
                dr["盈亏"] = $"{profit}";
                this.DataSource.Rows.Add(dr);
            }
        }

        decimal GetTextValue(TextBox textBox)
        {
            decimal d = 0;
            decimal.TryParse(textBox.Text, out d);
            return Math.Round(d, 3);
        }

        decimal GetExchangeCount(decimal amount)
        {
            var yu = amount % 100;
            if(yu > 50)
            {
                return Math.Floor(amount / 100) * 100 + 100;
            }
            else
            {
                return Math.Floor(amount / 100) * 100;
            }
        }

        private void btnValuatie_Click(object sender, EventArgs e)
        {
            var frm = new VoluatieForm();
            frm.ShowDialog();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Strategy = new StrategyEntity()
            {
                Name = this.txtName.Text,
                IncreasePricePer = GetTextValue(this.txtBuyRate),
                IncreaseAmount = GetTextValue(this.txtBuyAmount),
                MaxPositionPer = GetTextValue(this.txtSingleBuyPercent),
                IncreaseMaxSlidePer = GetTextValue(this.txtMaxDownPer),
                IncreaseMorePer = GetTextValue(this.txtDownPercent1),
                IncreaseMoreAmountPer = GetTextValue(this.txtExtraBuyPercent1),
                IncreaseMaxPer = GetTextValue(this.txtDownPercent2),
                IncreaseMaxAmountPer = GetTextValue(this.txtExtraBuyPercent2),
                ReducePricePer = GetTextValue(this.txtSaleRate),
                ReducePositionPer = GetTextValue(this.txtSaleHoldPer),
                RemindQQ = this.txtRemindQQ.Text,
                RemindEmail = this.txtRemindEmail.Text,
                RemindCount = (int)GetTextValue(this.txtRemindCount),
                RemindPer = GetTextValue(this.txtRemindPer)
            };
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
