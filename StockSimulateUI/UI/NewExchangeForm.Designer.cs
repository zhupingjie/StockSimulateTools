namespace StockSimulateUI.UI
{
    partial class NewExchangeForm
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
            this.txtDealQty = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDealPrice = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDealType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDealAmount = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtDealQty
            // 
            this.txtDealQty.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDealQty.Location = new System.Drawing.Point(202, 154);
            this.txtDealQty.Name = "txtDealQty";
            this.txtDealQty.Size = new System.Drawing.Size(217, 54);
            this.txtDealQty.TabIndex = 13;
            this.txtDealQty.Text = "100";
            this.txtDealQty.TextChanged += new System.EventHandler(this.txtDealQty_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(19, 157);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 46);
            this.label2.TabIndex = 12;
            this.label2.Text = "交易数量";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(19, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 46);
            this.label1.TabIndex = 11;
            this.label1.Text = "交易价格";
            // 
            // txtDealPrice
            // 
            this.txtDealPrice.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDealPrice.Location = new System.Drawing.Point(202, 87);
            this.txtDealPrice.Name = "txtDealPrice";
            this.txtDealPrice.Size = new System.Drawing.Size(217, 54);
            this.txtDealPrice.TabIndex = 10;
            this.txtDealPrice.Text = "0";
            this.txtDealPrice.TextChanged += new System.EventHandler(this.txtDealPrice_TextChanged);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(288, 292);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(131, 53);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "确认";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(19, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 46);
            this.label4.TabIndex = 17;
            this.label4.Text = "交易类型";
            // 
            // txtDealType
            // 
            this.txtDealType.Enabled = false;
            this.txtDealType.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDealType.FormattingEnabled = true;
            this.txtDealType.Items.AddRange(new object[] {
            "买入",
            "卖出"});
            this.txtDealType.Location = new System.Drawing.Point(202, 17);
            this.txtDealType.Name = "txtDealType";
            this.txtDealType.Size = new System.Drawing.Size(217, 54);
            this.txtDealType.TabIndex = 16;
            this.txtDealType.Text = "买入";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(19, 223);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 46);
            this.label3.TabIndex = 14;
            this.label3.Text = "交易市值";
            // 
            // txtDealAmount
            // 
            this.txtDealAmount.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDealAmount.Location = new System.Drawing.Point(202, 223);
            this.txtDealAmount.Name = "txtDealAmount";
            this.txtDealAmount.ReadOnly = true;
            this.txtDealAmount.Size = new System.Drawing.Size(217, 54);
            this.txtDealAmount.TabIndex = 15;
            this.txtDealAmount.Text = "0";
            // 
            // NewExchangeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 370);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtDealType);
            this.Controls.Add(this.txtDealAmount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDealQty);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDealPrice);
            this.Controls.Add(this.btnOK);
            this.Name = "NewExchangeForm";
            this.Text = "买卖交易";
            this.Load += new System.EventHandler(this.NewExchangeForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtDealQty;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDealPrice;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox txtDealType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDealAmount;
    }
}