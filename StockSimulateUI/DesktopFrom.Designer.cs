namespace StockPriceTools
{
    partial class DesktopFrom
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DesktopFrom));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddStock = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAddStrategy = new System.Windows.Forms.ToolStripButton();
            this.btnSetStrategy = new System.Windows.Forms.ToolStripButton();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnInitialize = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnGather = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtStockCode = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridStockList = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lstBaseInfo = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControlLeft = new System.Windows.Forms.TabControl();
            this.tabBaseData = new System.Windows.Forms.TabPage();
            this.tabStrategyData = new System.Windows.Forms.TabPage();
            this.lstStrategyInfo = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControlBottom = new System.Windows.Forms.TabControl();
            this.tabPriceList = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gridPriceList = new System.Windows.Forms.DataGridView();
            this.gridExchangeList = new System.Windows.Forms.DataGridView();
            this.btnAccountInfo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tabPriceData = new System.Windows.Forms.TabPage();
            this.tabExchange = new System.Windows.Forms.TabPage();
            this.lstPriceInfo = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lstExchangeInfo = new System.Windows.Forms.ListView();
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridStockList)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControlLeft.SuspendLayout();
            this.tabBaseData.SuspendLayout();
            this.tabStrategyData.SuspendLayout();
            this.tabControlBottom.SuspendLayout();
            this.tabPriceList.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPriceList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridExchangeList)).BeginInit();
            this.tabPriceData.SuspendLayout();
            this.tabExchange.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 612);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1208, 22);
            this.statusStrip1.TabIndex = 38;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAccountInfo,
            this.toolStripSeparator2,
            this.btnAddStock,
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.btnAddStrategy,
            this.btnSetStrategy,
            this.toolStripSplitButton1,
            this.btnInitialize});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1208, 25);
            this.toolStrip1.TabIndex = 39;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAddStock
            // 
            this.btnAddStock.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnAddStock.Image = ((System.Drawing.Image)(resources.GetObject("btnAddStock.Image")));
            this.btnAddStock.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddStock.Name = "btnAddStock";
            this.btnAddStock.Size = new System.Drawing.Size(60, 22);
            this.btnAddStock.Text = "添加股票";
            this.btnAddStock.Click += new System.EventHandler(this.btnAddStock_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(60, 22);
            this.toolStripButton1.Text = "删除股票";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAddStrategy
            // 
            this.btnAddStrategy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnAddStrategy.Image = ((System.Drawing.Image)(resources.GetObject("btnAddStrategy.Image")));
            this.btnAddStrategy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddStrategy.Name = "btnAddStrategy";
            this.btnAddStrategy.Size = new System.Drawing.Size(60, 22);
            this.btnAddStrategy.Text = "买卖策略";
            this.btnAddStrategy.Click += new System.EventHandler(this.btnAddStrategy_Click);
            // 
            // btnSetStrategy
            // 
            this.btnSetStrategy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSetStrategy.Image = ((System.Drawing.Image)(resources.GetObject("btnSetStrategy.Image")));
            this.btnSetStrategy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSetStrategy.Name = "btnSetStrategy";
            this.btnSetStrategy.Size = new System.Drawing.Size(60, 22);
            this.btnSetStrategy.Text = "设置策略";
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnInitialize
            // 
            this.btnInitialize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnInitialize.Image = ((System.Drawing.Image)(resources.GetObject("btnInitialize.Image")));
            this.btnInitialize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnInitialize.Name = "btnInitialize";
            this.btnInitialize.Size = new System.Drawing.Size(48, 22);
            this.btnInitialize.Text = "初始化";
            this.btnInitialize.Click += new System.EventHandler(this.btnInitialize_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnGather);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.txtStockCode);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1208, 30);
            this.panel2.TabIndex = 42;
            // 
            // btnGather
            // 
            this.btnGather.Location = new System.Drawing.Point(161, 3);
            this.btnGather.Name = "btnGather";
            this.btnGather.Size = new System.Drawing.Size(54, 23);
            this.btnGather.TabIndex = 2;
            this.btnGather.Text = "同步";
            this.btnGather.UseVisualStyleBackColor = true;
            this.btnGather.Click += new System.EventHandler(this.btnGather_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(101, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(54, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtStockCode
            // 
            this.txtStockCode.Location = new System.Drawing.Point(4, 4);
            this.txtStockCode.Name = "txtStockCode";
            this.txtStockCode.Size = new System.Drawing.Size(91, 21);
            this.txtStockCode.TabIndex = 0;
            this.txtStockCode.Text = "股票代码";
            this.txtStockCode.TextChanged += new System.EventHandler(this.txtStockCode_TextChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 55);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2MinSize = 120;
            this.splitContainer1.Size = new System.Drawing.Size(1208, 557);
            this.splitContainer1.SplitterDistance = 850;
            this.splitContainer1.TabIndex = 43;
            // 
            // gridStockList
            // 
            this.gridStockList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridStockList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridStockList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridStockList.Location = new System.Drawing.Point(0, 0);
            this.gridStockList.MultiSelect = false;
            this.gridStockList.Name = "gridStockList";
            this.gridStockList.ReadOnly = true;
            this.gridStockList.RowTemplate.Height = 23;
            this.gridStockList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridStockList.Size = new System.Drawing.Size(850, 221);
            this.gridStockList.TabIndex = 41;
            this.gridStockList.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_RowEnter);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.tabControlLeft);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(354, 557);
            this.panel1.TabIndex = 41;
            // 
            // lstBaseInfo
            // 
            this.lstBaseInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lstBaseInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstBaseInfo.FullRowSelect = true;
            this.lstBaseInfo.GridLines = true;
            this.lstBaseInfo.Location = new System.Drawing.Point(3, 3);
            this.lstBaseInfo.MultiSelect = false;
            this.lstBaseInfo.Name = "lstBaseInfo";
            this.lstBaseInfo.Size = new System.Drawing.Size(340, 525);
            this.lstBaseInfo.TabIndex = 0;
            this.lstBaseInfo.UseCompatibleStateImageBehavior = false;
            this.lstBaseInfo.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "项目";
            this.columnHeader1.Width = 180;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "数据";
            this.columnHeader2.Width = 140;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.gridStockList);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControlBottom);
            this.splitContainer2.Size = new System.Drawing.Size(850, 557);
            this.splitContainer2.SplitterDistance = 221;
            this.splitContainer2.TabIndex = 0;
            // 
            // tabControlLeft
            // 
            this.tabControlLeft.Controls.Add(this.tabBaseData);
            this.tabControlLeft.Controls.Add(this.tabStrategyData);
            this.tabControlLeft.Controls.Add(this.tabPriceData);
            this.tabControlLeft.Controls.Add(this.tabExchange);
            this.tabControlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlLeft.Location = new System.Drawing.Point(0, 0);
            this.tabControlLeft.Name = "tabControlLeft";
            this.tabControlLeft.SelectedIndex = 0;
            this.tabControlLeft.Size = new System.Drawing.Size(354, 557);
            this.tabControlLeft.TabIndex = 1;
            // 
            // tabBaseData
            // 
            this.tabBaseData.Controls.Add(this.lstBaseInfo);
            this.tabBaseData.Location = new System.Drawing.Point(4, 22);
            this.tabBaseData.Name = "tabBaseData";
            this.tabBaseData.Padding = new System.Windows.Forms.Padding(3);
            this.tabBaseData.Size = new System.Drawing.Size(346, 531);
            this.tabBaseData.TabIndex = 0;
            this.tabBaseData.Text = "核心数据";
            this.tabBaseData.UseVisualStyleBackColor = true;
            // 
            // tabStrategyData
            // 
            this.tabStrategyData.Controls.Add(this.lstStrategyInfo);
            this.tabStrategyData.Location = new System.Drawing.Point(4, 22);
            this.tabStrategyData.Name = "tabStrategyData";
            this.tabStrategyData.Padding = new System.Windows.Forms.Padding(3);
            this.tabStrategyData.Size = new System.Drawing.Size(346, 531);
            this.tabStrategyData.TabIndex = 1;
            this.tabStrategyData.Text = "策略数据";
            this.tabStrategyData.UseVisualStyleBackColor = true;
            // 
            // lstStrategyInfo
            // 
            this.lstStrategyInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.lstStrategyInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstStrategyInfo.FullRowSelect = true;
            this.lstStrategyInfo.GridLines = true;
            this.lstStrategyInfo.Location = new System.Drawing.Point(3, 3);
            this.lstStrategyInfo.MultiSelect = false;
            this.lstStrategyInfo.Name = "lstStrategyInfo";
            this.lstStrategyInfo.Size = new System.Drawing.Size(340, 525);
            this.lstStrategyInfo.TabIndex = 1;
            this.lstStrategyInfo.UseCompatibleStateImageBehavior = false;
            this.lstStrategyInfo.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "项目";
            this.columnHeader3.Width = 180;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "数据";
            this.columnHeader4.Width = 140;
            // 
            // tabControlBottom
            // 
            this.tabControlBottom.Controls.Add(this.tabPriceList);
            this.tabControlBottom.Controls.Add(this.tabPage2);
            this.tabControlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlBottom.Location = new System.Drawing.Point(0, 0);
            this.tabControlBottom.Name = "tabControlBottom";
            this.tabControlBottom.SelectedIndex = 0;
            this.tabControlBottom.Size = new System.Drawing.Size(850, 332);
            this.tabControlBottom.TabIndex = 44;
            // 
            // tabPriceList
            // 
            this.tabPriceList.Controls.Add(this.gridPriceList);
            this.tabPriceList.Location = new System.Drawing.Point(4, 22);
            this.tabPriceList.Name = "tabPriceList";
            this.tabPriceList.Padding = new System.Windows.Forms.Padding(3);
            this.tabPriceList.Size = new System.Drawing.Size(842, 306);
            this.tabPriceList.TabIndex = 0;
            this.tabPriceList.Text = "历史股价";
            this.tabPriceList.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gridExchangeList);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(842, 306);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "交易记录";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gridPriceList
            // 
            this.gridPriceList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridPriceList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPriceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPriceList.Location = new System.Drawing.Point(3, 3);
            this.gridPriceList.MultiSelect = false;
            this.gridPriceList.Name = "gridPriceList";
            this.gridPriceList.ReadOnly = true;
            this.gridPriceList.RowTemplate.Height = 23;
            this.gridPriceList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPriceList.Size = new System.Drawing.Size(836, 300);
            this.gridPriceList.TabIndex = 44;
            this.gridPriceList.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridPriceList_RowEnter);
            // 
            // gridExchangeList
            // 
            this.gridExchangeList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridExchangeList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridExchangeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridExchangeList.Location = new System.Drawing.Point(3, 3);
            this.gridExchangeList.MultiSelect = false;
            this.gridExchangeList.Name = "gridExchangeList";
            this.gridExchangeList.ReadOnly = true;
            this.gridExchangeList.RowTemplate.Height = 23;
            this.gridExchangeList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridExchangeList.Size = new System.Drawing.Size(836, 300);
            this.gridExchangeList.TabIndex = 45;
            this.gridExchangeList.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridExchangeList_RowEnter);
            // 
            // btnAccountInfo
            // 
            this.btnAccountInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnAccountInfo.Image = ((System.Drawing.Image)(resources.GetObject("btnAccountInfo.Image")));
            this.btnAccountInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAccountInfo.Name = "btnAccountInfo";
            this.btnAccountInfo.Size = new System.Drawing.Size(60, 22);
            this.btnAccountInfo.Text = "我的账户";
            this.btnAccountInfo.Click += new System.EventHandler(this.btnAccountInfo_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tabPriceData
            // 
            this.tabPriceData.Controls.Add(this.lstPriceInfo);
            this.tabPriceData.Location = new System.Drawing.Point(4, 22);
            this.tabPriceData.Name = "tabPriceData";
            this.tabPriceData.Size = new System.Drawing.Size(346, 531);
            this.tabPriceData.TabIndex = 2;
            this.tabPriceData.Text = "股价数据";
            this.tabPriceData.UseVisualStyleBackColor = true;
            // 
            // tabExchange
            // 
            this.tabExchange.Controls.Add(this.lstExchangeInfo);
            this.tabExchange.Location = new System.Drawing.Point(4, 22);
            this.tabExchange.Name = "tabExchange";
            this.tabExchange.Size = new System.Drawing.Size(346, 531);
            this.tabExchange.TabIndex = 3;
            this.tabExchange.Text = "交易数据";
            this.tabExchange.UseVisualStyleBackColor = true;
            // 
            // lstPriceInfo
            // 
            this.lstPriceInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6});
            this.lstPriceInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPriceInfo.FullRowSelect = true;
            this.lstPriceInfo.GridLines = true;
            this.lstPriceInfo.Location = new System.Drawing.Point(0, 0);
            this.lstPriceInfo.MultiSelect = false;
            this.lstPriceInfo.Name = "lstPriceInfo";
            this.lstPriceInfo.Size = new System.Drawing.Size(346, 531);
            this.lstPriceInfo.TabIndex = 1;
            this.lstPriceInfo.UseCompatibleStateImageBehavior = false;
            this.lstPriceInfo.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "项目";
            this.columnHeader5.Width = 180;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "数据";
            this.columnHeader6.Width = 140;
            // 
            // lstExchangeInfo
            // 
            this.lstExchangeInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader8});
            this.lstExchangeInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstExchangeInfo.FullRowSelect = true;
            this.lstExchangeInfo.GridLines = true;
            this.lstExchangeInfo.Location = new System.Drawing.Point(0, 0);
            this.lstExchangeInfo.MultiSelect = false;
            this.lstExchangeInfo.Name = "lstExchangeInfo";
            this.lstExchangeInfo.Size = new System.Drawing.Size(346, 531);
            this.lstExchangeInfo.TabIndex = 1;
            this.lstExchangeInfo.UseCompatibleStateImageBehavior = false;
            this.lstExchangeInfo.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "项目";
            this.columnHeader7.Width = 180;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "数据";
            this.columnHeader8.Width = 140;
            // 
            // DesktopFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1208, 634);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "DesktopFrom";
            this.Text = "Stock Simulate App";
            this.Load += new System.EventHandler(this.DesktopFrom_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridStockList)).EndInit();
            this.panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControlLeft.ResumeLayout(false);
            this.tabBaseData.ResumeLayout(false);
            this.tabStrategyData.ResumeLayout(false);
            this.tabControlBottom.ResumeLayout(false);
            this.tabPriceList.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridPriceList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridExchangeList)).EndInit();
            this.tabPriceData.ResumeLayout(false);
            this.tabExchange.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAddStock;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView gridStockList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtStockCode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnAddStrategy;
        private System.Windows.Forms.ToolStripButton btnSetStrategy;
        private System.Windows.Forms.ToolStripSeparator toolStripSplitButton1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ListView lstBaseInfo;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnGather;
        private System.Windows.Forms.ToolStripButton btnInitialize;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabControlLeft;
        private System.Windows.Forms.TabPage tabBaseData;
        private System.Windows.Forms.TabPage tabStrategyData;
        private System.Windows.Forms.ListView lstStrategyInfo;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ToolStripButton btnAccountInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.TabControl tabControlBottom;
        private System.Windows.Forms.TabPage tabPriceList;
        private System.Windows.Forms.DataGridView gridPriceList;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView gridExchangeList;
        private System.Windows.Forms.TabPage tabPriceData;
        private System.Windows.Forms.ListView lstPriceInfo;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.TabPage tabExchange;
        private System.Windows.Forms.ListView lstExchangeInfo;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
    }
}