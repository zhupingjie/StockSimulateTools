namespace StockSimulateUI.UI
{
    partial class StrategyForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StrategyForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtName = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtSalePrice = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtSaleHoldPer = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtSaleRate = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtBaseBuyAmount = new System.Windows.Forms.TextBox();
            this.label111 = new System.Windows.Forms.Label();
            this.txtBaseBuyPrice = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCalcuate = new System.Windows.Forms.Button();
            this.txtExtraBuyPercent2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDownPercent2 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtExtraBuyPercent1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDownPercent1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBuyAmount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBuyRate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTotalBuyAmount = new System.Windows.Forms.TextBox();
            this.txtStopLossPer = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtStockCode = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtStockName = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnCalcuate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1005, 190);
            this.panel1.TabIndex = 2;
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtName.FormattingEnabled = true;
            this.txtName.Items.AddRange(new object[] {
            "左侧交易",
            "T+0交易",
            "T+N交易",
            "波段交易"});
            this.txtName.Location = new System.Drawing.Point(107, 59);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(107, 29);
            this.txtName.TabIndex = 47;
            this.txtName.Text = "左侧交易";
            this.txtName.SelectedIndexChanged += new System.EventHandler(this.txtName_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(27, 62);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(74, 21);
            this.label15.TabIndex = 46;
            this.label15.Text = "策略名称";
            // 
            // txtSalePrice
            // 
            this.txtSalePrice.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSalePrice.Location = new System.Drawing.Point(107, 128);
            this.txtSalePrice.Name = "txtSalePrice";
            this.txtSalePrice.Size = new System.Drawing.Size(107, 29);
            this.txtSalePrice.TabIndex = 44;
            this.txtSalePrice.Text = "0";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label19.Location = new System.Drawing.Point(43, 131);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(58, 21);
            this.label19.TabIndex = 45;
            this.label19.Text = "减仓价";
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.Location = new System.Drawing.Point(906, 14);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(87, 78);
            this.btnSave.TabIndex = 33;
            this.btnSave.Text = "保存策略";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtSaleHoldPer
            // 
            this.txtSaleHoldPer.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSaleHoldPer.Location = new System.Drawing.Point(317, 131);
            this.txtSaleHoldPer.Name = "txtSaleHoldPer";
            this.txtSaleHoldPer.Size = new System.Drawing.Size(103, 29);
            this.txtSaleHoldPer.TabIndex = 29;
            this.txtSaleHoldPer.Text = "10";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(215, 137);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(98, 21);
            this.label13.TabIndex = 30;
            this.label13.Text = "减仓仓位(%)";
            // 
            // txtSaleRate
            // 
            this.txtSaleRate.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSaleRate.Location = new System.Drawing.Point(108, 137);
            this.txtSaleRate.Name = "txtSaleRate";
            this.txtSaleRate.Size = new System.Drawing.Size(101, 29);
            this.txtSaleRate.TabIndex = 27;
            this.txtSaleRate.Text = "5";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(4, 140);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(98, 21);
            this.label12.TabIndex = 28;
            this.label12.Text = "上涨幅度(%)";
            // 
            // txtBaseBuyAmount
            // 
            this.txtBaseBuyAmount.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBaseBuyAmount.Location = new System.Drawing.Point(336, 93);
            this.txtBaseBuyAmount.Name = "txtBaseBuyAmount";
            this.txtBaseBuyAmount.Size = new System.Drawing.Size(107, 29);
            this.txtBaseBuyAmount.TabIndex = 25;
            this.txtBaseBuyAmount.Text = "5000";
            // 
            // label111
            // 
            this.label111.AutoSize = true;
            this.label111.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label111.Location = new System.Drawing.Point(230, 98);
            this.label111.Name = "label111";
            this.label111.Size = new System.Drawing.Size(100, 21);
            this.label111.TabIndex = 26;
            this.label111.Text = "建仓市值(元)";
            // 
            // txtBaseBuyPrice
            // 
            this.txtBaseBuyPrice.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBaseBuyPrice.Location = new System.Drawing.Point(107, 93);
            this.txtBaseBuyPrice.Name = "txtBaseBuyPrice";
            this.txtBaseBuyPrice.Size = new System.Drawing.Size(107, 29);
            this.txtBaseBuyPrice.TabIndex = 23;
            this.txtBaseBuyPrice.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(43, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 21);
            this.label4.TabIndex = 24;
            this.label4.Text = "建仓价";
            // 
            // btnCalcuate
            // 
            this.btnCalcuate.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCalcuate.Location = new System.Drawing.Point(906, 105);
            this.btnCalcuate.Name = "btnCalcuate";
            this.btnCalcuate.Size = new System.Drawing.Size(87, 78);
            this.btnCalcuate.TabIndex = 22;
            this.btnCalcuate.Text = "模拟计算";
            this.btnCalcuate.UseVisualStyleBackColor = true;
            this.btnCalcuate.Click += new System.EventHandler(this.btnCalcuate_Click);
            // 
            // txtExtraBuyPercent2
            // 
            this.txtExtraBuyPercent2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtExtraBuyPercent2.Location = new System.Drawing.Point(317, 95);
            this.txtExtraBuyPercent2.Name = "txtExtraBuyPercent2";
            this.txtExtraBuyPercent2.Size = new System.Drawing.Size(103, 29);
            this.txtExtraBuyPercent2.TabIndex = 14;
            this.txtExtraBuyPercent2.Text = "50";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(213, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 21);
            this.label7.TabIndex = 15;
            this.label7.Text = "额外加仓(%)";
            // 
            // txtDownPercent2
            // 
            this.txtDownPercent2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDownPercent2.Location = new System.Drawing.Point(108, 100);
            this.txtDownPercent2.Name = "txtDownPercent2";
            this.txtDownPercent2.Size = new System.Drawing.Size(101, 29);
            this.txtDownPercent2.TabIndex = 12;
            this.txtDownPercent2.Text = "-30";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(4, 103);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 21);
            this.label8.TabIndex = 13;
            this.label8.Text = "下跌幅度(%)";
            // 
            // txtExtraBuyPercent1
            // 
            this.txtExtraBuyPercent1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtExtraBuyPercent1.Location = new System.Drawing.Point(317, 60);
            this.txtExtraBuyPercent1.Name = "txtExtraBuyPercent1";
            this.txtExtraBuyPercent1.Size = new System.Drawing.Size(103, 29);
            this.txtExtraBuyPercent1.TabIndex = 10;
            this.txtExtraBuyPercent1.Text = "30";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(213, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 21);
            this.label6.TabIndex = 11;
            this.label6.Text = "额外加仓(%)";
            // 
            // txtDownPercent1
            // 
            this.txtDownPercent1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDownPercent1.Location = new System.Drawing.Point(108, 65);
            this.txtDownPercent1.Name = "txtDownPercent1";
            this.txtDownPercent1.Size = new System.Drawing.Size(101, 29);
            this.txtDownPercent1.TabIndex = 8;
            this.txtDownPercent1.Text = "-20";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(4, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 21);
            this.label5.TabIndex = 9;
            this.label5.Text = "下跌幅度(%)";
            // 
            // txtBuyAmount
            // 
            this.txtBuyAmount.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBuyAmount.Location = new System.Drawing.Point(317, 27);
            this.txtBuyAmount.Name = "txtBuyAmount";
            this.txtBuyAmount.Size = new System.Drawing.Size(103, 29);
            this.txtBuyAmount.TabIndex = 4;
            this.txtBuyAmount.Text = "5000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(213, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 21);
            this.label3.TabIndex = 5;
            this.label3.Text = "加仓市值(元)";
            // 
            // txtBuyRate
            // 
            this.txtBuyRate.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBuyRate.Location = new System.Drawing.Point(108, 30);
            this.txtBuyRate.Name = "txtBuyRate";
            this.txtBuyRate.Size = new System.Drawing.Size(101, 29);
            this.txtBuyRate.TabIndex = 2;
            this.txtBuyRate.Text = "-5";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(4, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "下跌幅度(%)";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 190);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1005, 458);
            this.dataGridView1.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtBuyRate);
            this.groupBox1.Controls.Add(this.txtExtraBuyPercent2);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtSaleHoldPer);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtBuyAmount);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.txtDownPercent2);
            this.groupBox1.Controls.Add(this.txtExtraBuyPercent1);
            this.groupBox1.Controls.Add(this.txtSaleRate);
            this.groupBox1.Controls.Add(this.txtDownPercent1);
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(467, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(433, 183);
            this.groupBox1.TabIndex = 48;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "买卖属性";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtStockName);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.txtStockCode);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtStopLossPer);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtTotalBuyAmount);
            this.groupBox2.Controls.Add(this.txtName);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.txtBaseBuyPrice);
            this.groupBox2.Controls.Add(this.txtSalePrice);
            this.groupBox2.Controls.Add(this.label111);
            this.groupBox2.Controls.Add(this.txtBaseBuyAmount);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(12, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(449, 180);
            this.groupBox2.TabIndex = 49;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "主要属性";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(230, 131);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 21);
            this.label1.TabIndex = 49;
            this.label1.Text = "投入市值(元)";
            // 
            // txtTotalBuyAmount
            // 
            this.txtTotalBuyAmount.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTotalBuyAmount.Location = new System.Drawing.Point(336, 128);
            this.txtTotalBuyAmount.Name = "txtTotalBuyAmount";
            this.txtTotalBuyAmount.Size = new System.Drawing.Size(107, 29);
            this.txtTotalBuyAmount.TabIndex = 48;
            this.txtTotalBuyAmount.Text = "40000";
            // 
            // txtStopLossPer
            // 
            this.txtStopLossPer.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtStopLossPer.Location = new System.Drawing.Point(336, 59);
            this.txtStopLossPer.Name = "txtStopLossPer";
            this.txtStopLossPer.ReadOnly = true;
            this.txtStopLossPer.Size = new System.Drawing.Size(107, 29);
            this.txtStopLossPer.TabIndex = 50;
            this.txtStopLossPer.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(232, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 21);
            this.label9.TabIndex = 51;
            this.label9.Text = "止损幅度(%)";
            // 
            // txtStockCode
            // 
            this.txtStockCode.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtStockCode.Location = new System.Drawing.Point(107, 24);
            this.txtStockCode.Name = "txtStockCode";
            this.txtStockCode.ReadOnly = true;
            this.txtStockCode.Size = new System.Drawing.Size(107, 29);
            this.txtStockCode.TabIndex = 52;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(27, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 21);
            this.label10.TabIndex = 53;
            this.label10.Text = "股票代码";
            // 
            // txtStockName
            // 
            this.txtStockName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtStockName.Location = new System.Drawing.Point(336, 22);
            this.txtStockName.Name = "txtStockName";
            this.txtStockName.ReadOnly = true;
            this.txtStockName.Size = new System.Drawing.Size(107, 29);
            this.txtStockName.TabIndex = 54;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(256, 30);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(74, 21);
            this.label11.TabIndex = 55;
            this.label11.Text = "股票名称";
            // 
            // StrategyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1005, 648);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StrategyForm";
            this.Text = "买卖策略";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtBuyAmount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBuyRate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDownPercent1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtExtraBuyPercent2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtDownPercent2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtExtraBuyPercent1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnCalcuate;
        private System.Windows.Forms.TextBox txtBaseBuyPrice;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBaseBuyAmount;
        private System.Windows.Forms.Label label111;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtSaleHoldPer;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtSaleRate;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtSalePrice;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox txtName;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTotalBuyAmount;
        private System.Windows.Forms.TextBox txtStopLossPer;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtStockName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtStockCode;
        private System.Windows.Forms.Label label10;
    }
}