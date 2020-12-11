using StockSimulateCore.Model;
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
    public partial class StrategyListForm : Form
    {
        private SQLiteDBUtil Repository = SQLiteDBUtil.Instance;
        public StrategyListForm()
        {
            InitializeComponent();
        }

        private void StrategyListForm_Load(object sender, EventArgs e)
        {
            this.LoadStrategyList();
        }

        void LoadStrategyList()
        {
            var where = string.Empty;
            if (!string.IsNullOrEmpty(this.txtStrategyName.Text) && this.txtStrategyName.Text != "名称")
            {
                where = $"Name like '%{this.txtStrategyName.Text}%'";
            }
            var strategys = Repository.QueryAll<StrategyEntity>(where);
            var dt = ObjectUtil.ConvertTable<StrategyEntity>(strategys);
            this.dataGridView1.DataSource = dt.DefaultView;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.ReadOnly = true;
            //this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            for (var i = 0; i < this.dataGridView1.ColumnCount; i++)
            {
                var length = this.dataGridView1.Columns[i].Name.Length;
                this.dataGridView1.Columns[i].Width = length < 6 ? 80 : length < 8 ? 120 : length < 12 ? 160 : 200;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            var frm = new StrategyForm();
            frm.StartPosition = FormStartPosition.CenterParent;
            if(frm.ShowDialog() == DialogResult.OK)
            {
                var strategy = Repository.QueryFirst<StrategyEntity>($"Name='{frm.Strategy.Name}'");
                if(strategy == null)
                {
                    Repository.Insert<StrategyEntity>(frm.Strategy);
                }
                else
                {
                    frm.Strategy.ID = strategy.ID;
                    Repository.Update<StrategyEntity>(frm.Strategy);
                }
                this.LoadStrategyList();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }
    }
}
