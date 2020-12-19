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
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(517, 298);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(82, 53);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "设置";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(42, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(320, 46);
            this.label1.TabIndex = 6;
            this.label1.Text = "股价涨跌幅超过(%)";
            // 
            // txtUDPer
            // 
            this.txtUDPer.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUDPer.Location = new System.Drawing.Point(382, 83);
            this.txtUDPer.Name = "txtUDPer";
            this.txtUDPer.Size = new System.Drawing.Size(217, 54);
            this.txtUDPer.TabIndex = 5;
            this.txtUDPer.Text = "1.5,3,5,9";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(42, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(324, 46);
            this.label2.TabIndex = 7;
            this.label2.Text = "股价上涨到价格(元)";
            // 
            // txtUpPrice
            // 
            this.txtUpPrice.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUpPrice.Location = new System.Drawing.Point(382, 150);
            this.txtUpPrice.Name = "txtUpPrice";
            this.txtUpPrice.Size = new System.Drawing.Size(217, 54);
            this.txtUpPrice.TabIndex = 8;
            // 
            // txtDownPrice
            // 
            this.txtDownPrice.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDownPrice.Location = new System.Drawing.Point(382, 223);
            this.txtDownPrice.Name = "txtDownPrice";
            this.txtDownPrice.Size = new System.Drawing.Size(217, 54);
            this.txtDownPrice.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(42, 223);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(324, 46);
            this.label3.TabIndex = 13;
            this.label3.Text = "股价下跌到价格(元)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(202, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(160, 46);
            this.label5.TabIndex = 22;
            this.label5.Text = "交易账户";
            // 
            // txtAccount
            // 
            this.txtAccount.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtAccount.FormattingEnabled = true;
            this.txtAccount.Location = new System.Drawing.Point(382, 12);
            this.txtAccount.Name = "txtAccount";
            this.txtAccount.Size = new System.Drawing.Size(217, 54);
            this.txtAccount.TabIndex = 21;
            // 
            // btnRemindAll
            // 
            this.btnRemindAll.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRemindAll.Location = new System.Drawing.Point(50, 298);
            this.btnRemindAll.Name = "btnRemindAll";
            this.btnRemindAll.Size = new System.Drawing.Size(296, 53);
            this.btnRemindAll.TabIndex = 59;
            this.btnRemindAll.Text = "按安全价设置买入提醒";
            this.btnRemindAll.UseVisualStyleBackColor = true;
            this.btnRemindAll.Click += new System.EventHandler(this.btnRemindAll_Click);
            // 
            // btnOKAll
            // 
            this.btnOKAll.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOKAll.Location = new System.Drawing.Point(382, 298);
            this.btnOKAll.Name = "btnOKAll";
            this.btnOKAll.Size = new System.Drawing.Size(129, 53);
            this.btnOKAll.TabIndex = 60;
            this.btnOKAll.Text = "全部设置";
            this.btnOKAll.UseVisualStyleBackColor = true;
            this.btnOKAll.Click += new System.EventHandler(this.btnOKAll_Click);
            // 
            // RemindForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 369);
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
    }
}