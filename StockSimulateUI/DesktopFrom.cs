using StockPriceTools.UI;
using StockSimulateCore.Config;
using StockSimulateCore.Model;
using StockSimulateCore.Service;
using StockSimulateCore.Utils;
using StockSimulateUI.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockPriceTools
{
    public partial class DesktopFrom : Form
    {
        private SQLiteDBUtil Repository = SQLiteDBUtil.Instance;
        private CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
        private RunningConfig RC = RunningConfig.Instance;
        public DesktopFrom()
        {
            InitializeComponent();
        }

        #region 页面加载事件
        private void DesktopFrom_Load(object sender, EventArgs e)
        {
            this.LoadData();

            this.GatherData();

            this.RemindData();
        }

        void LoadData()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Action act = delegate ()
                    {
                        this.LoadStockList();
                        this.LoadStockStrategyList();
                    };
                    this.Invoke(act);
                    this.ActionLog("已加载自选股及策略股数据...");
                    Thread.Sleep(RC.LoadStockDataInterval * 1000);
                }
            });
        }

        void GatherData()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var stocks = Repository.QueryAll<StockEntity>();
                    foreach (var stock in stocks)
                    {
                        var stockInfo = EastMoneyUtil.GetStockPrice(stock.Code);
                        if (stockInfo == null) return;

                        stockInfo.Stock.ID = stock.ID;
                        Repository.Update<StockEntity>(stockInfo.Stock);

                        var price = Repository.QueryFirst<StockPriceEntity>($"StockCode='{stock.Code}' and DealDate='{DateTime.Now.ToString("yyyy-MM-dd")}'");
                        if (price == null)
                        {
                            Repository.Insert<StockPriceEntity>(stockInfo.Price);
                        }
                        else
                        {
                            stockInfo.Price.ID = price.ID;
                            Repository.Update<StockPriceEntity>(stockInfo.Price);
                        }
                        this.ActionLog($"已采集[{stock.Name}]今日股价数据...[{stockInfo.Price.Price}] [{stockInfo.Price.UDPer}%]");
                    }
                    Thread.Sleep(RC.GatherStockPriceInterval * 1000);
                }
            }, CancellationTokenSource.Token);

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var stocks = Repository.QueryAll<StockEntity>();
                    foreach (var stock in stocks)
                    {
                        var mainTargetInfos = EastMoneyUtil.GetMainTargets(stock.Code, 0);
                        if (mainTargetInfos.Length > 0)
                        {
                            var dates = mainTargetInfos.Select(c => c.Date).Distinct().ToArray();
                            var mts = Repository.QueryAll<MainTargetEntity>($"StockCode='{stock.Code}' and Rtype=0 and Date in ('{string.Join("','", dates)}')");
                            var mtDates = mts.Select(c => c.Date).Distinct().ToArray();
                            var newMts = mainTargetInfos.Where(c => !mtDates.Contains(c.Date)).ToArray();
                            if (newMts.Length > 0)
                            {
                                Repository.Insert<MainTargetEntity>(newMts);
                            }
                        }
                        mainTargetInfos = EastMoneyUtil.GetMainTargets(stock.Code, 1);
                        if (mainTargetInfos.Length > 0)
                        {
                            var dates = mainTargetInfos.Select(c => c.Date).Distinct().ToArray();
                            var mts = Repository.QueryAll<MainTargetEntity>($"StockCode='{stock.Code}' and Rtype=1 and Date in ('{string.Join("','", dates)}')");
                            var mtDates = mts.Select(c => c.Date).Distinct().ToArray();
                            var newMts = mainTargetInfos.Where(c => !mtDates.Contains(c.Date)).ToArray();
                            if (newMts.Length > 0)
                            {
                                Repository.Insert<MainTargetEntity>(newMts);
                            }
                        }
                        mainTargetInfos = EastMoneyUtil.GetMainTargets(stock.Code, 2);
                        if (mainTargetInfos.Length > 0)
                        {
                            var dates = mainTargetInfos.Select(c => c.Date).Distinct().ToArray();
                            var mts = Repository.QueryAll<MainTargetEntity>($"StockCode='{stock.Code}' and Rtype=2 and Date in ('{string.Join("','", dates)}')");
                            var mtDates = mts.Select(c => c.Date).Distinct().ToArray();
                            var newMts = mainTargetInfos.Where(c => !mtDates.Contains(c.Date)).ToArray();
                            if (newMts.Length > 0)
                            {
                                Repository.Insert<MainTargetEntity>(newMts);
                            }
                        }
                        this.ActionLog($"已采集[{stock.Name}]主要指标数据...");
                    }
                    Thread.Sleep(RC.GatherStockMainTargetInterval * 1000);
                }
            }, CancellationTokenSource.Token);
        }

        void RemindData()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var stockStrategyDetails = Repository.QueryAll<StockStrategyDetailEntity>();
                    var stockCodes = stockStrategyDetails.Select(c => c.StockCode).Distinct().ToArray();
                    var stocks = Repository.QueryAll<StockEntity>($"Code in ('{string.Join("','", stockCodes)}')");

                    foreach (var stock in stocks)
                    {
                        var stockStrategyDetail = stockStrategyDetails.FirstOrDefault(c => c.StockCode == stock.Code && c.BuyQty > 0 && c.MaxPrice >= stock.Price && c.MinPrice <= stock.Price && !c.Execute);
                        if (stockStrategyDetail == null) continue;

                        var message = $"[{stock.Name}]当前股价[{stock.Price} | {stock.UDPer}%]已达成买卖点[{stockStrategyDetail.Target}({stockStrategyDetail.MinPrice}-{stockStrategyDetail.MaxPrice})],请注意!";
                        this.ShowMessage(message);
                        this.ActionLog(message);
                    }
                    Thread.Sleep(RC.RemindStockStrategyInterval * 1000);
                }
            }, CancellationTokenSource.Token);
        }
   

        #endregion

        #region 加载GridList数据

        void LoadStockList()
        {
            var stocks = Repository.QueryAll<StockEntity>();
            var dt = ObjectUtil.ConvertTable(stocks);
            this.gridStockList.DataSource = dt.DefaultView;
            for(var i=0; i<this.gridStockList.ColumnCount; i++)
            {
                var length = this.gridStockList.Columns[i].Name.Length;
                this.gridStockList.Columns[i].Width = length < 6 ? 80 : length < 8 ? 120 : 140;
            }
            for(var i=0; i<this.gridStockList.Rows.Count; i++)
            {
                var row = this.gridStockList.Rows[i];
                var value = ObjectUtil.ToValue<decimal>(row.Cells["浮动(%)"].Value, 0);
                if(value > 0)
                {
                    this.gridStockList.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                }
                else
                {
                    this.gridStockList.Rows[i].DefaultCellStyle.ForeColor = Color.Green;
                }
            }
        }

        void LoadStockStrategyList()
        {
            var accountStocks = Repository.QueryAll<StockStrategyEntity>();
            var dt = ObjectUtil.ConvertTable(accountStocks);
            this.gridStockStrategyList.DataSource = dt.DefaultView;
            for (var i = 0; i < this.gridStockStrategyList.ColumnCount; i++)
            {
                var length = this.gridStockStrategyList.Columns[i].Name.Length;
                this.gridStockStrategyList.Columns[i].Width = length < 6 ? 80 : length < 8 ? 120 : 140;
            }
        }
        
        void LoadPriceList(string stockCode)
        {
            var stockPrices = Repository.QueryAll<StockPriceEntity>($"StockCode='{stockCode}'", "DealDate desc",  60);
            var dt = ObjectUtil.ConvertTable(stockPrices);
            this.gridPriceList.DataSource = dt.DefaultView;
            for (var i = 0; i < this.gridPriceList.ColumnCount; i++)
            {
                var columnName = this.gridPriceList.Columns[i].Name;
                if (columnName == "股票代码") this.gridPriceList.Columns[i].Visible = false;
                else
                {
                    var length = columnName.Length;
                    this.gridPriceList.Columns[i].Width = length < 6 ? 80 : length < 8 ? 120 : 140;
                }
            }
            for (var i = 0; i < this.gridPriceList.Rows.Count; i++)
            {
                var row = this.gridPriceList.Rows[i];
                var value = ObjectUtil.ToValue<decimal>(row.Cells["浮动(%)"].Value, 0);
                if (value > 0)
                {
                    this.gridPriceList.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                }
                else
                {
                    this.gridPriceList.Rows[i].DefaultCellStyle.ForeColor = Color.Green;
                }
            }
        }

        void LoadExchangeList(string stockCode)
        {
            var exchangeOrders = Repository.QueryAll<ExchangeOrderEntity>($"StockCode='{stockCode}'", "ExchangeTime desc", 60);
            var dt = ObjectUtil.ConvertTable(exchangeOrders);
            this.gridExchangeList.DataSource = dt.DefaultView;
            for (var i = 0; i < this.gridExchangeList.ColumnCount; i++)
            {
                var columnName = this.gridExchangeList.Columns[i].Name;
                if (columnName == "股票代码") this.gridExchangeList.Columns[i].Visible = false;
                else
                {
                    var length = columnName.Length;
                    this.gridExchangeList.Columns[i].Width = length < 6 ? 80 : length < 8 ? 120 : 140;
                }
            }
        }

        void LoadMainTargetInfo(string stockCode)
        {
            var mainTargets = Repository.QueryAll<MainTargetEntity>($"StockCode='{stockCode}' and Rtype=0", "", 60);
            //var dt = ObjectUtil.ConvertTable(mainTargets);
            var dt = EastMoneyUtil.ConvertMainTargetData(mainTargets);
            this.gridMaintargetList.DataSource = dt.DefaultView;
            for (var i = 0; i < this.gridMaintargetList.ColumnCount; i++)
            {
                if (i == 0) this.gridMaintargetList.Columns[i].Width = 160;
                else
                {
                    this.gridMaintargetList.Columns[i].Width = 100;
                }
            }
        }

        void LoadStockStrategyDetailList(string stockCode)
        {
            var strategyDetails = Repository.QueryAll<StockStrategyDetailEntity>($"StockCode='{stockCode}'");
            var dt = ObjectUtil.ConvertTable(strategyDetails);
            this.gridStockStrategyDetailList.DataSource = dt.DefaultView;
            for (var i = 0; i < this.gridStockStrategyDetailList.ColumnCount; i++)
            {
                var columnName = this.gridStockStrategyDetailList.Columns[i].Name;
                if (columnName == "股票代码") this.gridStockStrategyDetailList.Columns[i].Visible = false;
                else
                {
                    var length = columnName.Length;
                    this.gridStockStrategyDetailList.Columns[i].Width = length < 6 ? 80 : length < 8 ? 120 : 140;
                }
            }
            for (var i = 0; i < this.gridStockStrategyDetailList.Rows.Count; i++)
            {
                var row = this.gridStockStrategyDetailList.Rows[i];
                var value = ObjectUtil.ToValue<bool>(row.Cells["执行策略"].Value, false);
                if (value)
                {
                    this.gridStockStrategyDetailList.Rows[i].DefaultCellStyle.ForeColor = Color.Green;
                }
                else
                {
                    this.gridStockStrategyDetailList.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
                }
            }
        }

        #endregion

        #region 加载ListView数据
        void LoadStockBaseInfo(string stockCode)
        {
            this.lstBaseInfo.Items.Clear();

            var stock = Repository.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if (stock == null) return;

            var preps = typeof(StockEntity).GetProperties();
            foreach(var prep in preps)
            {
                if (prep.Name == "ID") continue;

                var desc = prep.Name;
                var attr = prep.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault() as DescriptionAttribute;
                if (attr != null) desc = attr.Description;

                var listViewItem = new ListViewItem();
                listViewItem.Text = $"{desc}";
                listViewItem.SubItems.Add($"{ObjectUtil.GetPropertyValue(stock, prep.Name)}");
                this.lstBaseInfo.Items.Add(listViewItem);
            }
            //foreach (DataGridViewCell cell in selectRow.Cells)
            //{
            //    var listViewItem = new ListViewItem();
            //    listViewItem.Text = $"{this.gridStockList.Columns[cell.ColumnIndex].Name}";
            //    listViewItem.SubItems.Add($"{selectRow.Cells[cell.ColumnIndex].Value}");
            //    this.lstBaseInfo.Items.Add(listViewItem);
            //}
        }

        void LoadStockStrategyInfo(string stockCode)
        {
            this.lstStrategyInfo.Items.Clear();

            var accountStock = Repository.QueryFirst<StockStrategyEntity>($"StockCode='{stockCode}'");
            if (accountStock == null) return;

            var strategy = Repository.QueryFirst<StrategyEntity>($"Name='{accountStock.StrategyName}'");
            if (strategy == null) return;

            var preps = typeof(StrategyEntity).GetProperties();
            foreach (var prep in preps)
            {
                if (prep.Name == "ID") continue;

                var desc = prep.Name;
                var attr = prep.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault() as DescriptionAttribute;
                if (attr != null) desc = attr.Description;

                var listViewItem = new ListViewItem();
                listViewItem.Text = $"{desc}";
                listViewItem.SubItems.Add($"{ObjectUtil.GetPropertyValue(strategy, prep.Name)}");
                this.lstStrategyInfo.Items.Add(listViewItem);
            }
        }

        #endregion

        #region GridList事件
        /// <summary>
        /// 自选股列表点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            this.lstBaseInfo.Items.Clear();

            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";
            this.LoadStockBaseInfo(stockCode);
            this.LoadStockStrategyInfo(stockCode);
            //this.LoadPriceList(stockCode);
            //this.LoadExchangeList(stockCode);
            //this.LoadMainTargetInfo(stockCode);
        }

        /// <summary>
        /// 历史股价列表点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridPriceList_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (this.gridPriceList.SelectedRows.Count == 0) return;
            this.lstPriceInfo.Items.Clear();

            var selectRow = this.gridPriceList.SelectedRows[0];
            foreach (DataGridViewCell cell in selectRow.Cells)
            {
                var listViewItem = new ListViewItem();
                listViewItem.Text = $"{this.gridPriceList.Columns[cell.ColumnIndex].Name}";
                listViewItem.SubItems.Add($"{selectRow.Cells[cell.ColumnIndex].Value}");
                this.lstPriceInfo.Items.Add(listViewItem);
            }
        }

        /// <summary>
        /// 交易数据列表点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void gridExchangeList_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (this.gridExchangeList.SelectedRows.Count == 0) return;
            this.lstExchangeInfo.Items.Clear();

            var selectRow = this.gridExchangeList.SelectedRows[0];
            foreach (DataGridViewCell cell in selectRow.Cells)
            {
                var listViewItem = new ListViewItem();
                listViewItem.Text = $"{this.gridExchangeList.Columns[cell.ColumnIndex].Name}";
                listViewItem.SubItems.Add($"{selectRow.Cells[cell.ColumnIndex].Value}");
                this.lstExchangeInfo.Items.Add(listViewItem);
            }

        }

        #endregion

        #region 工具栏按钮事件
        private void btnAddStock_Click(object sender, EventArgs e)
        {
            var stockCode = string.Empty;
            var dlg = new NewStockForm();
            if (dlg.ShowDialog() == DialogResult.OK) stockCode = dlg.StockCode;
            if (string.IsNullOrEmpty(stockCode)) return;

            var stock = Repository.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if (stock == null)
            {
                stock = new StockEntity()
                {
                    Code = stockCode
                };
                Repository.Insert<StockEntity>(stock);
            }
            this.LoadStockList();
        }
        private void btnInitialize_Click(object sender, EventArgs e)
        {
            Repository.InitSQLiteDB();
        }

        private void btnAddStrategy_Click(object sender, EventArgs e)
        {
            var frm = new StrategyListForm();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();
        }

        private void btnAccountInfo_Click(object sender, EventArgs e)
        {
            var frm = new AccountForm();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void btnSetStrategy_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";
            var stockName = $"{selectRow.Cells["股票名称"].Value}";

            var frm = new SetStrategyForm();
            frm.StartPosition = FormStartPosition.CenterParent;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var strategy = Repository.QueryFirst<StrategyEntity>($"Name='{frm.StrategyName}'");
                if (strategy == null) return;

                var account = Repository.QueryFirst<AccountEntity>();
                if (account == null) return;

                var stockStrategy = Repository.QueryFirst<StockStrategyEntity>($"StockCode='{stockCode}'");
                if (stockStrategy == null)
                {
                    stockStrategy = new StockStrategyEntity()
                    {
                        StockCode = stockCode,
                        StockName = stockName,
                        StrategyName = frm.StrategyName,
                        BuyPrice = frm.BuyPrice,
                        BuyAmount = frm.BuyAmount,
                        SalePrice = frm.SalePrice,
                    };
                    Repository.Insert<StockStrategyEntity>(stockStrategy);
                }
                else
                {
                    stockStrategy.StockCode = stockCode;
                    stockStrategy.StockName = stockName;
                    stockStrategy.StrategyName = frm.StrategyName;
                    stockStrategy.BuyPrice = frm.BuyPrice;
                    stockStrategy.BuyAmount = frm.BuyAmount;
                    stockStrategy.SalePrice = frm.SalePrice;
                    Repository.Update<StockStrategyEntity>(stockStrategy);
                }
                Repository.Delete<StockStrategyDetailEntity>($"StockCode='{stockCode}'");

                var dt = StockStrategyService.MakeStrategyData(strategy, stockStrategy.BuyPrice, stockStrategy.BuyAmount, stockStrategy.SalePrice, account.Amount);
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var dr = dt.Rows[i];
                    var detail = new StockStrategyDetailEntity()
                    {
                        StockCode = stockCode,
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
                    detail.MaxPrice = Math.Round(detail.Price * 1.02m, 2);
                    detail.MinPrice = Math.Round(detail.Price * 0.98m, 2);
                    Repository.Insert<StockStrategyDetailEntity>(detail);
                }
                this.LoadStockStrategyList();
            }
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            SQLiteDBUtil.Instance.InitSQLiteDB();
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            var frm = new ConfigForm();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }


        #endregion

        #region TabControl事件

        private void tabControlBottom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            switch (this.tabControlBottom.SelectedIndex)
            {
                case 1:
                    this.LoadPriceList(stockCode);
                    break;
                case 2:
                    this.LoadStockStrategyDetailList(stockCode);
                    break;
                case 3:
                    this.LoadExchangeList(stockCode);
                    break;
                case 4:
                    this.LoadMainTargetInfo(stockCode);
                    break;
            }
        }

        #endregion

        #region 操作日志事件

        public void ActionLog(string message)
        {
            Action act = delegate ()
            {
                this.txtActionLog.AppendText($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {message}\n");
                this.txtActionLog.SelectionStart = this.txtActionLog.Text.Length;
                this.txtActionLog.ScrollToCaret();
            };
            this.Invoke(act);
        }

        public void ShowMessage(string message)
        {
            Action act = delegate ()
            {
                this.lblMessage.Text = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {message}";
            };
            this.Invoke(act);
        }
        #endregion
    }
}
