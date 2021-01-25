namespace StockSimulateUI.UI
{
    partial class RemindForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemindForm));
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUDPer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUpPrice = new System.Windows.Forms.TextBox();
            this.txtDownPrice = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAccount = new System.Windows.Forms.ComboBox();
            this.btnRemindAll = new System.Windows.Forms.Button();
            this.btnOKAll = new System.Windows.Forms.Button();
            this.txtUpAverage = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDownAverage = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnDownAverage = new System.Windows.Forms.Button();
            this.btnUpAverage = new System.Windows.Forms.Button();
            this.btnUDPer = new System.Windows.Forms.Button();
            this.btnUpAverageReverse = new System.Windows.Forms.Button();
            this.btnDownAverageReverse = new System.Windows.Forms.Button();
            this.txtDownMaxDayPricePer = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtUpMinDayPricePer = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtGoldMacd = new System.Windows.Forms.CheckBox();
            this.txtDieMacd = new System.Windows.Forms.CheckBox();
            this.txtDieKdj = new System.Windows.Forms.CheckBox();
            this.txtGoldKdj = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMacdDown = new System.Windows.Forms.CheckBox();
            this.txtMacdUp = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(418, 554);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(82, 43);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "设置";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(34, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 31);
            this.label1.TabIndex = 6;
            this.label1.Text = "*股价涨跌幅超过(%)";
            // 
            // txtUDPer
            // 
            this.txtUDPer.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUDPer.Location = new System.Drawing.Point(283, 64);
            this.txtUDPer.Name = "txtUDPer";
            this.txtUDPer.Size = new System.Drawing.Size(217, 39);
            this.txtUDPer.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(42, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(222, 31);
            this.label2.TabIndex = 7;
            this.label2.Text = "股价上涨到价格(元)";
            // 
            // txtUpPrice
            // 
            this.txtUpPrice.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUpPrice.Location = new System.Drawing.Point(283, 112);
            this.txtUpPrice.Name = "txtUpPrice";
            this.txtUpPrice.Size = new System.Drawing.Size(217, 39);
            this.txtUpPrice.TabIndex = 8;
            // 
            // txtDownPrice
            // 
            this.txtDownPrice.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDownPrice.Location = new System.Drawing.Point(283, 162);
            this.txtDownPrice.Name = "txtDownPrice";
            this.txtDownPrice.Size = new System.Drawing.Size(217, 39);
            this.txtDownPrice.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(42, 163);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(222, 31);
            this.label3.TabIndex = 13;
            this.label3.Text = "股价下跌到价格(元)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(154, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 31);
            this.label5.TabIndex = 22;
            this.label5.Text = "交易账户";
            // 
            // txtAccount
            // 
            this.txtAccount.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtAccount.FormattingEnabled = true;
            this.txtAccount.Location = new System.Drawing.Point(283, 12);
            this.txtAccount.Name = "txtAccount";
            this.txtAccount.Size = new System.Drawing.Size(217, 39);
            this.txtAccount.TabIndex = 21;
            // 
            // btnRemindAll
            // 
            this.btnRemindAll.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRemindAll.Location = new System.Drawing.Point(12, 554);
            this.btnRemindAll.Name = "btnRemindAll";
            this.btnRemindAll.Size = new System.Drawing.Size(252, 43);
            this.btnRemindAll.TabIndex = 59;
            this.btnRemindAll.Text = "按安全价设置买入提醒";
            this.btnRemindAll.UseVisualStyleBackColor = true;
            this.btnRemindAll.Click += new System.EventHandler(this.btnRemindAll_Click);
            // 
            // btnOKAll
            // 
            this.btnOKAll.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOKAll.Location = new System.Drawing.Point(283, 554);
            this.btnOKAll.Name = "btnOKAll";
            this.btnOKAll.Size = new System.Drawing.Size(129, 43);
            this.btnOKAll.TabIndex = 60;
            this.btnOKAll.Text = "全部设置";
            this.btnOKAll.UseVisualStyleBackColor = true;
            this.btnOKAll.Click += new System.EventHandler(this.btnOKAll_Click);
            // 
            // txtUpAverage
            // 
            this.txtUpAverage.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUpAverage.Location = new System.Drawing.Point(283, 307);
            this.txtUpAverage.Name = "txtUpAverage";
            this.txtUpAverage.Size = new System.Drawing.Size(217, 39);
            this.txtUpAverage.TabIndex = 62;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(95, 309);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(169, 31);
            this.label4.TabIndex = 61;
            this.label4.Text = "*股价突破均线";
            // 
            // txtDownAverage
            // 
            this.txtDownAverage.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDownAverage.Location = new System.Drawing.Point(283, 355);
            this.txtDownAverage.Name = "txtDownAverage";
            this.txtDownAverage.Size = new System.Drawing.Size(217, 39);
            this.txtDownAverage.TabIndex = 64;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(95, 357);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(169, 31);
            this.label6.TabIndex = 63;
            this.label6.Text = "*股价跌破均线";
            // 
            // btnDownAverage
            // 
            this.btnDownAverage.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDownAverage.Location = new System.Drawing.Point(506, 356);
            this.btnDownAverage.Name = "btnDownAverage";
            this.btnDownAverage.Size = new System.Drawing.Size(58, 39);
            this.btnDownAverage.TabIndex = 65;
            this.btnDownAverage.Text = "预设";
            this.btnDownAverage.UseVisualStyleBackColor = true;
            this.btnDownAverage.Click += new System.EventHandler(this.btnDownAverage_Click);
            // 
            // btnUpAverage
            // 
            this.btnUpAverage.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpAverage.Location = new System.Drawing.Point(506, 308);
            this.btnUpAverage.Name = "btnUpAverage";
            this.btnUpAverage.Size = new System.Drawing.Size(58, 39);
            this.btnUpAverage.TabIndex = 66;
            this.btnUpAverage.Text = "预设";
            this.btnUpAverage.UseVisualStyleBackColor = true;
            this.btnUpAverage.Click += new System.EventHandler(this.btnUpAverage_Click);
            // 
            // btnUDPer
            // 
            this.btnUDPer.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUDPer.Location = new System.Drawing.Point(506, 64);
            this.btnUDPer.Name = "btnUDPer";
            this.btnUDPer.Size = new System.Drawing.Size(58, 39);
            this.btnUDPer.TabIndex = 67;
            this.btnUDPer.Text = "预设";
            this.btnUDPer.UseVisualStyleBackColor = true;
            this.btnUDPer.Click += new System.EventHandler(this.btnUDPer_Click);
            // 
            // btnUpAverageReverse
            // 
            this.btnUpAverageReverse.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpAverageReverse.Location = new System.Drawing.Point(506, 211);
            this.btnUpAverageReverse.Name = "btnUpAverageReverse";
            this.btnUpAverageReverse.Size = new System.Drawing.Size(58, 39);
            this.btnUpAverageReverse.TabIndex = 73;
            this.btnUpAverageReverse.Text = "预设";
            this.btnUpAverageReverse.UseVisualStyleBackColor = true;
            this.btnUpAverageReverse.Click += new System.EventHandler(this.btnUpAverageReverse_Click);
            // 
            // btnDownAverageReverse
            // 
            this.btnDownAverageReverse.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDownAverageReverse.Location = new System.Drawing.Point(506, 259);
            this.btnDownAverageReverse.Name = "btnDownAverageReverse";
            this.btnDownAverageReverse.Size = new System.Drawing.Size(58, 39);
            this.btnDownAverageReverse.TabIndex = 72;
            this.btnDownAverageReverse.Text = "预设";
            this.btnDownAverageReverse.UseVisualStyleBackColor = true;
            this.btnDownAverageReverse.Click += new System.EventHandler(this.btnDownAverageReverse_Click);
            // 
            // txtDownMaxDayPricePer
            // 
            this.txtDownMaxDayPricePer.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDownMaxDayPricePer.Location = new System.Drawing.Point(283, 258);
            this.txtDownMaxDayPricePer.Name = "txtDownMaxDayPricePer";
            this.txtDownMaxDayPricePer.Size = new System.Drawing.Size(217, 39);
            this.txtDownMaxDayPricePer.TabIndex = 71;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(34, 258);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(230, 31);
            this.label7.TabIndex = 70;
            this.label7.Text = "*当日最高价回落(%)";
            // 
            // txtUpMinDayPricePer
            // 
            this.txtUpMinDayPricePer.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUpMinDayPricePer.Location = new System.Drawing.Point(283, 210);
            this.txtUpMinDayPricePer.Name = "txtUpMinDayPricePer";
            this.txtUpMinDayPricePer.Size = new System.Drawing.Size(217, 39);
            this.txtUpMinDayPricePer.TabIndex = 69;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(34, 210);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(230, 31);
            this.label8.TabIndex = 68;
            this.label8.Text = "*当日最低价反弹(%)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(64, 411);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(195, 31);
            this.label9.TabIndex = 74;
            this.label9.Text = "*MACD快线趋势";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(154, 506);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(105, 31);
            this.label10.TabIndex = 77;
            this.label10.Text = "KDJ指标";
            // 
            // txtGoldMacd
            // 
            this.txtGoldMacd.AutoSize = true;
            this.txtGoldMacd.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGoldMacd.Location = new System.Drawing.Point(283, 411);
            this.txtGoldMacd.Name = "txtGoldMacd";
            this.txtGoldMacd.Size = new System.Drawing.Size(81, 35);
            this.txtGoldMacd.TabIndex = 78;
            this.txtGoldMacd.Text = "金叉";
            this.txtGoldMacd.UseVisualStyleBackColor = true;
            // 
            // txtDieMacd
            // 
            this.txtDieMacd.AutoSize = true;
            this.txtDieMacd.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDieMacd.Location = new System.Drawing.Point(380, 411);
            this.txtDieMacd.Name = "txtDieMacd";
            this.txtDieMacd.Size = new System.Drawing.Size(81, 35);
            this.txtDieMacd.TabIndex = 79;
            this.txtDieMacd.Text = "死叉";
            this.txtDieMacd.UseVisualStyleBackColor = true;
            // 
            // txtDieKdj
            // 
            this.txtDieKdj.AutoSize = true;
            this.txtDieKdj.Enabled = false;
            this.txtDieKdj.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDieKdj.Location = new System.Drawing.Point(380, 503);
            this.txtDieKdj.Name = "txtDieKdj";
            this.txtDieKdj.Size = new System.Drawing.Size(81, 35);
            this.txtDieKdj.TabIndex = 81;
            this.txtDieKdj.Text = "死叉";
            this.txtDieKdj.UseVisualStyleBackColor = true;
            // 
            // txtGoldKdj
            // 
            this.txtGoldKdj.AutoSize = true;
            this.txtGoldKdj.Enabled = false;
            this.txtGoldKdj.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGoldKdj.Location = new System.Drawing.Point(283, 503);
            this.txtGoldKdj.Name = "txtGoldKdj";
            this.txtGoldKdj.Size = new System.Drawing.Size(81, 35);
            this.txtGoldKdj.TabIndex = 80;
            this.txtGoldKdj.Text = "金叉";
            this.txtGoldKdj.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(77, 456);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(184, 31);
            this.label11.TabIndex = 82;
            this.label11.Text = "MACD柱状趋势";
            // 
            // txtMacdDown
            // 
            this.txtMacdDown.AutoSize = true;
            this.txtMacdDown.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMacdDown.Location = new System.Drawing.Point(380, 456);
            this.txtMacdDown.Name = "txtMacdDown";
            this.txtMacdDown.Size = new System.Drawing.Size(81, 35);
            this.txtMacdDown.TabIndex = 84;
            this.txtMacdDown.Text = "看空";
            this.txtMacdDown.UseVisualStyleBackColor = true;
            // 
            // txtMacdUp
            // 
            this.txtMacdUp.AutoSize = true;
            this.txtMacdUp.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMacdUp.Location = new System.Drawing.Point(283, 456);
            this.txtMacdUp.Name = "txtMacdUp";
            this.txtMacdUp.Size = new System.Drawing.Size(81, 35);
            this.txtMacdUp.TabIndex = 83;
            this.txtMacdUp.Text = "看多";
            this.txtMacdUp.UseVisualStyleBackColor = true;
            // 
            // RemindForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 617);
            this.Controls.Add(this.txtMacdDown);
            this.Controls.Add(this.txtMacdUp);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtDieKdj);
            this.Controls.Add(this.txtGoldKdj);
            this.Controls.Add(this.txtDieMacd);
            this.Controls.Add(this.txtGoldMacd);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnUpAverageReverse);
            this.Controls.Add(this.btnDownAverageReverse);
            this.Controls.Add(this.txtDownMaxDayPricePer);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtUpMinDayPricePer);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnUDPer);
            this.Controls.Add(this.btnUpAverage);
            this.Controls.Add(this.btnDownAverage);
            this.Controls.Add(this.txtDownAverage);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtUpAverage);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnOKAll);
            this.Controls.Add(this.btnRemindAll);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtAccount);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtDownPrice);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtUpPrice);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtUDPer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RemindForm";
            this.Text = "设置提醒";
            this.Load += new System.EventHandler(this.SetRemindForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUDPer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUpPrice;
        private System.Windows.Forms.TextBox txtDownPrice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox txtAccount;
        private System.Windows.Forms.Button btnRemindAll;
        private System.Windows.Forms.Button btnOKAll;
        private System.Windows.Forms.TextBox txtUpAverage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDownAverage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnDownAverage;
        private System.Windows.Forms.Button btnUpAverage;
        private System.Windows.Forms.Button btnUDPer;
        private System.Windows.Forms.Button btnUpAverageReverse;
        private System.Windows.Forms.Button btnDownAverageReverse;
        private System.Windows.Forms.TextBox txtDownMaxDayPricePer;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtUpMinDayPricePer;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox txtGoldMacd;
        private System.Windows.Forms.CheckBox txtDieMacd;
        private System.Windows.Forms.CheckBox txtDieKdj;
        private System.Windows.Forms.CheckBox txtGoldKdj;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox txtMacdDown;
        private System.Windows.Forms.CheckBox txtMacdUp;
    }
}