using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StockSimulateCore.Utils;
using StockSimulateCore.Model;
using StockSimulateCore.Service;
using StockSimulateCore.Config;
using StockSimulateCore.Entity;

namespace StockSimulateUI.UC
{
    public partial class LeftExchangeUC : BaseUC
    {
        public LeftExchangeUC()
        {
            InitializeComponent();
        }

        public override void CalcuateStrategy(string accountName, string strategyName, string stockCode)
        {
            var strategy = this.GetStrategyInfo() as LeftExchangeStrategyInfo;
            if (strategy.BuyPrice == 0 || strategy.BuyAmount == 0) return;
            strategy.AccountName = accountName;
            strategy.StockCode = stockCode;
            strategy.Name = strategyName;

            var stockStrategys = StockStrategyService.MakeStockStrategys(strategy);
            var dt = ObjectUtil.ConvertTable<StockStrategyEntity>(stockStrategys);

            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = dt.DefaultView;
            for (var i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                var columnName = this.dataGridView1.Columns[i].Name;
                var length = columnName.Length;
                this.dataGridView1.Columns[i].Width = length < 6 ? 80 : length < 8 ? 120 : 140;
            }
            for (var i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                var row = this.dataGridView1.Rows[i];
                var value = ObjectUtil.ToValue<int>(row.Cells["执行策略"].Value, -1);
                this.dataGridView1.Rows[i].Cells["执行策略"].Value = (value == 1 ? "自动模拟交易" : value == 0 ? "买卖点提醒" : "");

                var mode = ObjectUtil.ToValue<int>(row.Cells["条件"].Value, -1);
                this.dataGridView1.Rows[i].Cells["条件"].Value = (mode == 0 ? "低于" : mode == 1 ? "高于" : mode == 2 ? "等待" : "");

                var ok = ObjectUtil.ToValue<int>(row.Cells["执行结果"].Value, -1);
                this.dataGridView1.Rows[i].Cells["执行结果"].Value = (ok == 1 ? "成功" : ok == 2 ? "失败" : ok == 0 ? "等待" : "");
            }
        }

        public override StrategyInfo GetStrategyInfo()
        {
            decimal buyAmount = ObjectUtil.ToValue<decimal>(this.txtBaseBuyAmount.Text, 0);
            decimal buyPrice = ObjectUtil.ToValue<decimal>(this.txtBaseBuyPrice.Text, 0);
            decimal salePrice = ObjectUtil.ToValue<decimal>(this.txtSalePrice.Text, 0);
            decimal totalBuyAmount = ObjectUtil.ToValue<decimal>(this.txtTotalBuyAmount.Text, 0);
            decimal increasePricePer = ObjectUtil.ToValue<decimal>(this.txtBuyRate.Text, 0);
            decimal increaseAmount = ObjectUtil.ToValue<decimal>(this.txtBuyAmount.Text, 0);
            decimal extraBuyPercent1 = ObjectUtil.ToValue<decimal>(this.txtExtraBuyPercent1.Text, 0);
            decimal extraBuyPercent2 = ObjectUtil.ToValue<decimal>(this.txtExtraBuyPercent2.Text, 0);
            decimal downPercent1 = ObjectUtil.ToValue<decimal>(this.txtDownPercent1.Text, 0);
            decimal downPercent2 = ObjectUtil.ToValue<decimal>(this.txtDownPercent2.Text, 0);
            decimal reducePricePer = ObjectUtil.ToValue<decimal>(this.txtSaleRate.Text, 0);
            decimal reducePositionPer = ObjectUtil.ToValue<decimal>(this.txtSaleHoldPer.Text, 0);

            return new LeftExchangeStrategyInfo()
            {
                BuyPrice = buyPrice,
                BuyAmount = buyAmount,
                SalePrice = salePrice,
                TotalBuyAmount = totalBuyAmount,
                IncreasePricePer = increasePricePer,
                IncreaseAmount = increaseAmount,
                IncreaseMorePer = downPercent1,
                IncreaseMoreAmountPer = extraBuyPercent1,
                IncreaseMaxPer = downPercent2,
                IncreaseMaxAmountPer = extraBuyPercent2,
                ReducePricePer = reducePricePer,
                ReducePositionPer = reducePositionPer,
            };
        }
    }
}
