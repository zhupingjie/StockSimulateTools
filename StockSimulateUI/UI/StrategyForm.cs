using StockPriceTools;
using StockSimulateCore.Model;
using StockSimulateCore.Service;
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

        public StrategyEntity Strategy { get; set; }

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
            decimal maxPer = GetTextValue(this.txtSingleBuyPercent);
            decimal saleRate = GetTextValue(this.txtSaleRate);
            decimal saleHoldPer = GetTextValue(this.txtSaleHoldPer);
            decimal maxDownPer = GetTextValue(this.txtMaxDownPer);
            decimal maxBuyAmount = GetTextValue(this.txtMaxBuyPrice);
            decimal salePrice = GetTextValue(this.txtSalePrice);

            var strategy = new StrategyEntity()
            {
                IncreasePricePer = buyRate,
                IncreaseAmount = buyAmount,
                MaxPositionPer = maxPer,
                IncreaseMorePer = downPercent1,
                IncreaseMoreAmountPer = extraBuyPercent1,
                IncreaseMaxPer = downPercent2,
                IncreaseMaxAmountPer = extraBuyPercent2,
                IncreaseMaxSlidePer = maxDownPer,
                ReducePricePer = saleRate,
                ReducePositionPer = saleHoldPer,
            };
            var dt = StockStrategyService.MakeStrategyData(strategy, lastBuyPrice, baseBuyAmount, salePrice, maxBuyAmount);

            this.dataGridView1.DataSource = dt.DefaultView;
            for(var i=0; i<this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].Width = 80;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }


        decimal GetTextValue(TextBox textBox)
        {
            decimal d = 0;
            decimal.TryParse(textBox.Text, out d);
            return Math.Round(d, 3);
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
                //RemindQQ = this.txtRemindQQ.Text,
                //RemindEmail = this.txtRemindEmail.Text,
                //RemindCount = (int)GetTextValue(this.txtRemindCount),
                //RemindPer = GetTextValue(this.txtRemindPer)
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
