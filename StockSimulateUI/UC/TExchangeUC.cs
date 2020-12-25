using System;
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
using StockSimulateDomain.Utils;

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
