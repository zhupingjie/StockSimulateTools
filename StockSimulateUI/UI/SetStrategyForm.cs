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
    public partial class SetStrategyForm : Form
    {
        private SQLiteDBUtil Repository = SQLiteDBUtil.Instance;
        public string StrategyName { get; set; }
        public string AccountName { get; set; }
        public string StockName { get; set; }
        public string StockCode { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal BuyAmount { get; set; }
        public decimal SalePrice { get; set; }
        public SetStrategyForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.AccountName = txtAccount.Text;
            this.StrategyName = txtStrategy.Text;
            this.BuyPrice = GetTextValue(this.txtBuyPrice);
            this.BuyAmount = GetTextValue(this.txtBuyAmount);
            this.SalePrice = GetTextValue(this.txtSalePrice);

            if (string.IsNullOrEmpty(AccountName)) return;
            if (string.IsNullOrEmpty(StrategyName)) return;

            var strategy = Repository.QueryFirst<StrategyEntity>($"Name='{StrategyName}'");
            if (strategy == null) return;

            var account = Repository.QueryFirst<AccountEntity>($"RealType='True'");
            if (account == null) return;

            var stockStrategy = Repository.QueryFirst<AccountStockEntity>($"StockCode='{StockCode}'");
            if (stockStrategy == null)
            {
                stockStrategy = new AccountStockEntity()
                {
                    AccountName = AccountName,
                    StockCode = StockCode,
                    StockName = StockName,
                    StrategyName = StrategyName,
                    BuyPrice = BuyPrice,
                    BuyAmount = BuyAmount,
                    SalePrice = SalePrice,
                };
                Repository.Insert<AccountStockEntity>(stockStrategy);
            }
            else
            {
                stockStrategy.StockCode = StockCode;
                stockStrategy.StockName = StockName;
                stockStrategy.StrategyName = StrategyName;
                stockStrategy.BuyPrice = BuyPrice;
                stockStrategy.BuyAmount = BuyAmount;
                stockStrategy.SalePrice = SalePrice;
                Repository.Update<AccountStockEntity>(stockStrategy);
            }
            Repository.Delete<StockStrategyEntity>($"StockCode='{StockCode}'");
            Repository.Delete<RemindEntity>($"StockCode='{StockCode}' and (RType={8} or RType={9})");
            var dt = StockStrategyService.MakeStrategyData(strategy, stockStrategy.BuyPrice, stockStrategy.BuyAmount, stockStrategy.SalePrice, account.Amount);
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var dr = dt.Rows[i];
                var detail = new StockStrategyEntity()
                {
                    StockCode = StockCode,
                    StrategyName = strategy.Name,
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
                        Title = detail.Target
                    };
                    remind.MaxPrice = Math.Round(remind.Target * (1 + RunningConfig.Instance.RemindStockPriceFloatPer / 100), 2);
                    remind.MinPrice = Math.Round(remind.Target * (1 - RunningConfig.Instance.RemindStockPriceFloatPer / 100), 2);
                    Repository.Insert<RemindEntity>(remind);
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void SetStrategyForm_Load(object sender, EventArgs e)
        {
            var accounts = Repository.QueryAll<AccountEntity>();
            if (accounts.Length == 0) return;

            this.txtAccount.Items.Clear();
            this.txtAccount.Items.AddRange(accounts.Select(c => c.Name).ToArray());

            var strategys = Repository.QueryAll<StrategyEntity>();
            this.txtStrategy.Items.AddRange(strategys.Select(c=>c.Name).ToArray());
            this.txtStrategy.SelectedIndex = 0;
        }

        decimal GetTextValue(TextBox textBox)
        {
            decimal d = 0;
            decimal.TryParse(textBox.Text, out d);
            return Math.Round(d, 3);
        }
    }
}
