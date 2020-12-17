namespace StockSimulateUI.UC
{
    partial class TExchangeUC
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMinPriceStop = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMaxPriceStop = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtMaxErrorMatch = new System.Windows.Forms.TextBox();
            this.txtDownPer = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBSQty = new System.Windows.Forms.TextBox();
            this.txtMaxSingleBS = new System.Windows.Forms.TextBox();
            this.txtLockQty = new System.Windows.Forms.TextBox();
            this.txtUpPer = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtBasePrice = new System.Windows.Forms.TextBox();
            this.label111 = new System.Windows.Forms.Label();
            this.txtHoldQty = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(872, 187);
            this.panel1.TabIndex = 53;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtMinPriceStop);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtMaxPriceStop);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtMaxErrorMatch);
            this.groupBox1.Controls.Add(this.txtDownPer);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtBSQty);
            this.groupBox1.Controls.Add(this.txtMaxSingleBS);
            this.groupBox1.Controls.Add(this.txtLockQty);
            this.groupBox1.Controls.Add(this.txtUpPer);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(239, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(633, 187);
            this.groupBox1.TabIndex = 55;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "买卖属性";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(318, 140);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(138, 21);
            this.label11.TabIndex = 51;
            this.label11.Text = "股价下跌达到终止";
            // 
            // txtMinPriceStop
            // 
            this.txtMinPriceStop.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMinPriceStop.Location = new System.Drawing.Point(462, 139);
            this.txtMinPriceStop.Name = "txtMinPriceStop";
            this.txtMinPriceStop.Size = new System.Drawing.Size(103, 29);
            this.txtMinPriceStop.TabIndex = 50;
            this.txtMinPriceStop.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(318, 103);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(138, 21);
            this.label10.TabIndex = 49;
            this.label10.Text = "股价上涨达到终止";
            // 
            // txtMaxPriceStop
            // 
            this.txtMaxPriceStop.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMaxPriceStop.Location = new System.Drawing.Point(462, 103);
            this.txtMaxPriceStop.Name = "txtMaxPriceStop";
            this.txtMaxPriceStop.Size = new System.Drawing.Size(103, 29);
            this.txtMaxPriceStop.TabIndex = 48;
            this.txtMaxPriceStop.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(24, 142);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(170, 21);
            this.label9.TabIndex = 47;
            this.label9.Text = "连续匹配失败终止次数";
            // 
            // txtMaxErrorMatch
            // 
            this.txtMaxErrorMatch.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMaxErrorMatch.Location = new System.Drawing.Point(200, 140);
            this.txtMaxErrorMatch.Name = "txtMaxErrorMatch";
            this.txtMaxErrorMatch.Size = new System.Drawing.Size(101, 29);
            this.txtMaxErrorMatch.TabIndex = 46;
            this.txtMaxErrorMatch.Text = "2";
            // 
            // txtDownPer
            // 
            this.txtDownPer.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDownPer.Location = new System.Drawing.Point(200, 30);
            this.txtDownPer.Name = "txtDownPer";
            this.txtDownPer.Size = new System.Drawing.Size(101, 29);
            this.txtDownPer.TabIndex = 2;
            this.txtDownPer.Text = "-2.5";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(356, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 21);
            this.label6.TabIndex = 11;
            this.label6.Text = "锁定数量(股)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(32, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(162, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "股价下跌幅度达到(%)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(32, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(162, 21);
            this.label5.TabIndex = 9;
            this.label5.Text = "股价上涨幅度达到(%)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(24, 105);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(170, 21);
            this.label8.TabIndex = 13;
            this.label8.Text = "连续单向买卖最大次数";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(356, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 21);
            this.label3.TabIndex = 5;
            this.label3.Text = "每笔数量(股)";
            // 
            // txtBSQty
            // 
            this.txtBSQty.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBSQty.Location = new System.Drawing.Point(462, 30);
            this.txtBSQty.Name = "txtBSQty";
            this.txtBSQty.Size = new System.Drawing.Size(103, 29);
            this.txtBSQty.TabIndex = 4;
            this.txtBSQty.Text = "300";
            // 
            // txtMaxSingleBS
            // 
            this.txtMaxSingleBS.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMaxSingleBS.Location = new System.Drawing.Point(200, 103);
            this.txtMaxSingleBS.Name = "txtMaxSingleBS";
            this.txtMaxSingleBS.Size = new System.Drawing.Size(101, 29);
            this.txtMaxSingleBS.TabIndex = 12;
            this.txtMaxSingleBS.Text = "2";
            // 
            // txtLockQty
            // 
            this.txtLockQty.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLockQty.Location = new System.Drawing.Point(462, 65);
            this.txtLockQty.Name = "txtLockQty";
            this.txtLockQty.Size = new System.Drawing.Size(103, 29);
            this.txtLockQty.TabIndex = 10;
            this.txtLockQty.Text = "200";
            // 
            // txtUpPer
            // 
            this.txtUpPer.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUpPer.Location = new System.Drawing.Point(200, 65);
            this.txtUpPer.Name = "txtUpPer";
            this.txtUpPer.Size = new System.Drawing.Size(101, 29);
            this.txtUpPer.TabIndex = 8;
            this.txtUpPer.Text = "2.5";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtBasePrice);
            this.groupBox2.Controls.Add(this.label111);
            this.groupBox2.Controls.Add(this.txtHoldQty);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(239, 187);
            this.groupBox2.TabIndex = 53;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "主要属性";
            // 
            // txtBasePrice
            // 
            this.txtBasePrice.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBasePrice.Location = new System.Drawing.Point(113, 30);
            this.txtBasePrice.Name = "txtBasePrice";
            this.txtBasePrice.Size = new System.Drawing.Size(107, 29);
            this.txtBasePrice.TabIndex = 23;
            this.txtBasePrice.Text = "0";
            // 
            // label111
            // 
            this.label111.AutoSize = true;
            this.label111.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label111.Location = new System.Drawing.Point(9, 70);
            this.label111.Name = "label111";
            this.label111.Size = new System.Drawing.Size(100, 21);
            this.label111.TabIndex = 26;
            this.label111.Text = "底仓数量(股)";
            // 
            // txtHoldQty
            // 
            this.txtHoldQty.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtHoldQty.Location = new System.Drawing.Point(113, 67);
            this.txtHoldQty.Name = "txtHoldQty";
            this.txtHoldQty.Size = new System.Drawing.Size(107, 29);
            this.txtHoldQty.TabIndex = 25;
            this.txtHoldQty.Text = "500";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(49, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 21);
            this.label4.TabIndex = 24;
            this.label4.Text = "基准价";
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 187);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 10;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(872, 336);
            this.dataGridView1.TabIndex = 54;
            // 
            // TExchangeUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Name = "TExchangeUC";
            this.Size = new System.Drawing.Size(872, 523);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtBasePrice;
        private System.Windows.Forms.Label label111;
        private System.Windows.Forms.TextBox txtHoldQty;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtDownPer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBSQty;
        private System.Windows.Forms.TextBox txtMaxSingleBS;
        private System.Windows.Forms.TextBox txtLockQty;
        private System.Windows.Forms.TextBox txtUpPer;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtMaxErrorMatch;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtMinPriceStop;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtMaxPriceStop;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}
