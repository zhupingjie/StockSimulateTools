namespace StockSimulateUI.UI
{
    partial class ConfigForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtLoadGlobalConfigInterval = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtLoadMessageInterval = new System.Windows.Forms.TextBox();
            this.txtRemindNoticeByMessage = new System.Windows.Forms.CheckBox();
            this.txtRemindNoticeByEmail = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtKeepStockAssistTargetDays = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtAccount = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtUpdateStockAssistTargetInterval = new System.Windows.Forms.TextBox();
            this.txtDebugMode = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUpdateAccountStockProfitInterval = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRemindStockPriceFloatPer = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRemindStockStrategyInterval = new System.Windows.Forms.TextBox();
            this.txtGatherStockMainTargetInterval = new System.Windows.Forms.TextBox();
            this.txtGatherStockPriceInterval = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtGatherHistoryPriceStartDate = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtGatherHistoryPriceEndDate = new System.Windows.Forms.TextBox();
            this.txtGatherHistoryPrice = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.txtGatherHistoryPrice);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.txtGatherHistoryPriceEndDate);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtGatherHistoryPriceStartDate);
            this.panel1.Controls.Add(this.txtLoadGlobalConfigInterval);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.txtLoadMessageInterval);
            this.panel1.Controls.Add(this.txtRemindNoticeByMessage);
            this.panel1.Controls.Add(this.txtRemindNoticeByEmail);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtKeepStockAssistTargetDays);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtAccount);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtUpdateStockAssistTargetInterval);
            this.panel1.Controls.Add(this.txtDebugMode);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtUpdateAccountStockProfitInterval);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtRemindStockPriceFloatPer);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtRemindStockStrategyInterval);
            this.panel1.Controls.Add(this.txtGatherStockMainTargetInterval);
            this.panel1.Controls.Add(this.txtGatherStockPriceInterval);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(660, 638);
            this.panel1.TabIndex = 4;
            // 
            // txtLoadGlobalConfigInterval
            // 
            this.txtLoadGlobalConfigInterval.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLoadGlobalConfigInterval.Location = new System.Drawing.Point(258, 52);
            this.txtLoadGlobalConfigInterval.Name = "txtLoadGlobalConfigInterval";
            this.txtLoadGlobalConfigInterval.Size = new System.Drawing.Size(379, 32);
            this.txtLoadGlobalConfigInterval.TabIndex = 33;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(57, 53);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(195, 25);
            this.label12.TabIndex = 32;
            this.label12.Text = "全局配置读取时间(秒)";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(50, 413);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(195, 25);
            this.label11.TabIndex = 31;
            this.label11.Text = "通知消息读取时间(秒)";
            // 
            // txtLoadMessageInterval
            // 
            this.txtLoadMessageInterval.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLoadMessageInterval.Location = new System.Drawing.Point(258, 410);
            this.txtLoadMessageInterval.Name = "txtLoadMessageInterval";
            this.txtLoadMessageInterval.Size = new System.Drawing.Size(379, 32);
            this.txtLoadMessageInterval.TabIndex = 30;
            // 
            // txtRemindNoticeByMessage
            // 
            this.txtRemindNoticeByMessage.AutoSize = true;
            this.txtRemindNoticeByMessage.Checked = true;
            this.txtRemindNoticeByMessage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.txtRemindNoticeByMessage.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRemindNoticeByMessage.Location = new System.Drawing.Point(371, 546);
            this.txtRemindNoticeByMessage.Name = "txtRemindNoticeByMessage";
            this.txtRemindNoticeByMessage.Size = new System.Drawing.Size(107, 29);
            this.txtRemindNoticeByMessage.TabIndex = 29;
            this.txtRemindNoticeByMessage.Text = "系统消息";
            this.txtRemindNoticeByMessage.UseVisualStyleBackColor = true;
            // 
            // txtRemindNoticeByEmail
            // 
            this.txtRemindNoticeByEmail.AutoSize = true;
            this.txtRemindNoticeByEmail.Checked = true;
            this.txtRemindNoticeByEmail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.txtRemindNoticeByEmail.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRemindNoticeByEmail.Location = new System.Drawing.Point(258, 546);
            this.txtRemindNoticeByEmail.Name = "txtRemindNoticeByEmail";
            this.txtRemindNoticeByEmail.Size = new System.Drawing.Size(69, 29);
            this.txtRemindNoticeByEmail.TabIndex = 28;
            this.txtRemindNoticeByEmail.Text = "邮件";
            this.txtRemindNoticeByEmail.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(80, 546);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(164, 25);
            this.label10.TabIndex = 27;
            this.label10.Text = "提醒消息通知方式";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(53, 272);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(195, 25);
            this.label9.TabIndex = 26;
            this.label9.Text = "辅助指标保留天数(天)";
            // 
            // txtKeepStockAssistTargetDays
            // 
            this.txtKeepStockAssistTargetDays.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtKeepStockAssistTargetDays.Location = new System.Drawing.Point(258, 269);
            this.txtKeepStockAssistTargetDays.Name = "txtKeepStockAssistTargetDays";
            this.txtKeepStockAssistTargetDays.Size = new System.Drawing.Size(379, 32);
            this.txtKeepStockAssistTargetDays.TabIndex = 25;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(126, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(126, 25);
            this.label8.TabIndex = 24;
            this.label8.Text = "当前交易账户";
            // 
            // txtAccount
            // 
            this.txtAccount.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtAccount.FormattingEnabled = true;
            this.txtAccount.Location = new System.Drawing.Point(258, 11);
            this.txtAccount.Name = "txtAccount";
            this.txtAccount.Size = new System.Drawing.Size(379, 33);
            this.txtAccount.TabIndex = 23;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(50, 224);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(195, 25);
            this.label7.TabIndex = 15;
            this.label7.Text = "辅助指标采集时间(秒)";
            // 
            // txtUpdateStockAssistTargetInterval
            // 
            this.txtUpdateStockAssistTargetInterval.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUpdateStockAssistTargetInterval.Location = new System.Drawing.Point(258, 224);
            this.txtUpdateStockAssistTargetInterval.Name = "txtUpdateStockAssistTargetInterval";
            this.txtUpdateStockAssistTargetInterval.Size = new System.Drawing.Size(379, 32);
            this.txtUpdateStockAssistTargetInterval.TabIndex = 14;
            // 
            // txtDebugMode
            // 
            this.txtDebugMode.AutoSize = true;
            this.txtDebugMode.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDebugMode.Location = new System.Drawing.Point(258, 590);
            this.txtDebugMode.Name = "txtDebugMode";
            this.txtDebugMode.Size = new System.Drawing.Size(107, 29);
            this.txtDebugMode.TabIndex = 13;
            this.txtDebugMode.Text = "调试模式";
            this.txtDebugMode.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(53, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 25);
            this.label1.TabIndex = 11;
            this.label1.Text = "持股盈亏计算时间(秒)";
            // 
            // txtUpdateAccountStockProfitInterval
            // 
            this.txtUpdateAccountStockProfitInterval.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUpdateAccountStockProfitInterval.Location = new System.Drawing.Point(258, 135);
            this.txtUpdateAccountStockProfitInterval.Name = "txtUpdateAccountStockProfitInterval";
            this.txtUpdateAccountStockProfitInterval.Size = new System.Drawing.Size(379, 32);
            this.txtUpdateAccountStockProfitInterval.TabIndex = 10;
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(555, 546);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(82, 73);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "保存";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(33, 364);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(212, 25);
            this.label5.TabIndex = 9;
            this.label5.Text = "买卖点价格浮动比例(%)";
            // 
            // txtRemindStockPriceFloatPer
            // 
            this.txtRemindStockPriceFloatPer.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRemindStockPriceFloatPer.Location = new System.Drawing.Point(257, 362);
            this.txtRemindStockPriceFloatPer.Name = "txtRemindStockPriceFloatPer";
            this.txtRemindStockPriceFloatPer.Size = new System.Drawing.Size(380, 32);
            this.txtRemindStockPriceFloatPer.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(50, 319);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(195, 25);
            this.label4.TabIndex = 7;
            this.label4.Text = "买卖策略提醒时间(秒)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(12, 181);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(233, 25);
            this.label3.TabIndex = 6;
            this.label3.Text = "财务机构研报采集时间(时)";
            // 
            // txtRemindStockStrategyInterval
            // 
            this.txtRemindStockStrategyInterval.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRemindStockStrategyInterval.Location = new System.Drawing.Point(257, 316);
            this.txtRemindStockStrategyInterval.Name = "txtRemindStockStrategyInterval";
            this.txtRemindStockStrategyInterval.Size = new System.Drawing.Size(380, 32);
            this.txtRemindStockStrategyInterval.TabIndex = 5;
            // 
            // txtGatherStockMainTargetInterval
            // 
            this.txtGatherStockMainTargetInterval.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGatherStockMainTargetInterval.Location = new System.Drawing.Point(258, 179);
            this.txtGatherStockMainTargetInterval.Name = "txtGatherStockMainTargetInterval";
            this.txtGatherStockMainTargetInterval.Size = new System.Drawing.Size(379, 32);
            this.txtGatherStockMainTargetInterval.TabIndex = 4;
            // 
            // txtGatherStockPriceInterval
            // 
            this.txtGatherStockPriceInterval.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGatherStockPriceInterval.Location = new System.Drawing.Point(258, 93);
            this.txtGatherStockPriceInterval.Name = "txtGatherStockPriceInterval";
            this.txtGatherStockPriceInterval.Size = new System.Drawing.Size(379, 32);
            this.txtGatherStockPriceInterval.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(53, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(195, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "股票价格采集时间(秒)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(42, 458);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(202, 25);
            this.label6.TabIndex = 35;
            this.label6.Text = "读取历史股价开始日期";
            // 
            // txtGatherHistoryPriceStartDate
            // 
            this.txtGatherHistoryPriceStartDate.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGatherHistoryPriceStartDate.Location = new System.Drawing.Point(258, 456);
            this.txtGatherHistoryPriceStartDate.Name = "txtGatherHistoryPriceStartDate";
            this.txtGatherHistoryPriceStartDate.Size = new System.Drawing.Size(379, 32);
            this.txtGatherHistoryPriceStartDate.TabIndex = 34;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(42, 503);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(202, 25);
            this.label13.TabIndex = 37;
            this.label13.Text = "读取历史股价截至日期";
            // 
            // txtGatherHistoryPriceEndDate
            // 
            this.txtGatherHistoryPriceEndDate.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGatherHistoryPriceEndDate.Location = new System.Drawing.Point(257, 503);
            this.txtGatherHistoryPriceEndDate.Name = "txtGatherHistoryPriceEndDate";
            this.txtGatherHistoryPriceEndDate.Size = new System.Drawing.Size(380, 32);
            this.txtGatherHistoryPriceEndDate.TabIndex = 36;
            // 
            // txtGatherHistoryPrice
            // 
            this.txtGatherHistoryPrice.AutoSize = true;
            this.txtGatherHistoryPrice.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGatherHistoryPrice.Location = new System.Drawing.Point(371, 590);
            this.txtGatherHistoryPrice.Name = "txtGatherHistoryPrice";
            this.txtGatherHistoryPrice.Size = new System.Drawing.Size(145, 29);
            this.txtGatherHistoryPrice.TabIndex = 38;
            this.txtGatherHistoryPrice.Text = "采集历史数据";
            this.txtGatherHistoryPrice.UseVisualStyleBackColor = true;
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 638);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConfigForm";
            this.Text = "设置";
            this.Load += new System.EventHandler(this.ConfigForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRemindStockStrategyInterval;
        private System.Windows.Forms.TextBox txtGatherStockMainTargetInterval;
        private System.Windows.Forms.TextBox txtGatherStockPriceInterval;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRemindStockPriceFloatPer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUpdateAccountStockProfitInterval;
        private System.Windows.Forms.CheckBox txtDebugMode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtUpdateStockAssistTargetInterval;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox txtAccount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtKeepStockAssistTargetDays;
        private System.Windows.Forms.CheckBox txtRemindNoticeByMessage;
        private System.Windows.Forms.CheckBox txtRemindNoticeByEmail;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtLoadMessageInterval;
        private System.Windows.Forms.TextBox txtLoadGlobalConfigInterval;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox txtGatherHistoryPrice;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtGatherHistoryPriceEndDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtGatherHistoryPriceStartDate;
    }
}