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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRemindStockStrategyInterval = new System.Windows.Forms.TextBox();
            this.txtGatherStockMainTargetInterval = new System.Windows.Forms.TextBox();
            this.txtGatherStockPriceInterval = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLoadStockDataInterval = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRemindStockPriceFloatPer = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtRemindStockPriceFloatPer);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtRemindStockStrategyInterval);
            this.panel1.Controls.Add(this.txtGatherStockMainTargetInterval);
            this.panel1.Controls.Add(this.txtGatherStockPriceInterval);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtLoadStockDataInterval);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(794, 390);
            this.panel1.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(12, 232);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(359, 46);
            this.label4.TabIndex = 7;
            this.label4.Text = "买卖策略提醒时间(秒)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(12, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(359, 46);
            this.label3.TabIndex = 6;
            this.label3.Text = "主要指标加载时间(秒)";
            // 
            // txtRemindStockStrategyInterval
            // 
            this.txtRemindStockStrategyInterval.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRemindStockStrategyInterval.Location = new System.Drawing.Point(428, 232);
            this.txtRemindStockStrategyInterval.Name = "txtRemindStockStrategyInterval";
            this.txtRemindStockStrategyInterval.Size = new System.Drawing.Size(331, 54);
            this.txtRemindStockStrategyInterval.TabIndex = 5;
            // 
            // txtGatherStockMainTargetInterval
            // 
            this.txtGatherStockMainTargetInterval.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGatherStockMainTargetInterval.Location = new System.Drawing.Point(428, 157);
            this.txtGatherStockMainTargetInterval.Name = "txtGatherStockMainTargetInterval";
            this.txtGatherStockMainTargetInterval.Size = new System.Drawing.Size(331, 54);
            this.txtGatherStockMainTargetInterval.TabIndex = 4;
            // 
            // txtGatherStockPriceInterval
            // 
            this.txtGatherStockPriceInterval.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGatherStockPriceInterval.Location = new System.Drawing.Point(428, 83);
            this.txtGatherStockPriceInterval.Name = "txtGatherStockPriceInterval";
            this.txtGatherStockPriceInterval.Size = new System.Drawing.Size(331, 54);
            this.txtGatherStockPriceInterval.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(12, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(359, 46);
            this.label2.TabIndex = 2;
            this.label2.Text = "股票价格加载时间(秒)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(394, 46);
            this.label1.TabIndex = 1;
            this.label1.Text = "自选股数据加载时间(秒)";
            // 
            // txtLoadStockDataInterval
            // 
            this.txtLoadStockDataInterval.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLoadStockDataInterval.Location = new System.Drawing.Point(428, 12);
            this.txtLoadStockDataInterval.Name = "txtLoadStockDataInterval";
            this.txtLoadStockDataInterval.Size = new System.Drawing.Size(331, 54);
            this.txtLoadStockDataInterval.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 390);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(794, 68);
            this.panel2.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(603, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 41);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(684, 15);
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
            this.label5.Location = new System.Drawing.Point(12, 304);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(390, 46);
            this.label5.TabIndex = 9;
            this.label5.Text = "买卖点价格浮动比例(%)";
            // 
            // txtRemindStockPriceFloatPer
            // 
            this.txtRemindStockPriceFloatPer.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRemindStockPriceFloatPer.Location = new System.Drawing.Point(428, 304);
            this.txtRemindStockPriceFloatPer.Name = "txtRemindStockPriceFloatPer";
            this.txtRemindStockPriceFloatPer.Size = new System.Drawing.Size(331, 54);
            this.txtRemindStockPriceFloatPer.TabIndex = 8;
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 458);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "ConfigForm";
            this.Text = "设置";
            this.Load += new System.EventHandler(this.ConfigForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLoadStockDataInterval;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRemindStockPriceFloatPer;
    }
}