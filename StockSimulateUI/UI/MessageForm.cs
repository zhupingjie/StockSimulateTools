﻿using StockSimulateDomain.Entity;
using StockSimulateService.Service;
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
using StockSimulateCore.Data;

namespace StockSimulateUI.UI
{
    public partial class MessageForm : Form
    {
        public MessageForm()
        {
            InitializeComponent();
        }

        void LoadMessageList()
        {
            var messages = Repository.Instance.QueryAll<MessageEntity>($"Handled=0 and ReadTime>='{DateTime.Now.ToString("yyyy-MM-dd")}'", "ID desc");
            var dt = ObjectUtil.ConvertTable(messages, true);
            this.gridMessageList.DataSource = null;
            this.gridMessageList.DataSource = dt.DefaultView;

            var goodCellStyle = new DataGridViewCellStyle();
            goodCellStyle.BackColor = Color.LightYellow;
            for (var i = 0; i < this.gridMessageList.ColumnCount; i++)
            {
                this.gridMessageList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                var columnName = this.gridMessageList.Columns[i].Name;
                if (columnName == "消息内容")
                {
                    this.gridMessageList.Columns[i].Width = 300;
                }
                else
                {
                    this.gridMessageList.Columns[i].Width = ObjectUtil.GetGridColumnLength(columnName);
                }
                this.gridMessageList.Columns[i].DefaultCellStyle.ForeColor = Color.DarkGray;
                if (new string[] { "股票代码", "股票名称", "股价", "浮动(%)", "消息内容" }.Contains(columnName))
                {
                    this.gridMessageList.Columns[i].DefaultCellStyle = goodCellStyle;
                }
            }
            for (var i = 0; i < this.gridMessageList.Rows.Count; i++)
            {
                var row = this.gridMessageList.Rows[i];
                var value = ObjectUtil.ToValue<decimal>(row.Cells["浮动(%)"].Value, 0);
                if (value > 0)
                {
                    row.Cells["股价"].Style.ForeColor = Color.Red;
                    row.Cells["浮动(%)"].Style.ForeColor = Color.Red;
                }
                else
                {
                    row.Cells["股价"].Style.ForeColor = Color.Green;
                    row.Cells["浮动(%)"].Style.ForeColor = Color.Green;
                }
                value = ObjectUtil.ToValue<decimal>(row.Cells["股价"].Value, 0);
                row.Cells["股价"].Value = ObjectUtil.ToValue<decimal>(value, 0).ToString("#.###");
            }
        }

        private void MessageForm_Load(object sender, EventArgs e)
        {
            Action act = delegate ()
            {
                this.LoadMessageList();
            };
            this.Invoke(act);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.gridMessageList.SelectedRows.Count == 0) return;

            if (MessageBox.Show($"确认要删除选中的消息数据?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;

            var ids = new List<int>();
            for (var i = 0; i < this.gridMessageList.SelectedRows.Count; i++)
            {
                var selectRow = this.gridMessageList.SelectedRows[i];

                var id = ObjectUtil.ToValue<int>(selectRow.Cells["序号"].Value, 0);
                if (id > 0) ids.Add(id);
            }
            if (ids.Count == 0) return;

            StockMessageService.Delete(ids.ToArray());

            Action act = delegate ()
            {
                this.LoadMessageList();
            };
            this.Invoke(act);
        }

        private void btnHandled_Click(object sender, EventArgs e)
        {
            if (this.gridMessageList.SelectedRows.Count == 0) return;

            if (MessageBox.Show($"确认要处理选中的消息数据?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;

            var ids = new List<int>();
            for (var i = 0; i < this.gridMessageList.SelectedRows.Count; i++)
            {
                var selectRow = this.gridMessageList.SelectedRows[i];

                var id = ObjectUtil.ToValue<int>(selectRow.Cells["序号"].Value, 0);
                if (id > 0) ids.Add(id);
            }
            if (ids.Count == 0) return;

            StockMessageService.Handled(ids.ToArray());

            Action act = delegate ()
            {
                this.LoadMessageList();
            };
            this.Invoke(act);
        }

        private void btnAllHandle_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"确认要处理所有的消息数据?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;

            StockMessageService.AllHandled();

            Action act = delegate ()
            {
                this.LoadMessageList();
            };
            this.Invoke(act);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
