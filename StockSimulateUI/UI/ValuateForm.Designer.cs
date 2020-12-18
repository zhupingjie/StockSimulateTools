namespace StockSimulateUI.UI
{
    partial class ValuateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ValuateForm));
            this.btnOK = new System.Windows.Forms.Button();
            this.txtNetProfit = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtWantNetProfit = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCapital = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTTM = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtWantPE = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.txtYetNetProfit = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtWantProfitGrow = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtYetGrow = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtWantEPS = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtYetEPS = new System.Windows.Forms.TextBox();
            this.txtWantAmount = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtSafeRate = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txtLostNetProfit = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtYetLostNetProfit = new System.Windows.Forms.TextBox();
            this.btnValuateAll = new System.Windows.Forms.Button();
            this.txtAdvise = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(597, 490);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(162, 42);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "保存预测结果";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtNetProfit
            // 
            this.txtNetProfit.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtNetProfit.Location = new System.Drawing.Point(226, 165);
            this.txtNetProfit.Name = "txtNetProfit";
            this.txtNetProfit.ReadOnly = true;
            this.txtNetProfit.Size = new System.Drawing.Size(162, 34);
            this.txtNetProfit.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(47, 165);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 28);
            this.label1.TabIndex = 18;
            this.label1.Text = "当前净利润(亿元)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(418, 213);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(173, 28);
            this.label6.TabIndex = 22;
            this.label6.Text = "预测净利润(亿元)";
            // 
            // txtWantNetProfit
            // 
            this.txtWantNetProfit.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtWantNetProfit.Location = new System.Drawing.Point(597, 211);
            this.txtWantNetProfit.Name = "txtWantNetProfit";
            this.txtWantNetProfit.Size = new System.Drawing.Size(162, 34);
            this.txtWantNetProfit.TabIndex = 4;
            this.txtWantNetProfit.TextChanged += new System.EventHandler(this.txtWantNetProfit_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(460, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 28);
            this.label3.TabIndex = 25;
            this.label3.Text = "总股本(亿股)";
            // 
            // txtCapital
            // 
            this.txtCapital.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCapital.Location = new System.Drawing.Point(597, 19);
            this.txtCapital.Name = "txtCapital";
            this.txtCapital.ReadOnly = true;
            this.txtCapital.Size = new System.Drawing.Size(162, 34);
            this.txtCapital.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(47, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 28);
            this.label2.TabIndex = 27;
            this.label2.Text = "当前市盈率(TTM)";
            // 
            // txtTTM
            // 
            this.txtTTM.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTTM.Location = new System.Drawing.Point(226, 114);
            this.txtTTM.Name = "txtTTM";
            this.txtTTM.ReadOnly = true;
            this.txtTTM.Size = new System.Drawing.Size(162, 34);
            this.txtTTM.TabIndex = 26;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(435, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(156, 28);
            this.label4.TabIndex = 29;
            this.label4.Text = "预测市盈率(PE)";
            // 
            // txtWantPE
            // 
            this.txtWantPE.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtWantPE.Location = new System.Drawing.Point(597, 114);
            this.txtWantPE.Name = "txtWantPE";
            this.txtWantPE.Size = new System.Drawing.Size(162, 34);
            this.txtWantPE.TabIndex = 2;
            this.txtWantPE.TextChanged += new System.EventHandler(this.txtWantPE_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(89, 396);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 28);
            this.label5.TabIndex = 31;
            this.label5.Text = "当前股价(元)";
            // 
            // txtPrice
            // 
            this.txtPrice.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPrice.Location = new System.Drawing.Point(226, 396);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.ReadOnly = true;
            this.txtPrice.Size = new System.Drawing.Size(162, 34);
            this.txtPrice.TabIndex = 30;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(460, 398);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(131, 28);
            this.label7.TabIndex = 33;
            this.label7.Text = "预测股价(元)";
            // 
            // txtTarget
            // 
            this.txtTarget.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTarget.Location = new System.Drawing.Point(597, 398);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.ReadOnly = true;
            this.txtTarget.Size = new System.Drawing.Size(162, 34);
            this.txtTarget.TabIndex = 8;
            // 
            // txtYetNetProfit
            // 
            this.txtYetNetProfit.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtYetNetProfit.Location = new System.Drawing.Point(226, 211);
            this.txtYetNetProfit.Name = "txtYetNetProfit";
            this.txtYetNetProfit.ReadOnly = true;
            this.txtYetNetProfit.Size = new System.Drawing.Size(162, 34);
            this.txtYetNetProfit.TabIndex = 34;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(124, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(96, 28);
            this.label9.TabIndex = 37;
            this.label9.Text = "股票名称";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtName.Location = new System.Drawing.Point(226, 17);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(162, 34);
            this.txtName.TabIndex = 36;
            // 
            // txtWantProfitGrow
            // 
            this.txtWantProfitGrow.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtWantProfitGrow.Location = new System.Drawing.Point(597, 66);
            this.txtWantProfitGrow.Name = "txtWantProfitGrow";
            this.txtWantProfitGrow.Size = new System.Drawing.Size(162, 34);
            this.txtWantProfitGrow.TabIndex = 1;
            this.txtWantProfitGrow.TextChanged += new System.EventHandler(this.txtWantProfitGrow_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(399, 70);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(192, 28);
            this.label10.TabIndex = 39;
            this.label10.Text = "预测今年增长率(%)";
            // 
            // txtYetGrow
            // 
            this.txtYetGrow.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtYetGrow.Location = new System.Drawing.Point(226, 66);
            this.txtYetGrow.Name = "txtYetGrow";
            this.txtYetGrow.ReadOnly = true;
            this.txtYetGrow.Size = new System.Drawing.Size(162, 34);
            this.txtYetGrow.TabIndex = 42;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(70, 66);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(150, 28);
            this.label11.TabIndex = 41;
            this.label11.Text = "去年增长率(%)";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(68, 302);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(152, 28);
            this.label12.TabIndex = 44;
            this.label12.Text = "当前市值(亿元)";
            // 
            // txtAmount
            // 
            this.txtAmount.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtAmount.Location = new System.Drawing.Point(226, 300);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.ReadOnly = true;
            this.txtAmount.Size = new System.Drawing.Size(162, 34);
            this.txtAmount.TabIndex = 43;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(418, 352);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(173, 28);
            this.label13.TabIndex = 48;
            this.label13.Text = "预测每股收益(元)";
            // 
            // txtWantEPS
            // 
            this.txtWantEPS.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtWantEPS.Location = new System.Drawing.Point(597, 350);
            this.txtWantEPS.Name = "txtWantEPS";
            this.txtWantEPS.ReadOnly = true;
            this.txtWantEPS.Size = new System.Drawing.Size(162, 34);
            this.txtWantEPS.TabIndex = 7;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(89, 346);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(131, 28);
            this.label14.TabIndex = 46;
            this.label14.Text = "每股收益(元)";
            // 
            // txtYetEPS
            // 
            this.txtYetEPS.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtYetEPS.Location = new System.Drawing.Point(226, 346);
            this.txtYetEPS.Name = "txtYetEPS";
            this.txtYetEPS.ReadOnly = true;
            this.txtYetEPS.Size = new System.Drawing.Size(162, 34);
            this.txtYetEPS.TabIndex = 45;
            // 
            // txtWantAmount
            // 
            this.txtWantAmount.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtWantAmount.Location = new System.Drawing.Point(597, 301);
            this.txtWantAmount.Name = "txtWantAmount";
            this.txtWantAmount.ReadOnly = true;
            this.txtWantAmount.Size = new System.Drawing.Size(162, 34);
            this.txtWantAmount.TabIndex = 6;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(435, 302);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(152, 28);
            this.label15.TabIndex = 49;
            this.label15.Text = "预测市值(亿元)";
            // 
            // txtSafeRate
            // 
            this.txtSafeRate.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSafeRate.Location = new System.Drawing.Point(597, 163);
            this.txtSafeRate.Name = "txtSafeRate";
            this.txtSafeRate.Size = new System.Drawing.Size(162, 34);
            this.txtSafeRate.TabIndex = 3;
            this.txtSafeRate.Text = "100";
            this.txtSafeRate.TextChanged += new System.EventHandler(this.txtSafeRate_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(458, 165);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(129, 28);
            this.label16.TabIndex = 51;
            this.label16.Text = "安全边际(%)";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(418, 258);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(173, 28);
            this.label17.TabIndex = 54;
            this.label17.Text = "净利润差值(亿元)";
            // 
            // txtLostNetProfit
            // 
            this.txtLostNetProfit.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLostNetProfit.Location = new System.Drawing.Point(597, 256);
            this.txtLostNetProfit.Name = "txtLostNetProfit";
            this.txtLostNetProfit.ReadOnly = true;
            this.txtLostNetProfit.Size = new System.Drawing.Size(162, 34);
            this.txtLostNetProfit.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(47, 214);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(173, 28);
            this.label8.TabIndex = 55;
            this.label8.Text = "去年净利润(亿元)";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label18.Location = new System.Drawing.Point(11, 258);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(215, 28);
            this.label18.TabIndex = 57;
            this.label18.Text = "剩余季度净利润(亿元)";
            // 
            // txtYetLostNetProfit
            // 
            this.txtYetLostNetProfit.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtYetLostNetProfit.Location = new System.Drawing.Point(226, 256);
            this.txtYetLostNetProfit.Name = "txtYetLostNetProfit";
            this.txtYetLostNetProfit.ReadOnly = true;
            this.txtYetLostNetProfit.Size = new System.Drawing.Size(162, 34);
            this.txtYetLostNetProfit.TabIndex = 56;
            // 
            // btnValuateAll
            // 
            this.btnValuateAll.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnValuateAll.Location = new System.Drawing.Point(123, 490);
            this.btnValuateAll.Name = "btnValuateAll";
            this.btnValuateAll.Size = new System.Drawing.Size(265, 42);
            this.btnValuateAll.TabIndex = 58;
            this.btnValuateAll.Text = "按去年业绩预测所有股票";
            this.btnValuateAll.UseVisualStyleBackColor = true;
            this.btnValuateAll.Click += new System.EventHandler(this.btnValuateAll_Click);
            // 
            // txtAdvise
            // 
            this.txtAdvise.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtAdvise.Location = new System.Drawing.Point(597, 442);
            this.txtAdvise.Name = "txtAdvise";
            this.txtAdvise.ReadOnly = true;
            this.txtAdvise.Size = new System.Drawing.Size(162, 34);
            this.txtAdvise.TabIndex = 59;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label19.Location = new System.Drawing.Point(449, 444);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(138, 28);
            this.label19.TabIndex = 60;
            this.label19.Text = "预测结果推荐";
            // 
            // ValuateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 553);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.txtAdvise);
            this.Controls.Add(this.btnValuateAll);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.txtYetLostNetProfit);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.txtLostNetProfit);
            this.Controls.Add(this.txtSafeRate);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.txtWantAmount);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtWantEPS);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtYetEPS);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.txtYetGrow);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtWantProfitGrow);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtYetNetProfit);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtTarget);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtWantPE);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTTM);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtCapital);
            this.Controls.Add(this.txtWantNetProfit);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtNetProfit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ValuateForm";
            this.Text = "预测估值";
            this.Load += new System.EventHandler(this.ValuateForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtNetProfit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtWantNetProfit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCapital;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTTM;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtWantPE;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTarget;
        private System.Windows.Forms.TextBox txtYetNetProfit;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtWantProfitGrow;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtYetGrow;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtWantEPS;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtYetEPS;
        private System.Windows.Forms.TextBox txtWantAmount;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtSafeRate;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtLostNetProfit;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtYetLostNetProfit;
        private System.Windows.Forms.Button btnValuateAll;
        private System.Windows.Forms.TextBox txtAdvise;
        private System.Windows.Forms.Label label19;
    }
}