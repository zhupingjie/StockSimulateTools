﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StockSimulateDomain.Model;
using StockSimulateService.Service;
using StockSimulateDomain.Entity;
using StockSimulateCore.Utils;

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

            var stockStrategys = StockStrategyService.MakeStockStrategys(strategy, false);
            var dt = ObjectUtil.ConvertTable<StockStrategyEntity>(stockStrategys);

            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = dt.DefaultView;
            for (var i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                var columnName = this.dataGridView1.Columns[i].Name;
                this.dataGridView1.Columns[i].Width = ObjectUtil.GetGridColumnLength(columnName);
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
