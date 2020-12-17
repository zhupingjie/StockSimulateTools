using StockSimulateCore.Config;
using StockSimulateCore.Entity;
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
    public partial class RemindForm : Form
    {
        private SQLiteDBUtil Repository = SQLiteDBUtil.Instance;
        public string UDPer { get; set; }
        public string UpPrice { get; set; }
        public string DownPrice { get; set; }

        public string StockCode { get; set; }

        public RemindForm()
        {
            InitializeComponent();
        }

        private void SetRemindForm_Load(object sender, EventArgs e)
        {
            var accounts = Repository.QueryAll<AccountEntity>();
            if (accounts.Length == 0) return;

            this.txtAccount.Items.Clear();
            this.txtAccount.Items.AddRange(accounts.Select(c => c.Name).ToArray());
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.UDPer = this.txtUDPer.Text;
            this.UpPrice = this.txtUpPrice.Text;
            this.DownPrice = this.txtDownPrice.Text;

            var accountName = this.txtAccount.Text;
            var account = Repository.QueryFirst<AccountEntity>($"Name='{accountName}'");
            if (account == null) return;

            if (!string.IsNullOrEmpty(this.UDPer))
            {
                var udPers = ObjectUtil.GetSplitArray(this.UDPer, ",");
                foreach (var udPer in udPers)
                {
                    var target = ObjectUtil.ToValue<decimal>(udPer, 0);
                    if (target == 0) continue;

                    var remind = Repository.QueryFirst<RemindEntity>($"StockCode='{StockCode}' and Target={udPer} and RType=0");
                    if (remind == null)
                    {
                        remind = new RemindEntity()
                        {
                            StockCode = StockCode,
                            Target = target,
                            Email = account.Email,
                            QQ = account.QQ,
                            RType = 0,
                            StrategyName = "订阅",
                            StrategyTarget = $"波动{target}%"
                        };
                        Repository.Insert<RemindEntity>(remind);
                    }
                }
            }
            if (!string.IsNullOrEmpty(this.UpPrice))
            {
                var upPrices = ObjectUtil.GetSplitArray(this.UpPrice, ",");
                foreach (var upPrice in upPrices)
                {
                    var target = ObjectUtil.ToValue<decimal>(upPrice, 0);
                    if (target == 0) continue;

                    var remind = Repository.QueryFirst<RemindEntity>($"StockCode='{StockCode}' and Target={upPrice} and RType=1");
                    if (remind == null)
                    {
                        remind = new RemindEntity()
                        {
                            StockCode = StockCode,
                            Target = target,
                            Email = account.Email,
                            QQ = account.QQ,
                            RType = 0,
                            StrategyName = "订阅",
                            StrategyTarget = $"上涨至{target}"
                        };
                        remind.MaxPrice = Math.Round(remind.Target * (1 + RunningConfig.Instance.RemindStockPriceFloatPer / 100m), 2);
                        remind.MinPrice = Math.Round(remind.Target * (1 - RunningConfig.Instance.RemindStockPriceFloatPer / 100m), 2);
                        Repository.Insert<RemindEntity>(remind);
                    }
                }
            }
            if (!string.IsNullOrEmpty(this.DownPrice))
            {
                var downPrices = ObjectUtil.GetSplitArray(this.DownPrice, ",");
                foreach (var downPrice in downPrices)
                {
                    var target = ObjectUtil.ToValue<decimal>(downPrice, 0);
                    if (target == 0) continue;

                    var remind = Repository.QueryFirst<RemindEntity>($"StockCode='{StockCode}' and Target={downPrice} and RType=1");
                    if (remind == null)
                    {
                        remind = new RemindEntity()
                        {
                            StockCode = StockCode,
                            Target = target,
                            Email = account.Email,
                            QQ = account.QQ,
                            RType = 2,
                            StrategyName = "订阅",
                            StrategyTarget = $"下跌至{target}"
                        };
                        remind.MaxPrice = Math.Round(remind.Target * (1 + RunningConfig.Instance.RemindStockPriceFloatPer / 100m), 2);
                        remind.MinPrice = Math.Round(remind.Target * (1 - RunningConfig.Instance.RemindStockPriceFloatPer / 100m), 2);
                        Repository.Insert<RemindEntity>(remind);
                    }
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
