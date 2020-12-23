namespace StockSimulateUI.UI
{
    partial class DebugForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugForm));
            this.txtStockPrice = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtDealTime = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAccount = new System.Windows.Forms.ComboBox();
            this.txtStockCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClearDebug = new System.Windows.Forms.Button();
            this.btnGatherHisPrice = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtStockPrice
            // 
            this.txtStockPrice.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtStockPrice.Location = new System.Drawing.Point(307, 163);
            this.txtStockPrice.Name = "txtStockPrice";
            this.txtStockPrice.Size = new System.Drawing.Size(373, 54);
            this.txtStockPrice.TabIndex = 5;
            this.txtStockPrice.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(71, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(230, 46);
            this.label2.TabIndex = 4;
            this.label2.Text = "模拟最新股价";
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(605, 317);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 41);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "模拟";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtDealTime
            // 
            this.txtDealTime.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDealTime.Location = new System.Drawing.Point(307, 235);
            this.txtDealTime.Name = "txtDealTime";
            this.txtDealTime.Size = new System.Drawing.Size(373, 54);
            this.txtDealTime.TabIndex = 8;
            this.txtDealTime.Text = "2020-12-20 13:00:00";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(71, 235);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 46);
            this.label1.TabIndex = 7;
            this.label1.Text = "模拟结算时间";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(141, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(160, 46);
            this.label5.TabIndex = 22;
            this.label5.Text = "交易账户";
            // 
            // txtAccount
            // 
            this.txtAccount.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtAccount.FormattingEnabled = true;
            this.txtAccount.Location = new System.Drawing.Point(307, 22);
            this.txtAccount.Name = "txtAccount";
            this.txtAccount.Size = new System.Drawing.Size(373, 54);
            this.txtAccount.TabIndex = 21;
            // 
            // txtStockCode
            // 
            this.txtStockCode.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtStockCode.Location = new System.Drawing.Point(307, 91);
            this.txtStockCode.Name = "txtStockCode";
            this.txtStockCode.Size = new System.Drawing.Size(373, 54);
            this.txtStockCode.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(71, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(230, 46);
            this.label3.TabIndex = 24;
            this.label3.Text = "模拟股票代码";
            // 
            // btnClearDebug
            // 
            this.btnClearDebug.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClearDebug.Location = new System.Drawing.Point(307, 317);
            this.btnClearDebug.Name = "btnClearDebug";
            this.btnClearDebug.Size = new System.Drawing.Size(127, 41);
            this.btnClearDebug.TabIndex = 25;
            this.btnClearDebug.Text = "清空模拟数据";
            this.btnClearDebug.UseVisualStyleBackColor = true;
            this.btnClearDebug.Click += new System.EventHandler(this.btnClearDebug_Click);
            // 
            // btnGatherHisPrice
            // 
            this.btnGatherHisPrice.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGatherHisPrice.Location = new System.Drawing.Point(440, 317);
            this.btnGatherHisPrice.Name = "btnGatherHisPrice";
            this.btnGatherHisPrice.Size = new System.Drawing.Size(127, 41);
            this.btnGatherHisPrice.TabIndex = 26;
            this.btnGatherHisPrice.Text = "采集历史股价";
            this.btnGatherHisPrice.UseVisualStyleBackColor = true;
            this.btnGatherHisPrice.Click += new System.EventHandler(this.btnGatherHisPrice_Click);
            // 
            // DebugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 382);
            this.Controls.Add(this.btnGatherHisPrice);
            this.Controls.Add(this.btnClearDebug);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtStockCode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtAccount);
            this.Controls.Add(this.txtDealTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtStockPrice);
            this.Controls.Add(this.label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DebugForm";
            this.Text = "股价策略调试";
            this.Load += new System.EventHandler(this.DebugForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtStockPrice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtDealTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox txtAccount;
        private System.Windows.Forms.TextBox txtStockCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnClearDebug;
        private System.Windows.Forms.Button btnGatherHisPrice;
    }
}