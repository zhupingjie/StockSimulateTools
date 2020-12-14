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
            this.txtSalePrice = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtMaxDownPer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSaleHoldPer = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtSaleRate = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtBaseBuyAmount = new System.Windows.Forms.TextBox();
            this.label111 = new System.Windows.Forms.Label();
            this.txtBaseBuyPrice = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCalcuate = new System.Windows.Forms.Button();
            this.txtMaxBuyPercent = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtSingleBuyPercent = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMaxBuyPrice = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
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
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtSalePrice);
            this.panel1.Controls.Add(this.label19);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.txtMaxDownPer);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtSaleHoldPer);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.txtSaleRate);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.txtBaseBuyAmount);
            this.panel1.Controls.Add(this.label111);
            this.panel1.Controls.Add(this.txtBaseBuyPrice);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnCalcuate);
            this.panel1.Controls.Add(this.txtMaxBuyPercent);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.txtSingleBuyPercent);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.txtMaxBuyPrice);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtExtraBuyPercent2);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtDownPercent2);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtExtraBuyPercent1);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtDownPercent1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtBuyAmount);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtBuyRate);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1005, 142);
            this.panel1.TabIndex = 2;
            // 
            // txtSalePrice
            // 
            this.txtSalePrice.Location = new System.Drawing.Point(95, 102);
            this.txtSalePrice.Name = "txtSalePrice";
            this.txtSalePrice.Size = new System.Drawing.Size(82, 21);
            this.txtSalePrice.TabIndex = 44;
            this.txtSalePrice.Text = "10";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(12, 105);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(41, 12);
            this.label19.TabIndex = 45;
            this.label19.Text = "卖出价";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(696, 105);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(82, 21);
            this.txtName.TabIndex = 34;
            this.txtName.Text = "默认策略";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(613, 108);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 35;
            this.label14.Text = "策略名称";
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.Location = new System.Drawing.Point(906, 18);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(87, 78);
            this.btnSave.TabIndex = 33;
            this.btnSave.Text = "保存策略";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtMaxDownPer
            // 
            this.txtMaxDownPer.Location = new System.Drawing.Point(696, 45);
            this.txtMaxDownPer.Name = "txtMaxDownPer";
            this.txtMaxDownPer.Size = new System.Drawing.Size(82, 21);
            this.txtMaxDownPer.TabIndex = 31;
            this.txtMaxDownPer.Text = "-40";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(595, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 32;
            this.label1.Text = "最大跌幅(%)";
            // 
            // txtSaleHoldPer
            // 
            this.txtSaleHoldPer.Location = new System.Drawing.Point(492, 102);
            this.txtSaleHoldPer.Name = "txtSaleHoldPer";
            this.txtSaleHoldPer.Size = new System.Drawing.Size(82, 21);
            this.txtSaleHoldPer.TabIndex = 29;
            this.txtSaleHoldPer.Text = "10";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(402, 102);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(71, 12);
            this.label13.TabIndex = 30;
            this.label13.Text = "减仓仓位(%)";
            // 
            // txtSaleRate
            // 
            this.txtSaleRate.Location = new System.Drawing.Point(299, 99);
            this.txtSaleRate.Name = "txtSaleRate";
            this.txtSaleRate.Size = new System.Drawing.Size(82, 21);
            this.txtSaleRate.TabIndex = 27;
            this.txtSaleRate.Text = "5";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(216, 102);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(71, 12);
            this.label12.TabIndex = 28;
            this.label12.Text = "上涨幅度(%)";
            // 
            // txtBaseBuyAmount
            // 
            this.txtBaseBuyAmount.Location = new System.Drawing.Point(299, 18);
            this.txtBaseBuyAmount.Name = "txtBaseBuyAmount";
            this.txtBaseBuyAmount.Size = new System.Drawing.Size(82, 21);
            this.txtBaseBuyAmount.TabIndex = 25;
            this.txtBaseBuyAmount.Text = "5000";
            // 
            // label111
            // 
            this.label111.AutoSize = true;
            this.label111.Location = new System.Drawing.Point(198, 21);
            this.label111.Name = "label111";
            this.label111.Size = new System.Drawing.Size(53, 12);
            this.label111.TabIndex = 26;
            this.label111.Text = "建仓市值";
            // 
            // txtBaseBuyPrice
            // 
            this.txtBaseBuyPrice.Location = new System.Drawing.Point(95, 18);
            this.txtBaseBuyPrice.Name = "txtBaseBuyPrice";
            this.txtBaseBuyPrice.Size = new System.Drawing.Size(82, 21);
            this.txtBaseBuyPrice.TabIndex = 23;
            this.txtBaseBuyPrice.Text = "10";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 24;
            this.label4.Text = "建仓价";
            // 
            // btnCalcuate
            // 
            this.btnCalcuate.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCalcuate.Location = new System.Drawing.Point(802, 18);
            this.btnCalcuate.Name = "btnCalcuate";
            this.btnCalcuate.Size = new System.Drawing.Size(87, 78);
            this.btnCalcuate.TabIndex = 22;
            this.btnCalcuate.Text = "模拟计算";
            this.btnCalcuate.UseVisualStyleBackColor = true;
            this.btnCalcuate.Click += new System.EventHandler(this.btnCalcuate_Click);
            // 
            // txtMaxBuyPercent
            // 
            this.txtMaxBuyPercent.Location = new System.Drawing.Point(696, 18);
            this.txtMaxBuyPercent.Name = "txtMaxBuyPercent";
            this.txtMaxBuyPercent.Size = new System.Drawing.Size(82, 21);
            this.txtMaxBuyPercent.TabIndex = 20;
            this.txtMaxBuyPercent.Text = "80";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(595, 21);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 12);
            this.label11.TabIndex = 21;
            this.label11.Text = "最大仓位(%)";
            // 
            // txtSingleBuyPercent
            // 
            this.txtSingleBuyPercent.Location = new System.Drawing.Point(493, 45);
            this.txtSingleBuyPercent.Name = "txtSingleBuyPercent";
            this.txtSingleBuyPercent.Size = new System.Drawing.Size(82, 21);
            this.txtSingleBuyPercent.TabIndex = 18;
            this.txtSingleBuyPercent.Text = "20";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(408, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(83, 12);
            this.label10.TabIndex = 19;
            this.label10.Text = "单持股比例(%)";
            // 
            // txtMaxBuyPrice
            // 
            this.txtMaxBuyPrice.Location = new System.Drawing.Point(493, 18);
            this.txtMaxBuyPrice.Name = "txtMaxBuyPrice";
            this.txtMaxBuyPrice.Size = new System.Drawing.Size(82, 21);
            this.txtMaxBuyPrice.TabIndex = 16;
            this.txtMaxBuyPrice.Text = "200000";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(408, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 17;
            this.label9.Text = "总投资市值";
            // 
            // txtExtraBuyPercent2
            // 
            this.txtExtraBuyPercent2.Location = new System.Drawing.Point(696, 75);
            this.txtExtraBuyPercent2.Name = "txtExtraBuyPercent2";
            this.txtExtraBuyPercent2.Size = new System.Drawing.Size(82, 21);
            this.txtExtraBuyPercent2.TabIndex = 14;
            this.txtExtraBuyPercent2.Text = "50";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(595, 78);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 12);
            this.label7.TabIndex = 15;
            this.label7.Text = "额外加仓市值(%)";
            // 
            // txtDownPercent2
            // 
            this.txtDownPercent2.Location = new System.Drawing.Point(492, 75);
            this.txtDownPercent2.Name = "txtDownPercent2";
            this.txtDownPercent2.Size = new System.Drawing.Size(82, 21);
            this.txtDownPercent2.TabIndex = 12;
            this.txtDownPercent2.Text = "-30";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(408, 78);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 13;
            this.label8.Text = "下跌幅度1(%)";
            // 
            // txtExtraBuyPercent1
            // 
            this.txtExtraBuyPercent1.Location = new System.Drawing.Point(299, 72);
            this.txtExtraBuyPercent1.Name = "txtExtraBuyPercent1";
            this.txtExtraBuyPercent1.Size = new System.Drawing.Size(82, 21);
            this.txtExtraBuyPercent1.TabIndex = 10;
            this.txtExtraBuyPercent1.Text = "30";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(198, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "额外加仓市值(%)";
            // 
            // txtDownPercent1
            // 
            this.txtDownPercent1.Location = new System.Drawing.Point(95, 72);
            this.txtDownPercent1.Name = "txtDownPercent1";
            this.txtDownPercent1.Size = new System.Drawing.Size(82, 21);
            this.txtDownPercent1.TabIndex = 8;
            this.txtDownPercent1.Text = "-20";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "下跌幅度1(%)";
            // 
            // txtBuyAmount
            // 
            this.txtBuyAmount.Location = new System.Drawing.Point(299, 45);
            this.txtBuyAmount.Name = "txtBuyAmount";
            this.txtBuyAmount.Size = new System.Drawing.Size(82, 21);
            this.txtBuyAmount.TabIndex = 4;
            this.txtBuyAmount.Text = "5000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(198, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "加仓市值";
            // 
            // txtBuyRate
            // 
            this.txtBuyRate.Location = new System.Drawing.Point(95, 45);
            this.txtBuyRate.Name = "txtBuyRate";
            this.txtBuyRate.Size = new System.Drawing.Size(82, 21);
            this.txtBuyRate.TabIndex = 2;
            this.txtBuyRate.Text = "-5";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "加仓点数(%)";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 142);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1005, 506);
            this.dataGridView1.TabIndex = 3;
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
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
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
        private System.Windows.Forms.TextBox txtMaxBuyPrice;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSingleBuyPercent;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtMaxBuyPercent;
        private System.Windows.Forms.Label label11;
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
        private System.Windows.Forms.TextBox txtMaxDownPer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtSalePrice;
        private System.Windows.Forms.Label label19;
    }
}