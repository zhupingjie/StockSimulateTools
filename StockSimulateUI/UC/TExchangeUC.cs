using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StockSimulateCore.Model;
using StockSimulateCore.Service;
using StockSimulateCore.Utils;
using StockSimulateCore.Entity;

namespace StockSimulateUI.UC
{
    public partial class TExchangeUC : BaseUC
    {
        public TExchangeUC()
        {
            InitializeComponent();
        }

        public override void CalcuateStrategy(string accountName, string strategyName, string stockCode)
        {
            var strategy = this.GetStrategyInfo() as TExchangeStrategyInfo;
            if (strategy.BasePrice == 0 || strategy.HoldQty == 0) return;
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
            decimal basePrice = ObjectUtil.ToValue<decimal>(this.txtBasePrice.Text, 0);
            int holdQty = ObjectUtil.ToValue<int>(this.txtHoldQty.Text, 0);
            decimal downPer = ObjectUtil.ToValue<decimal>(this.txtDownPer.Text, 0);
            int bSQty = ObjectUtil.ToValue<int>(this.txtBSQty.Text, 0);
            int lockQty = ObjectUtil.ToValue<int>(this.txtLockQty.Text, 0);
            decimal upPer = ObjectUtil.ToValue<decimal>(this.txtUpPer.Text, 0);
            int maxSingleBS  = ObjectUtil.ToValue<int>(this.txtMaxSingleBS.Text, 0);
            int maxErrorMatch = ObjectUtil.ToValue<int>(this.txtMaxErrorMatch.Text, 0);
            decimal maxPriceStop = ObjectUtil.ToValue<decimal>(this.txtMaxPriceStop.Text, 0);
            decimal minPriceStop = ObjectUtil.ToValue<decimal>(this.txtMinPriceStop.Text, 0);

            return new TExchangeStrategyInfo()
            {
                BasePrice = basePrice,
                HoldQty = holdQty,
                ReducePricePer = downPer,
                IncreasePricePer = upPer,
                BSQty = bSQty,
                LockQty = lockQty,
                MaxSingleBS = maxSingleBS,
                MaxErrorMatch = maxErrorMatch,
                MaxPriceStop = maxPriceStop,
                MinPriceStop = minPriceStop
            };
        }
    }
}
