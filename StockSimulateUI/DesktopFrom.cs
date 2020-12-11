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

        private void btnValuatie_Click(object sender, EventArgs e)
        {
            var frm = new VoluatieForm();
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
            this.dataGridView1.DataSource = dt.DefaultView;
            for(var i=0; i<this.dataGridView1.ColumnCount; i++)
            {
                var length = this.dataGridView1.Columns[i].Name.Length;
                this.dataGridView1.Columns[i].Width = length < 6 ? 80 : length < 8 ? 120 : 140;
            }
            for(var i=0; i<this.dataGridView1.Rows.Count; i++)
            {
                var row = this.dataGridView1.Rows[i];
                var value = ObjectUtil.ToValue<decimal>(row.Cells["浮动(%)"].Value, 0);
                if(value > 0)
                {
                    this.dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                }
                else
                {
                    this.dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Green;
                }
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count == 0) return;
            this.listView1.Items.Clear();

            var selectRow = this.dataGridView1.SelectedRows[0];
            foreach (DataGridViewCell cell in selectRow.Cells)
            {
                var listViewItem = new ListViewItem();
                listViewItem.Text = $"{this.dataGridView1.Columns[cell.ColumnIndex].Name}";
                listViewItem.SubItems.Add($"{selectRow.Cells[cell.ColumnIndex].Value}");
                this.listView1.Items.Add(listViewItem);
            }
        }

        private void btnGather_Click(object sender, EventArgs e)
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
    }
}
