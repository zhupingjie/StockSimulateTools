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
            this.txtDebugMode = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.txtDebugMode);
            this.panel1.Controls.Add(this.label6);
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
            this.panel1.Size = new System.Drawing.Size(794, 523);
            this.panel1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(43, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(359, 46);
            this.label1.TabIndex = 11;
            this.label1.Text = "持股盈亏计算时间(秒)";
            // 
            // txtUpdateAccountStockProfitInterval
            // 
            this.txtUpdateAccountStockProfitInterval.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUpdateAccountStockProfitInterval.Location = new System.Drawing.Point(428, 154);
            this.txtUpdateAccountStockProfitInterval.Name = "txtUpdateAccountStockProfitInterval";
            this.txtUpdateAccountStockProfitInterval.Size = new System.Drawing.Size(331, 54);
            this.txtUpdateAccountStockProfitInterval.TabIndex = 10;
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(684, 442);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 41);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "保存";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(12, 372);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(390, 46);
            this.label5.TabIndex = 9;
            this.label5.Text = "买卖点价格浮动比例(%)";
            // 
            // txtRemindStockPriceFloatPer
            // 
            this.txtRemindStockPriceFloatPer.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRemindStockPriceFloatPer.Location = new System.Drawing.Point(428, 372);
            this.txtRemindStockPriceFloatPer.Name = "txtRemindStockPriceFloatPer";
            this.txtRemindStockPriceFloatPer.Size = new System.Drawing.Size(331, 54);
            this.txtRemindStockPriceFloatPer.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(43, 300);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(359, 46);
            this.label4.TabIndex = 7;
            this.label4.Text = "买卖策略提醒时间(秒)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(43, 225);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(359, 46);
            this.label3.TabIndex = 6;
            this.label3.Text = "财务指标采集时间(秒)";
            // 
            // txtRemindStockStrategyInterval
            // 
            this.txtRemindStockStrategyInterval.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRemindStockStrategyInterval.Location = new System.Drawing.Point(428, 300);
            this.txtRemindStockStrategyInterval.Name = "txtRemindStockStrategyInterval";
            this.txtRemindStockStrategyInterval.Size = new System.Drawing.Size(331, 54);
            this.txtRemindStockStrategyInterval.TabIndex = 5;
            // 
            // txtGatherStockMainTargetInterval
            // 
            this.txtGatherStockMainTargetInterval.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGatherStockMainTargetInterval.Location = new System.Drawing.Point(428, 225);
            this.txtGatherStockMainTargetInterval.Name = "txtGatherStockMainTargetInterval";
            this.txtGatherStockMainTargetInterval.Size = new System.Drawing.Size(331, 54);
            this.txtGatherStockMainTargetInterval.TabIndex = 4;
            // 
            // txtGatherStockPriceInterval
            // 
            this.txtGatherStockPriceInterval.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGatherStockPriceInterval.Location = new System.Drawing.Point(428, 81);
            this.txtGatherStockPriceInterval.Name = "txtGatherStockPriceInterval";
            this.txtGatherStockPriceInterval.Size = new System.Drawing.Size(331, 54);
            this.txtGatherStockPriceInterval.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(43, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(359, 46);
            this.label2.TabIndex = 2;
            this.label2.Text = "股票价格采集时间(秒)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(242, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(160, 46);
            this.label6.TabIndex = 12;
            this.label6.Text = "调试模式";
            // 
            // txtDebugMode
            // 
            this.txtDebugMode.AutoSize = true;
            this.txtDebugMode.Location = new System.Drawing.Point(428, 40);
            this.txtDebugMode.Name = "txtDebugMode";
            this.txtDebugMode.Size = new System.Drawing.Size(15, 14);
            this.txtDebugMode.TabIndex = 13;
            this.txtDebugMode.UseVisualStyleBackColor = true;
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 523);
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
        private System.Windows.Forms.Label label6;
    }
}