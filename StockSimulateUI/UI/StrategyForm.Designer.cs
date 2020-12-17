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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtRemindBS = new System.Windows.Forms.RadioButton();
            this.txtAutoBS = new System.Windows.Forms.RadioButton();
            this.txtAccountName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStockName = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtStockCode = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtStrategyName = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCalcuate = new System.Windows.Forms.Button();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1025, 97);
            this.panel1.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtRemindBS);
            this.groupBox2.Controls.Add(this.txtAutoBS);
            this.groupBox2.Controls.Add(this.txtAccountName);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtStockName);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.txtStockCode);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtStrategyName);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(825, 97);
            this.groupBox2.TabIndex = 62;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "股票策略";
            // 
            // txtRemindBS
            // 
            this.txtRemindBS.AutoSize = true;
            this.txtRemindBS.Checked = true;
            this.txtRemindBS.Location = new System.Drawing.Point(673, 62);
            this.txtRemindBS.Name = "txtRemindBS";
            this.txtRemindBS.Size = new System.Drawing.Size(108, 25);
            this.txtRemindBS.TabIndex = 59;
            this.txtRemindBS.TabStop = true;
            this.txtRemindBS.Text = "买卖点提醒";
            this.txtRemindBS.UseVisualStyleBackColor = true;
            // 
            // txtAutoBS
            // 
            this.txtAutoBS.AutoSize = true;
            this.txtAutoBS.Location = new System.Drawing.Point(672, 27);
            this.txtAutoBS.Name = "txtAutoBS";
            this.txtAutoBS.Size = new System.Drawing.Size(124, 25);
            this.txtAutoBS.TabIndex = 58;
            this.txtAutoBS.Text = "自动模拟交易";
            this.txtAutoBS.UseVisualStyleBackColor = true;
            // 
            // txtAccountName
            // 
            this.txtAccountName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtAccountName.Location = new System.Drawing.Point(404, 23);
            this.txtAccountName.Name = "txtAccountName";
            this.txtAccountName.Size = new System.Drawing.Size(219, 29);
            this.txtAccountName.TabIndex = 57;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(324, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 21);
            this.label1.TabIndex = 56;
            this.label1.Text = "交易账户";
            // 
            // txtStockName
            // 
            this.txtStockName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtStockName.Location = new System.Drawing.Point(107, 62);
            this.txtStockName.Name = "txtStockName";
            this.txtStockName.ReadOnly = true;
            this.txtStockName.Size = new System.Drawing.Size(163, 29);
            this.txtStockName.TabIndex = 54;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(17, 65);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(74, 21);
            this.label11.TabIndex = 55;
            this.label11.Text = "股票名称";
            // 
            // txtStockCode
            // 
            this.txtStockCode.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtStockCode.Location = new System.Drawing.Point(107, 24);
            this.txtStockCode.Name = "txtStockCode";
            this.txtStockCode.ReadOnly = true;
            this.txtStockCode.Size = new System.Drawing.Size(163, 29);
            this.txtStockCode.TabIndex = 52;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(27, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 21);
            this.label10.TabIndex = 53;
            this.label10.Text = "股票代码";
            // 
            // txtStrategyName
            // 
            this.txtStrategyName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtStrategyName.Items.AddRange(new object[] {
            "左侧交易",
            "右侧交易",
            "差价交易"});
            this.txtStrategyName.Location = new System.Drawing.Point(404, 61);
            this.txtStrategyName.Name = "txtStrategyName";
            this.txtStrategyName.Size = new System.Drawing.Size(219, 29);
            this.txtStrategyName.TabIndex = 47;
            this.txtStrategyName.SelectedIndexChanged += new System.EventHandler(this.txtName_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(321, 64);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(74, 21);
            this.label15.TabIndex = 46;
            this.label15.Text = "策略名称";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOK);
            this.groupBox1.Controls.Add(this.btnCalcuate);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(825, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 97);
            this.groupBox1.TabIndex = 61;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操作";
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(101, 38);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(87, 35);
            this.btnOK.TabIndex = 25;
            this.btnOK.Text = "保存策略";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCalcuate
            // 
            this.btnCalcuate.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCalcuate.Location = new System.Drawing.Point(8, 37);
            this.btnCalcuate.Name = "btnCalcuate";
            this.btnCalcuate.Size = new System.Drawing.Size(87, 37);
            this.btnCalcuate.TabIndex = 26;
            this.btnCalcuate.Text = "模拟计算";
            this.btnCalcuate.UseVisualStyleBackColor = true;
            this.btnCalcuate.Click += new System.EventHandler(this.btnCalcuate_Click);
            // 
            // pnlContainer
            // 
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(0, 97);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(1025, 551);
            this.pnlContainer.TabIndex = 3;
            // 
            // StrategyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1025, 648);
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StrategyForm";
            this.Text = "买卖策略";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlContainer;
        private System.Windows.Forms.Button btnCalcuate;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtStockName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtStockCode;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox txtStrategyName;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox txtAccountName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton txtRemindBS;
        private System.Windows.Forms.RadioButton txtAutoBS;
    }
}