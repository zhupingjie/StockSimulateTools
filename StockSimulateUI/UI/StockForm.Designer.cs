namespace StockPriceTools.UI
{
    partial class NewStockForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewStockForm));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtType = new System.Windows.Forms.ComboBox();
            this.txtDay = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSType = new System.Windows.Forms.ComboBox();
            this.txtSTypeValue = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(277, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(155, 54);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "股票代码";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(573, 13);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(68, 53);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确认";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtType
            // 
            this.txtType.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtType.FormattingEnabled = true;
            this.txtType.Items.AddRange(new object[] {
            "SZ",
            "SH",
            "ZS"});
            this.txtType.Location = new System.Drawing.Point(197, 12);
            this.txtType.Name = "txtType";
            this.txtType.Size = new System.Drawing.Size(68, 54);
            this.txtType.TabIndex = 2;
            this.txtType.Text = "SZ";
            // 
            // txtDay
            // 
            this.txtDay.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDay.FormattingEnabled = true;
            this.txtDay.Items.AddRange(new object[] {
            "0",
            "1"});
            this.txtDay.Location = new System.Drawing.Point(505, 12);
            this.txtDay.Name = "txtDay";
            this.txtDay.Size = new System.Drawing.Size(50, 54);
            this.txtDay.TabIndex = 3;
            this.txtDay.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(439, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 41);
            this.label1.TabIndex = 4;
            this.label1.Text = "T+";
            // 
            // txtSType
            // 
            this.txtSType.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSType.FormattingEnabled = true;
            this.txtSType.Items.AddRange(new object[] {
            "沪深股",
            "基金(ETF)"});
            this.txtSType.Location = new System.Drawing.Point(12, 12);
            this.txtSType.Name = "txtSType";
            this.txtSType.Size = new System.Drawing.Size(173, 54);
            this.txtSType.TabIndex = 5;
            this.txtSType.Text = "沪深股";
            // 
            // txtSTypeValue
            // 
            this.txtSTypeValue.Location = new System.Drawing.Point(13, 73);
            this.txtSTypeValue.Name = "txtSTypeValue";
            this.txtSTypeValue.Size = new System.Drawing.Size(100, 21);
            this.txtSTypeValue.TabIndex = 6;
            this.txtSTypeValue.Visible = false;
            // 
            // NewStockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 90);
            this.Controls.Add(this.txtSTypeValue);
            this.Controls.Add(this.txtSType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDay);
            this.Controls.Add(this.txtType);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.textBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NewStockForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "添加股票代码";
            this.Load += new System.EventHandler(this.NewStockForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NewStockForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox txtType;
        private System.Windows.Forms.ComboBox txtDay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox txtSType;
        private System.Windows.Forms.TextBox txtSTypeValue;
    }
}