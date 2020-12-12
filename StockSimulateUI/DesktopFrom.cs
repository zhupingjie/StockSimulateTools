using StockPriceTools.UI;
using StockSimulateCore.Model;
using StockSimulateCore.Utils;
using StockSimulateUI.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockPriceTools
{
    public partial class DesktopFrom : Form
    {
        private SQLiteDBUtil Repository = SQLiteDBUtil.Instance;
        public DesktopFrom()
        {
            InitializeComponent();
        }

        private void btnCalcuate_Click(object sender, EventArgs e)
        {
            var frm = new StrategyForm();
            frm.Show();
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            SQLiteDBUtil.Instance.InitSQLiteDB();
        }

        private void btnAddStock_Click(object sender, EventArgs e)
        {
            var stockCode = string.Empty;
            var dlg = new NewStockForm();
            if(dlg.ShowDialog() == DialogResult.OK) stockCode = dlg.StockCode;
            if (string.IsNullOrEmpty(stockCode)) return;

            var stock = Repository.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if(stock == null)
            {
                stock = new StockEntity()
                {
                    Code = stockCode
                };
                Repository.Insert<StockEntity>(stock);
            }
            this.LoadStockList();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadStockList();
        }

        void LoadStockList()
        {
            var where = string.Empty;
            if(!string.IsNullOrEmpty(this.txtStockCode.Text) && this.txtStockCode.Text != "股票代码")
            {
                where = $"Code like '%{this.txtStockCode.Text}%'";
            }
            var stocks = Repository.QueryAll<StockEntity>(where);
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

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            this.lstBaseInfo.Items.Clear();

            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";
            this.LoadStockBaseInfo(stockCode);
            this.LoadStockStrategyInfo(stockCode);
            this.LoadPriceList(stockCode);
            //this.LoadExchangeList(stockCode);
            //this.LoadMainTargetInfo(stockCode);
        }

        private void btnGather_Click(object sender, EventArgs e)
        {
            var stocks = Repository.QueryAll<StockEntity>();
            foreach (var stock in stocks)
            {
                var stockInfo = EastMoneyUtil.GetStockPrice(stock.Code);
                if (stockInfo == null) return;
                //if (stockInfo.Price.LastDate.DayOfWeek == DayOfWeek.Saturday || stockInfo.Price.LastDate.DayOfWeek == DayOfWeek.Sunday) continue;

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
            }
            this.LoadStockList();
        }

        private void btnInitialize_Click(object sender, EventArgs e)
        {
            Repository.InitSQLiteDB();
        }

        private void DesktopFrom_Load(object sender, EventArgs e)
        {
            LoadStockList();
            LoadStockStrategyList();
        }

        private void btnAddStrategy_Click(object sender, EventArgs e)
        {
            var frm = new StrategyListForm();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();
        }

        private void txtStockCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAccountInfo_Click(object sender, EventArgs e)
        {
            var frm = new AccountForm();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

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

        private void tabControlBottom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            switch(this.tabControlBottom.SelectedIndex)
            {
                case 0:
                    this.LoadPriceList(stockCode);
                    break;
                case 1:
                    this.LoadExchangeList(stockCode);
                    break;
                case 2:
                    this.LoadMainTargetInfo(stockCode);
                    break;
            }
        }

        private void btnSetStrategy_Click(object sender, EventArgs e)
        {
            if (this.gridStockList.SelectedRows.Count == 0) return;
            var selectRow = this.gridStockList.SelectedRows[0];
            var stockCode = $"{selectRow.Cells["股票代码"].Value}";

            var frm = new SetStrategyForm();
            frm.StartPosition = FormStartPosition.CenterParent;
            if(frm.ShowDialog() == DialogResult.OK)
            {
                var stockStrategy = Repository.QueryFirst<StockStrategyEntity>($"StockCode='{stockCode}'");
                if(stockStrategy == null)
                {
                    stockStrategy = new StockStrategyEntity()
                    {
                        StockCode = stockCode,
                        StrategyName = frm.StrategyName,
                        BuyPrice = frm.BuyPrice,
                        BuyAmount = frm.BuyAmount,
                        SalePrice = frm.SalePrice,
                    };
                    Repository.Insert<StockStrategyEntity>(stockStrategy);


                }
                else
                {
                    stockStrategy.StrategyName = frm.StrategyName;
                    stockStrategy.BuyPrice = frm.BuyPrice;
                    stockStrategy.BuyAmount = frm.BuyAmount;
                    stockStrategy.SalePrice = frm.SalePrice;
                    Repository.Update<StockStrategyEntity>(stockStrategy);
                }
            }
        }
    }
}
